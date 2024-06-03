using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using server.Db;
using server.Embedding;
using server.Enum;
using server.Model;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace server.Chat
{

    public class MessagePair
    {
        public string User { get; set; } = string.Empty;
        public string Assistant { get; set; } = string.Empty;
    }
    
    public class ChatRequest {
        public List<OpenAiMessage> messages { get; set; }
    }

    public class ChatContext
    {
        public ChatRequest ChatRequest { get; set; }
        public List<DocumentChunk> UsedChunk { get; set; }
    }

    public class ChatEndPointResponse
    {
        public string assistantMessage { get; set; }
        public List<DocumentChunk> documentChunks { get; set; } //Trasformarli in dto
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

        public async Task<ChatEndPointResponse> SendRequest(int chatId, int userId, string text)
        {
            Message message = new ()
            {
                Text = text,
                ChatId = chatId,
                Role = ChatRole.User,
                Embedding = (await embedding.GetChunkEmbeddingAsync([DocumentChunk.Builder().Text(text).Build()]))[0]
            };

            var savedMessage = _context.Messages.Add(message);
            var dbSaving = _context.SaveChangesAsync();

            ChatContext chatContext = await CreateChatContext(savedMessage.Entity, userId);
            string chatResponse = await SendRequestToChat(JsonSerializer.Serialize(chatContext.ChatRequest));

            await dbSaving; //Sicurezza sicurosa

            //salvataggio sul db della risposta di chat
            Message responseChat = new()
            {
                Text = chatResponse,
                ResponseId = savedMessage.Entity.Id,
                ChatId = chatId,
                Role = ChatRole.Assistant
            };

            _context.Messages.Add(responseChat);
            await _context.SaveChangesAsync();

            return new ChatEndPointResponse() { assistantMessage = chatResponse, documentChunks = chatContext.UsedChunk};
        }
        public async Task<ChatContext> CreateChatContext(Message userMessage, int userId)
        {
            List<MessagePair>? chatContext = new List<MessagePair>();
            List<MessagePair>? similarMessages = new List<MessagePair>();
            List<MessagePair>? lastMessages = await GetLast3Messages(userMessage.ChatId, userId);
            if (lastMessages.Count != 0)
            {
                similarMessages = await GetSimilarMessages(userMessage, lastMessages, userId);
                if (similarMessages != null) similarMessages.ForEach(chatContext.Add);
            }
            
            lastMessages.ForEach(chatContext.Add);
            MessagePair m = new MessagePair();
            m.User = userMessage.Text;
            chatContext.Add(m);

            List<OpenAiMessage> messages = new List<OpenAiMessage>();
            //

            List<DocumentChunk> similarDocumentChunks = await GetSimilarDocumentChunks(userMessage.Embedding, userId);

            if (similarDocumentChunks.Count > 0)
            {
                messages.Add(new OpenAiMessage { role = "system", content = "Sei un assistente che risponde alle domande con le informazioni a te fornite." });
                string documentChunksText = string.Join("\n", similarDocumentChunks.Select(dc => dc.Text));
                messages.Add(new OpenAiMessage { role = "system", content = documentChunksText });
            }
            //

            foreach (var messagePair in chatContext)
            {
                if (!string.IsNullOrEmpty(messagePair.User))
                    messages.Add(new OpenAiMessage { role = "user", content = messagePair.User });
                if (!string.IsNullOrEmpty(messagePair.Assistant))
                    messages.Add(new OpenAiMessage { role = "assistant", content = messagePair.Assistant });
            }

            var chatRequest = new ChatRequest() { messages = messages };

            return new ChatContext() { ChatRequest = chatRequest, UsedChunk = similarDocumentChunks };

        }
        private async Task<List<DocumentChunk>> GetSimilarDocumentChunks(Vector embedding, int userId)//ok
        {
            if (embedding == null)
                throw new ArgumentException("The provided embedding is null.");

            List<DocumentChunk>? similarDocumentChunks = await _context.DocumentChunks
                .Where(dc => dc.Document.UserId == userId)
                .Select(dc => new { DocumentChunk = dc, Distance = embedding.L2Distance(dc.Embedding) })
                .OrderBy(obj => obj.Distance)
                .Take(5) // Prendi i 5 chunk più simili
                .Select(obj => obj.DocumentChunk)
                .ToListAsync();

            return similarDocumentChunks;
        }

        /// <summary>
        /// Restituisce gli ultimi 3 messaggi (user e assistant) di una determinata chat id
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<List<MessagePair>> GetLast3Messages(int chatId, int userId)//ok
        {
            List<Message>? messages = await _context.Messages
                .OrderByDescending(m => m.Id)
                .Where(m => m.Role == ChatRole.User && m.ChatId == chatId && m.UserChat.UserId == userId)
                .Take(3)
                .ToListAsync();

            messages = messages.OrderBy(m => m.Id).ToList();
            List<Message> messagesWithResponses = await AddResponseToMessage(messages, chatId, userId);
            List<MessagePair> messagePairs = GenerateChatJson(messagesWithResponses);

            return messagePairs;
        }

        /// <summary>
        /// Prende i 2 messaggi piu simili al messaggio passato al metodo diversi dai 3 recenti
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<List<MessagePair>> GetSimilarMessages(Message message, List<MessagePair>? lastMessages, int userId)//ok
        {
            if (message.Embedding == null)
                throw new ArgumentException("The provided message does not have an embedding.");

            List<string> lastMessagesTexts = lastMessages.Select(lm => lm.User).ToList();

            List<Message>? similarUserMessages = _context.Messages
            .Where(mess => mess.ChatId == message.ChatId && mess.Id != message.Id && mess.Role == ChatRole.User && mess.UserChat.UserId == userId)
            .Select(m => new { Message = m, Distance = message.Embedding.L2Distance(m.Embedding) })
            .Where(obj => !lastMessagesTexts.Contains(obj.Message.Text)) // Escludi i messaggi con lo stesso testo
            .OrderBy(obj => obj.Distance)
            .Take(2)
            .Select(obj => obj.Message)
            .ToList();

            if (similarUserMessages.Count == 0)//il messaggio non ha somiglianze
                return null;
            List<Message> messagesWithResponses = await AddResponseToMessage(similarUserMessages, message.ChatId, userId);
            List<MessagePair> messagePairs = GenerateChatJson(messagesWithResponses);

            return messagePairs;
        }

        private async Task<List<Message>> AddResponseToMessage(List<Message> userMessages, int chatId, int userId)//ok
        {
            var userMessagesId = userMessages.Select(m => m.Id).ToList();

            List<Message>? responses = await _context.Messages
            .Where(m => m.ResponseId.HasValue && userMessagesId.Contains(m.ResponseId.Value) && m.ChatId == chatId && m.UserChat.UserId == userId)
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
            if (chatContext.IsNullOrEmpty())
                return new List<MessagePair>();

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
        public async Task<List<Message>> GetConversation(int chatId, int userId)//ok
        {
            return await _context.UserChats.Where(c => c.Id == chatId && c.UserId == userId).SelectMany(m => m.Messages).ToListAsync();
        }

        /// <summary>
        /// Restituisce tutte le chat di un determinato utente
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<UserChat>?> GetUserChats(User user)//ok
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
