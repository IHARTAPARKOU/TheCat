﻿using System.Net.Http.Headers;
using System.Text.Json;
using DataAccess.Entities.Abstractions;
using DataAccess.Services.Abstractions;

namespace DataAccess.Repositories.Abstractions;

public abstract class BaseApiRepository<T>(HttpClient httpClient, IConfigurationService configurationService)
    : BaseRepository<T>, IBaseApiRepository<T> where T : IBaseEntity, new()
{
    protected string ApiKey => configurationService.TheCatApiKey;
    protected abstract string BaseUri { get; }

    public virtual async Task<T?> GetByIdAsync(string id, CancellationToken token)
    {
        if (Cache.TryGetValue(id, out var item) == false)
        {
            item = await RequestSingleAsync($"/{id}", token);
        }

        return item;
    }

    protected async Task<IEnumerable<T>> RequestCollectionAsync(string requestUri, CancellationToken token)
    {
        var response = await RequestInternalAsync(requestUri, token);
        var result = JsonSerializer.Deserialize<IEnumerable<T>>(response) ?? [];
        AddToCache(result);
        return result;
    }

    protected async Task<T?> RequestSingleAsync(string requestUri, CancellationToken token)
    {
        var response = await RequestInternalAsync(requestUri, token);
        var result = JsonSerializer.Deserialize<T>(response);
        AddToCache(result);
        return result;
    }

    private async Task<string> RequestInternalAsync(string requestUri, CancellationToken token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, BaseUri + requestUri);
        request.Headers.Add("x-api-key", ApiKey);
        request.Content = new StringContent(string.Empty, new MediaTypeHeaderValue("application/json"));

        var response = await httpClient.SendAsync(request, token);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(token);
    }
}
