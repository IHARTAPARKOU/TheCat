using DataAccess.Repositories.Abstractions;

namespace DataAccess.Repositories;

public class ImagesRepository(HttpClient httpClient, string directoryPath) : IImagesRepository
{
    public async Task<string> GetAsync(string url, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentNullException(nameof(url));
        }

        var fileName = Path.GetFileName(url);
        var filePath = Path.Combine(directoryPath, fileName);
        if (File.Exists(filePath))
        {
            return filePath;
        }

        if (Directory.Exists(directoryPath) == false)
        {
            Directory.CreateDirectory(directoryPath);
        }

        var imageBytes = await httpClient.GetByteArrayAsync(url, token);
        await File.WriteAllBytesAsync(filePath, imageBytes, token);

        return filePath;
    }
}
