using System.Text.Json.Serialization;
using DataAccess.Entities.Abstractions;

namespace DataAccess.Entities;

public class CatImageDto : IBaseEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("width")]
    public int Width { get; init; }

    [JsonPropertyName("height")]
    public int Height { get; init; }

    [JsonPropertyName("url")]
    public string? Url { get; init; }
}
