using AutoMapper;
using DataAccess.Entities;
using DataAccess.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using TheCatApp.Infrastructure.Services.Abstractions;
using TheCatApp.Models;

namespace TheCatApp.Infrastructure.Services;

internal class BreedsService(IBreedsRepository breedsRepository, IImagesRepository imagesRepository,
    ICacheRepository cacheRepository, IMapper mapper, ILogger<BreedsService> logger) : IBreedsService
{
    public async Task<IReadOnlyCollection<CatBreed>> GetWithLimitAsync(int limit, int page, CancellationToken token)
    {
        try
        {
            var breedDtos = await breedsRepository.GetWithLimitAsync(limit, page, token);
            var breeds = breedDtos.Select(mapper.Map<CatBreed>).ToArray();

            await UpdateFromCash(breeds, token);

            return breeds;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message);
            return [];
        }
    }

    public async Task<IReadOnlyCollection<CatBreed>> SearchAsync(string name, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return [];
        }

        try
        {

            var breedDtos = await breedsRepository.SearchAsync(name, attachImage: true, token);
            var breeds = breedDtos.Select(mapper.Map<CatBreed>).ToArray();

            await UpdateFromCash(breeds, token);

            return breeds;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message);
            return [];
        }
    }

    public async Task<bool> InsertOrUpdateAsync(CatBreed catBreed, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(catBreed.Id))
        {
            return false;
        }

        try
        {
            var cachedCatBreedDto = mapper.Map<CachedCatBreedDto>(catBreed);
            return await cacheRepository.InsertOrUpdateAsync(cachedCatBreedDto, token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message);
            return false;
        }
    }

    private async Task UpdateFromCash(IEnumerable<CatBreed> breeds, CancellationToken token)
    {
        await Parallel.ForEachAsync(breeds, token, async (breed, cancellationToken) =>
        {
            if (string.IsNullOrWhiteSpace(breed.Id) == false)
            {
                if (cacheRepository.TryGetLocalValue(breed.Id, out var cached))
                {
                    breed.IsFavorite = cached!.IsFavorite;
                    breed.Description = cached.Description;
                }
            }

            if (string.IsNullOrWhiteSpace(breed.PhotoUrl) == false)
            {
                try
                {
                    breed.PhotoUrl = await imagesRepository.GetAsync(breed.PhotoUrl, token);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "{message}", ex.Message);
                }
            }
        });
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        try
        {
            await cacheRepository.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{message}", ex.Message);
        }
    }
}
