using DataAccess.Services.Abstractions;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Services;

public class ConfigurationService(IConfiguration configuration) : IConfigurationService
{
    private readonly IConfiguration configuration = configuration;

    public string TheCatApiKey => configuration["TheCatApi:ApiKey"]
        ?? throw new InvalidOperationException($"{nameof(TheCatApiKey)} is not found in the configuration.");

    public string TheCatApiBaseUri => configuration["TheCatApi:BaseUri"]
        ?? throw new InvalidOperationException($"{nameof(TheCatApiBaseUri)} is not found in the configuration.");
}
