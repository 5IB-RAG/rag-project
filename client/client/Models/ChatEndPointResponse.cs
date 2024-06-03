using client.Model.Dto;

namespace client.Models
{
    public class ChatEndPointResponse
    {
        public string assistantMessage { get; set; }
        public List<DocumentChunkDto> documentChunks { get; set; }
    }
}
