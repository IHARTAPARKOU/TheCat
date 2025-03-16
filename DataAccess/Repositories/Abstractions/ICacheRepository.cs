using System.Diagnostics.CodeAnalysis;
using DataAccess.Entities;

namespace DataAccess.Repositories.Abstractions;

public interface ICacheRepository
{
    bool TryGetLocalValue(string id, [MaybeNullWhen(false)] out CachedCatBreedDto? item);
    Task<IEnumerable<CachedCatBreedDto>> GetAllAsync(CancellationToken token);
    Task<bool> InsertOrUpdateAsync(CachedCatBreedDto item, CancellationToken token);
    Task<bool> DeleteAsync(string id, CancellationToken token);
    Task SaveChangesAsync(CancellationToken token);
}