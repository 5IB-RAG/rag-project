using System.ComponentModel.DataAnnotations;

namespace client.Services
{
    public class ApiService
    {
        public string BaseAddress { get; } = "";
        public string DocumentsGet { get; }
        public string DocumentGetById { get; }

        public ApiService() {
            DocumentsGet = BaseAddress + "";
            DocumentGetById = BaseAddress + "";
        }
    }
}
