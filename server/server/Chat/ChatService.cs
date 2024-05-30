using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using server.Db;
using server.Embedding;
using server.Enum;
using server.Model;
using System.Runtime.InteropServices.Marshalling;
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

        public async void Ciao()
        {
            Message m = new Message();
            m.Text = "no, non penso che lo farò";
            m.ChatId = 1;
            m.Embedding = (await embedding.GetChunkEmbeddingAsync([DocumentChunk.Builder().Text(m.Text).Build()]))[0];
            var messageSalvato = _context.Messages.Add(m);

            //await _context.SaveChangesAsync();
            string messageJsonChat = CreateChatContext(messageSalvato.Entity).Result;
            _context.Messages.Remove(m);
        }
        public async Task<string> CreateChatContext(Message userMessage)
        {
            List<MessagePair>? chatContext = new List<MessagePair>();
            List<MessagePair>? similarMessages = new List<MessagePair>();
            List<MessagePair>? lastMessages = await GetLast3Messages(userMessage.ChatId);
            if (lastMessages.Count != 0)
                similarMessages = await GetSimilarMessages(userMessage,lastMessages);                

            similarMessages.ForEach(chatContext.Add);
            lastMessages.ForEach(chatContext.Add);
            MessagePair m = new MessagePair();
            m.User = userMessage.Text;
            chatContext.Add(m);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(chatContext, options);
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
                .Where(m => m.Role == ChatRole.USER && m.ChatId == chatId)
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
            .Where(mess => mess.ChatId == message.ChatId && mess.Id != message.Id && mess.Role == ChatRole.USER)
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

                    if (userMessage.Role == ChatRole.USER && assistantMessage.Role == ChatRole.ASSISTANT)
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

        public void PreLoad(WebApplicationBuilder builder, IServiceProvider provider)
        {
            _context = provider.GetService<PgVectorContext>() ?? throw new ApplicationException();
            embedding = provider.GetService<EmbeddingService>() ?? throw new ApplicationException();
            //throw new NotImplementedException();
        }

        public Task SendAsync(User user, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
