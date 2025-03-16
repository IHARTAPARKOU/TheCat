using System.Runtime.CompilerServices;
using DataAccess.Services.Abstractions;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Services;

public class ConfigurationService(IConfiguration configuration) : IConfigurationService
{
    private readonly IConfiguration configuration = configuration;

    public string TheCatApiKey => configuration["TheCatApi:ApiKey"] ?? Throw();

    public string TheCatApiBaseUri => configuration["TheCatApi:BaseUri"] ?? Throw();

    public string TheCatImagesUri => configuration["TheCatResources:Images"] ?? Throw();

    public string TheCatImagesCache
    {
        get
        {
            var path = configuration["TheCatResources:ImagesCache"] ?? Throw();
            return Format(path);
        }
    }

    public string TheCatFavorites
    {
        get
        {
            var path = configuration["TheCatResources:Favorites"] ?? Throw();

            return Format(path);
        }
    }

    private static string Format(string path)
    {
        if (path.StartsWith("{0}"))
        {
            var applicationPath = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            path = string.Format(path, applicationPath);
        }

        return path;
    }

    private static string Throw([CallerMemberName] string? name = null)
    {
        throw new ArgumentNullException($"{name} is not found in the configuration.");
    }
}
