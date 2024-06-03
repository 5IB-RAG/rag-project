using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using server.Db;
using server.Embedding;
using server.Enum;
using server.Model;
using System.Text;
using System.Text.Json;

namespace server.Chat
{

    public class MessagePair
    {
        public string User { get; set; } = string.Empty;
        public string Assistant { get; set; } = string.Empty;
    }

    public class ChatService : IChat
    {
        private HttpClient client = new();
        private OpenAiParameters? chatParameters = new();
        private string? urlChat;

        private PgVectorContext _context;
        private EmbeddingService embedding;

        public void Disable()
        {
            //throw new NotImplementedException();
        }
        public async void Enable(WebApplication app)
        {

            //save sul db del messaggio

            //throw new NotImplementedException();
        }
        public void PreLoad(WebApplicationBuilder builder, IServiceProvider provider)
        {
            _context = provider.GetService<PgVectorContext>() ?? throw new ApplicationException();
            embedding = provider.GetService<EmbeddingService>() ?? throw new ApplicationException();

            chatParameters = builder.Configuration.GetSection("ChatParameters").Get<OpenAiParameters>();
            urlChat = $"https://{chatParameters.ResourceName}.openai.azure.com/openai/deployments/{chatParameters.DeploymentId}/chat/completions?api-version={chatParameters.ApiVersion}";
            //throw new NotImplementedException();
        }

        public async Task<string> SendRequest()
        {
            int chatId = 1;
            Message m = new Message();
            m.Text = "no, non penso che lo farò";
            m.ChatId = chatId;
            m.Role = ChatRole.User;
            m.Embedding = (await embedding.GetChunkEmbeddingAsync([DocumentChunk.Builder().Text(m.Text).Build()]))[0];
            var messageSalvato = _context.Messages.Add(m);

            string messageJsonChat = CreateChatContext(messageSalvato.Entity).Result;
            string chatResponse = await SendRequestToChat(messageJsonChat);


            //salvataggio sul db della risposta di chat
            Message resposeChat = new Message();
            m.Text = chatResponse;
            m.ResponseId = messageSalvato.Entity.Id;
            m.ChatId = chatId;
            m.Role = ChatRole.Assistant;

            //_context.Messages.Add(resposeChat);
            //await _context.SaveChangesAsync();
            _context.Messages.Remove(m);

            return chatResponse;
        }
        public async Task<string> CreateChatContext(Message userMessage)
        {
            List<MessagePair>? chatContext = new List<MessagePair>();
            List<MessagePair>? similarMessages = new List<MessagePair>();
            List<MessagePair>? lastMessages = await GetLast3Messages(userMessage.ChatId);
            if (lastMessages.Count != 0)
                similarMessages = await GetSimilarMessages(userMessage, lastMessages);

            similarMessages.ForEach(chatContext.Add);
            lastMessages.ForEach(chatContext.Add);
            MessagePair m = new MessagePair();
            m.User = userMessage.Text;
            chatContext.Add(m);

            List<OpenAiMessage> messages = new List<OpenAiMessage>();

            foreach (var messagePair in chatContext)
            {
                if (!string.IsNullOrEmpty(messagePair.User))
                    messages.Add(new OpenAiMessage { role = "user", content = messagePair.User });
                if (!string.IsNullOrEmpty(messagePair.Assistant))
                    messages.Add(new OpenAiMessage { role = "assistant", content = messagePair.Assistant });
            }


            string context = JsonSerializer.Serialize(messages, new JsonSerializerOptions { WriteIndented = true });
            return context;

        }
        /// <summary>
        /// Restituisce gli ultimi 3 messaggi (user e assistant) di una determinata chat id
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<List<MessagePair>> GetLast3Messages(int chatId)
        {
            List<Message>? messages = await _context.Messages
                .OrderByDescending(m => m.Id)
                .Where(m => m.Role == ChatRole.User && m.ChatId == chatId)
                .Take(3)
                .ToListAsync();

            messages = messages.OrderBy(m => m.Id).ToList();
            List<Message> messagesWithResponses = await AddResponseToMessage(messages, chatId);
            List<MessagePair> messagePairs = GenerateChatJson(messagesWithResponses);

            return messagePairs;
        }

