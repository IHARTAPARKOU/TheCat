using DataAccess.Entities;

namespace DataAccess.Repositories.Abstractions;

public interface IBreedsRepository : IBaseApiRepository<CatBreedDto>
{
    Task<IEnumerable<CatBreedDto>> GetWithLimitAsync(int limit, int page, CancellationToken token);
    Task<IEnumerable<CatBreedDto>> GetFactsAsync(string id, CancellationToken token);
    Task<IEnumerable<CatBreedDto>> SearchAsync(string name, CancellationToken token);
    Task<IEnumerable<CatBreedDto>> SearchAsync(string name, bool attachImage, CancellationToken token);
}
