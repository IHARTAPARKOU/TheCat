using TheCatApp.Models;

namespace TheCatApp.Infrastructure.Services.Abstractions;

internal interface IBreedsService
{
    Task<IReadOnlyCollection<CatBreed>> GetWithLimitAsync(int limit, int page, CancellationToken token);
    Task<IReadOnlyCollection<CatBreed>> SearchAsync(string name, CancellationToken token);
    Task<bool> InsertOrUpdateAsync(CatBreed catBreed, CancellationToken token);
    Task SaveChangesAsync(CancellationToken token);
}