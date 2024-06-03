namespace server.Model;

public class OpenAiParameters
{
    public string ResourceName { get; set; }
    public string ApiKey { get; set; }
    public string DeploymentId { get; set; }
    public string ApiVersion { get; set; }
}