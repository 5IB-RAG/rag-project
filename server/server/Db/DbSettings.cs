using Microsoft.EntityFrameworkCore;

namespace client.Db
{
    public class DbSettings : DbContext
    {
        public string Server { get; set; }
        public string Database {  get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

    }
}
