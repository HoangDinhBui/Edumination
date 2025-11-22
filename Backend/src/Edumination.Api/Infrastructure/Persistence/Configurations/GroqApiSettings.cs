namespace Edumination.Api.Infrastructure.Persistence.Configurations;

public class GroqApiSettings
{
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
    public string Model { get; set; }
    public string ChatModel { get; set; }
    public int MaxTokens { get; set; }
    public double Temperature { get; set; }
}