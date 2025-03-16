using System.Text.Json.Serialization;
using DataAccess.Entities.Abstractions;

namespace DataAccess.Entities;

public class CatBreedDto : IBaseEntity
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("cfa_url")]
    public string? CfaUrl { get; init; }

    [JsonPropertyName("vetstreet_url")]
    public string? VetstreetUrl { get; init; }

    [JsonPropertyName("vcahospitals_url")]
    public string? VcaHospitalsUrl { get; init; }

    [JsonPropertyName("temperament")]
    public string? Temperament { get; init; }

    [JsonPropertyName("origin")]
    public string? Origin { get; init; }

    [JsonPropertyName("country_codes")]
    public string? CountryCodes { get; init; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("life_span")]
    public string? LifeSpan { get; init; }

    [JsonPropertyName("indoor")]
    public int Indoor { get; init; }

    [JsonPropertyName("lap")]
    public int Lap { get; init; }

    [JsonPropertyName("alt_names")]
    public string? AltNames { get; init; }

    [JsonPropertyName("adaptability")]
    public int Adaptability { get; init; }

    [JsonPropertyName("affection_level")]
    public int AffectionLevel { get; init; }

    [JsonPropertyName("child_friendly")]
    public int ChildFriendly { get; init; }

    [JsonPropertyName("dog_friendly")]
    public int DogFriendly { get; init; }

    [JsonPropertyName("energy_level")]
    public int EnergyLevel { get; init; }

    [JsonPropertyName("grooming")]
    public int Grooming { get; init; }

    [JsonPropertyName("health_issues")]
    public int HealthIssues { get; init; }

    [JsonPropertyName("intelligence")]
    public int Intelligence { get; init; }

    [JsonPropertyName("shedding_level")]
    public int SheddingLevel { get; init; }

    [JsonPropertyName("social_needs")]
    public int SocialNeeds { get; init; }

    [JsonPropertyName("stranger_friendly")]
    public int StrangerFriendly { get; init; }

    [JsonPropertyName("vocalisation")]
    public int Vocalisation { get; init; }

    [JsonPropertyName("experimental")]
    public int Experimental { get; init; }

    [JsonPropertyName("hairless")]
    public int Hairless { get; init; }

    [JsonPropertyName("natural")]
    public int Natural { get; init; }

    [JsonPropertyName("rare")]
    public int Rare { get; init; }

    [JsonPropertyName("rex")]
    public int Rex { get; init; }

    [JsonPropertyName("suppressed_tail")]
    public int SuppressedTail { get; init; }

    [JsonPropertyName("short_legs")]
    public int ShortLegs { get; init; }

    [JsonPropertyName("wikipedia_url")]
    public string? WikipediaUrl { get; init; }

    [JsonPropertyName("hypoallergenic")]
    public int Hypoallergenic { get; init; }

    [JsonPropertyName("reference_image_id")]
    public string? ReferenceImageId { get; init; }

    [JsonPropertyName("image")]
    public CatImageDto? Image { get; init; }

    [JsonPropertyName("weight")]
    public WeightDto? Weight { get; init; }

    public class WeightDto
    {
        [JsonPropertyName("imperial")]
        public string? Imperial { get; init; }

        [JsonPropertyName("metric")]
        public string? Metric { get; init; }
    }
}