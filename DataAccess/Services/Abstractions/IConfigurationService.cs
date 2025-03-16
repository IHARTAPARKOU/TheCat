namespace DataAccess.Services.Abstractions;

public interface IConfigurationService
{
    string TheCatApiKey { get; }
    string TheCatApiBaseUri { get; }
    string TheCatImagesUri { get; }
    string TheCatImagesCache { get; }
    string TheCatFavorites { get; }
}
