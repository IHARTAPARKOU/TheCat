using DataAccess.Entities;

namespace DataAccess.Repositories.Abstractions;

public interface IBreedsRepository
{
    Task<IEnumerable<CatBreed>> GetWithLimitAsync(int limit, int page, CancellationToken token);
    Task<IEnumerable<CatBreed>> GetFactsAsync(string id, CancellationToken token);
    Task<IEnumerable<CatBreed>> SearchAsync(string name, CancellationToken token);
    Task<IEnumerable<CatBreed>> SearchAsync(string name, bool attachImage, CancellationToken token);
}
