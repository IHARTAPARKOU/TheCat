using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using DataAccess.Services.Abstractions;

namespace DataAccess.Repositories;

public class BreedsRepository(HttpClient httpClient, IConfigurationService configurationService)
    : BaseApiRepository<CatBreedDto>(httpClient, configurationService), IBreedsRepository
{
    private readonly string baseUri = configurationService.TheCatApiBaseUri + "/breeds";
    protected override string BaseUri => baseUri;
    protected override bool IsNeedToCache => true;

    public async Task<IEnumerable<CatBreedDto>> GetWithLimitAsync(int limit, int page, CancellationToken token)
    {
        return await RequestCollectionAsync($"?limit={limit}&page={page}", token);
    }

    public async Task<IEnumerable<CatBreedDto>> GetFactsAsync(string id, CancellationToken token)
    {
        return await RequestCollectionAsync($"/{id}/facts", token);
    }

    public async Task<IEnumerable<CatBreedDto>> SearchAsync(string name, CancellationToken token)
    {
        return await SearchAsync(name, attachImage: false, token);
    }

    public async Task<IEnumerable<CatBreedDto>> SearchAsync(string name, bool attachImage, CancellationToken token)
    {
        var findResult = Cache.Values.Where(b => b.Name?.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
        if (findResult.Length > 0)
        {
            return findResult;
        }

        return await RequestCollectionAsync($"/search?q={name}&attach_image={Convert.ToInt32(attachImage)}", token);
    }
}