        /// <summary>
        /// Prende i 2 messaggi piu simili al messaggio passato al metodo diversi dai 3 recenti
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<MessagePair>> GetSimilarMessages(Message message, List<MessagePair>? lastMessages)
        {
            if (message.Embedding == null)
                throw new ArgumentException("The provided message does not have an embedding.");

            List<string> lastMessagesTexts = lastMessages.Select(lm => lm.User).ToList();

            List<Message>? similarUserMessages = _context.Messages
            .Where(mess => mess.ChatId == message.ChatId && mess.Id != message.Id && mess.Role == ChatRole.User)
            .Select(m => new { Message = m, Distance = message.Embedding.L2Distance(m.Embedding) })
            .Where(obj => !lastMessagesTexts.Contains(obj.Message.Text)) // Escludi i messaggi con lo stesso testo
            .OrderBy(obj => obj.Distance)
            .Take(2)
            .Select(obj => obj.Message)
            .ToList();

            if (similarUserMessages.Count == 0)//il messaggio non ha somiglianze
                return null;
            List<Message> messagesWithResponses = await AddResponseToMessage(similarUserMessages, message.ChatId);
            List<MessagePair> messagePairs = GenerateChatJson(messagesWithResponses);

            return messagePairs;
        }

        private async Task<List<Message>> AddResponseToMessage(List<Message> userMessages, int chatId)
        {
            var userMessagesId = userMessages.Select(m => m.Id).ToList();

            List<Message>? responses = await _context.Messages
            .Where(m => m.ResponseId.HasValue && userMessagesId.Contains(m.ResponseId.Value) && m.ChatId == chatId)
            .ToListAsync();

            for (int i = 0; i < userMessages.Count; i++)
            {
                Message userMessage = userMessages[i];
                Message? response = responses.FirstOrDefault(r => r.ResponseId == userMessage.Id);
                if (response != null)
                {
                    userMessages.Insert(i + 1, response);
                    i++;
                }
            }

            return userMessages;
        }

        /// <summary>
        /// Prende in ingresso la lista<message> e restituisce la lista indentato come relazione "User":"Assistant"
        /// </summary>
        /// <param name="chatContext"></param>
        /// <returns></returns>
        public List<MessagePair> GenerateChatJson(List<Message> chatContext)
        {
            if (chatContext.Count == 0)
                return default;

            List<MessagePair> messagePairs = new List<MessagePair>();

            for (int i = 0; i < chatContext.Count; i += 2)
            {
                if (i + 1 < chatContext.Count)
                {
                    Message userMessage = chatContext[i];
                    Message assistantMessage = chatContext[i + 1];

                    if (userMessage.Role == ChatRole.User && assistantMessage.Role == ChatRole.Assistant)
                    {
                        messagePairs.Add(new MessagePair
                        {
                            User = userMessage.Text,
                            Assistant = assistantMessage.Text
                        });
                    }
                }
            }

            return messagePairs;
        }

        /// <summary>
        /// Restituisce tutta la conversazione di un determinata chat
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Message>> GetConversation(int chatId)
        {
            return await _context.UserChats.Where(c => c.Id == chatId).SelectMany(m => m.Messages).ToListAsync();
        }

        /// <summary>
        /// Restituisce tutte le chat di un determinato utente
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<UserChat>?> GetUserChats(User user)
        {
            return await _context.UserChats.Where(us => us.UserId == user.Id).ToListAsync();
        }

        public async Task<string> SendRequestToChat(string chat)
        {
            client.DefaultRequestHeaders.Add("api-key", chatParameters.ApiKey);

            var content = new StringContent(chat, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(urlChat, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ChatResponse>();
                string riposta = responseContent.choices[0].message.content;
                return riposta;
            }

            throw new Exception("pippo");
        }
        public Task SendAsync(User user, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
