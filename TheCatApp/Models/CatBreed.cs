using TheCatApp.Models.Abstractions;

namespace TheCatApp.Models;

public class CatBreed : BaseModel
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    private string? description;
    public string? Description
    { 
        get => description;
        set => SetValue(ref description, value);
    }
    public string? Origin { get; set; }
    public string? LifeSpan { get; set; }
    public int HealthIssues { get; set; }
    public int Intelligence { get; set; }
    public int SheddingLevel { get; set; }
    public string? PhotoUrl { get; set; }
    public string? WikipediaUrl { get; set; }
    private bool isFavorite;
    public bool IsFavorite
    { 
        get => isFavorite;
        set => SetValue(ref isFavorite, value);
    }
}
