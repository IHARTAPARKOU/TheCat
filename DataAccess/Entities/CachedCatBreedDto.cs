using DataAccess.Entities.Abstractions;

namespace DataAccess.Entities;

public class CachedCatBreedDto : IBaseEntity
{
    public string? Id { get; init; }
    public string? Description { get; init; }
    public bool IsFavorite { get; init; }
}