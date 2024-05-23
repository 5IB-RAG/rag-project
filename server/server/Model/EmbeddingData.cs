namespace client.Model
{
    public class EmbeddingData
    {
        public string Object { get; set; }
        public int Index { get; set; }
        public List<float> Embedding { get; set; }
    }
}
