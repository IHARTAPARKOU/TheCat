namespace DataAccess.Services.Abstractions;

public interface IConfigurationService
{
    string TheCatApiKey { get; }
    string TheCatApiBaseUri { get; }
}
