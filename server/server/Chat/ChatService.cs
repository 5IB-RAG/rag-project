using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using server.Db;
using server.Embedding;
using server.Enum;
using server.Model;
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

        public ChatService(IServiceProvider provider)
        {
            _context = provider.GetService<PgVectorContext>() ?? throw new ApplicationException();
            embedding = provider.GetService<EmbeddingService>() ?? throw new ApplicationException();
        }
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
            m.Embedding = await embedding.GetContextChunk(m);
            var messageSalvato = _context.Messages.Add(m);

            await _context.SaveChangesAsync();

            string messageJsonChat = GenerateChatJson(await CreateChatContext(messageSalvato.Entity));
        }
        public async Task<List<Message>?> CreateChatContext(Message userMessage)
        {
            List<Message>? chatContext = new List<Message>();
            List<Message>? similarMessages = GetSimilarMessages(userMessage);
            if (similarMessages.Count!=0)
            {
                similarMessages.ForEach(chatContext.Add);

                List<Message>? lastMessages = await GetLast3Messages(userMessage.ChatId);
                lastMessages.ForEach(chatContext.Add);
            }
            
            return chatContext;
        }
        /// <summary>
        /// Restituisce gli ultimi 3 messaggi (user e assistant) di una determinata chat id
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<List<Message>?> GetLast3Messages(int chatId)
        {
            List<Message>? messaggi = (List<Message>?)(await _context.UserChats.FindAsync(chatId)).Messages.ToList().OrderByDescending(m => m.Id).Take(6);
            return messaggi;
        }

        /// <summary>
        /// Prende i 2 messaggi piu simili al messaggio passato al metodo
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<Message>? GetSimilarMessages(Message message)
        {
            if (message.Embedding == null)
                throw new ArgumentException("The provided message does not have an embedding.");
            
            List<Message>? similarMessages = new List<Message>();

            List<Message>? similarUserMessages = _context.Messages
            .Where(mess => mess.ChatId == message.ChatId && mess.Id != message.Id && mess.Role == ChatRole.USER)
            .Select(m => new { Message = m, Distance = m.Embedding.L2Distance(m.Embedding) })
            .OrderBy(obj => obj.Distance)
            .Take(2)
            .Select(obj => obj.Message)
            .ToList();

            if (similarUserMessages.Count==0)//il messaggio non ha somiglianze
                return similarMessages;

            var similarMessageIds = similarUserMessages.Select(m => m.Id).ToList();//prende gli id dei messaggi selezionati

            //Trova le risposte immediate per ciascun messaggio simile
            var responses = _context.Messages
                .Where(m => m.ChatId == message.ChatId && m.Role == ChatRole.ASSISTANT && similarMessageIds.Contains(m.Id - 1))
                .ToList();

            foreach (Message? userMessage in similarUserMessages)
            {
                similarMessages.Add(userMessage);
                var response = responses.FirstOrDefault(r => r.Id == userMessage.Id + 1);
                if (response != null)
                    similarMessages.Add(response);
            }

            return similarMessages;
        }
        
        /// <summary>
        /// Prende in ingresso la lista<message> e restituisce il json indentato come relazione "User":"Assistant"
        /// </summary>
        /// <param name="chatContext"></param>
        /// <returns></returns>
        public static string GenerateChatJson(List<Message>? chatContext)
        {
            if (chatContext.Count ==0)
                return string.Empty;

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

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(messagePairs, options);
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

        public void PreLoad(WebApplicationBuilder builder)
        {
            //throw new NotImplementedException();
        }

        public Task SendAsync(User user, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
