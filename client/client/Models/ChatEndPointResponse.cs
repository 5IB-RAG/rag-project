using client.Model.Dto;

namespace client.Models
{
    public class ChatEndPointResponse
    {
        public string responseMessage { get; set; }
        public List<DocumentDto> usedDocuments { get; set; }
    }
}
