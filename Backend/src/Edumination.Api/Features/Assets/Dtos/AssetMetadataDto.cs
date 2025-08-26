using System;

namespace Edumination.Api.Features.Assets.Dtos;

public class AssetMetadataDto
{
    public long Id { get; set; }
    public string Kind { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public long? ByteSize { get; set; }
    public string StorageUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}