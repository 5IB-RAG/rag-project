namespace server.Model
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Choice
    {
        public ContentFilterResults content_filter_results { get; set; }
        public string finish_reason { get; set; }
        public int index { get; set; }
        public object logprobs { get; set; }
        public OpenAiMessage message { get; set; }
    }

    public class ContentFilterResults
    {
        public Hate hate { get; set; }
        public SelfHarm self_harm { get; set; }
        public Sexual sexual { get; set; }
        public Violence violence { get; set; }
    }

    public class Hate
    {
        public bool filtered { get; set; }
        public string severity { get; set; }
    }

    public class OpenAiMessage
    {
        public string content { get; set; }
        public string role { get; set; }
    }

    public class PromptFilterResult
    {
        public int prompt_index { get; set; }
        public ContentFilterResults content_filter_results { get; set; }
    }

    public class ChatResponse
    {
        public List<Choice> choices { get; set; }
        public int created { get; set; }
        public string id { get; set; }
        public string model { get; set; }
        public string @object { get; set; }
        public List<PromptFilterResult> prompt_filter_results { get; set; }
        public string system_fingerprint { get; set; }
        public Usage usage { get; set; }
    }

    public class SelfHarm
    {
        public bool filtered { get; set; }
        public string severity { get; set; }
    }

    public class Sexual
    {
        public bool filtered { get; set; }
        public string severity { get; set; }
    }

    public class Usage
    {
        public int completion_tokens { get; set; }
        public int prompt_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class Violence
    {
        public bool filtered { get; set; }
        public string severity { get; set; }
    }


}
