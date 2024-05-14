namespace client.Model
{
    public class EmbeddingResponse
    {
        public string Object { get; set; }
        public List<EmbeddingData> Data { get; set; }
        public string Model { get; set; }
        public EmbeddingUsage Usage { get; set; }
    }
}
