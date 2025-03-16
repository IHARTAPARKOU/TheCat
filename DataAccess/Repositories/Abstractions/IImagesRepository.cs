namespace DataAccess.Repositories.Abstractions;

public interface IImagesRepository
{
    Task<string> GetAsync(string url, CancellationToken token);
}