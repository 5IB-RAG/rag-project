using System.ComponentModel.DataAnnotations;

namespace client.Services
{
    public class ApiService
    {
        public string BaseAddress { get; } = "http://localhost:5000";

        public string Documents { get; }
        public string Chats { get; }
        public string Message { get; }
        public string Login { get; }
        public string SignUp { get; }

        public ApiService() {
            Documents = BaseAddress + "/document";
            Chats = BaseAddress + "";
            Message = BaseAddress + "";
            Login = BaseAddress + "/auth/login";
            SignUp = BaseAddress + "/auth/signup";
        }
    }
}
