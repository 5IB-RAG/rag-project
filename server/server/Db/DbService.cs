using client.Services;

namespace client.Db
{
    public class DbService : IService
    {
        private PgVectorContext dbContext;

        public void Disable()
        {

        }

        public void Enable(WebApplication app)
        {
           
        }

        public void PreLoad(WebApplicationBuilder builder)
        {
            builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
            dbContext = new PgVectorContext();
            builder.Services.AddSingleton(() => dbContext);
        }
    }
}
