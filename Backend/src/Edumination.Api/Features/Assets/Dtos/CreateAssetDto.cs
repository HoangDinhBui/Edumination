namespace Edumination.Api.Features.Assets.Dtos;

public class CreateAssetDto
{
    public string Kind { get; set; } = string.Empty; // VIDEO, AUDIO, IMAGE, DOC, SUBTITLE, TRANSCRIPT, OTHER
    public string MediaType { get; set; } = string.Empty; // e.g., application/pdf
    public long ByteSize { get; set; }
}