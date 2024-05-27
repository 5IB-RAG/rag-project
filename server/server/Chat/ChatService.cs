using Microsoft.EntityFrameworkCore;
using server.Db;
using server.Embedding;
using server.Model;

namespace server.Chat
{
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
            Message m = new Message();
            _context.Messages.Add(m);
            _context.SaveChangesAsync();

            CreateChatContext(m);
            //throw new NotImplementedException();
        }
        public async Task<List<Message>> CreateChatContext(Message userMessage)
        {
            List<Message>? chatContext = new List<Message>();
            Message messaggioUtente = new Message();
            var similarMessages = GetSimilarMessages(userMessage);
            similarMessages.ForEach(chatContext.Add);

            var lastMessages = await GetLast3Messages(userMessage.ChatId);
            lastMessages.ForEach(chatContext.Add);

            return chatContext;
        }
        public async Task<List<Message>?> GetLast3Messages(int chatId)
        {
            List<Message> messaggi = (List<Message>)(await _context.UserChats.FindAsync(chatId)).Messages.ToList().OrderBy(m => m.Id);
            messaggi = (List<Message>)messaggi.Take(6);
            return messaggi;
        }

        public List<Message>? GetSimilarMessages(Message message)
        {
            if (message.Embedding == null)
                throw new ArgumentException("The provided message does not have an embedding.");

            string targetVectorString = $"[{string.Join(", ", message.Embedding)}]";

            FormattableString query = $"SELECT * FROM Messages WHERE chatId={message.ChatId} ORDER BY Embedding <-> '{targetVectorString}'LIMIT 3";

            List<Message> similarMessages = _context.Messages.FromSql(query).ToList();
            similarMessages.RemoveAt(0);

            return similarMessages;
        }

        public string CreateJsonChatContext()
        {

            return default;
        }

        public async Task<List<Message>> GetConversation(int userId)
        {
            return await _context.UserChats.Where(c => c.UserId == userId).SelectMany(m => m.Messages).ToListAsync();
        }

        public Task<UserChat[]> GetUserChats(User user)
        {
            throw new NotImplementedException();
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
