namespace DataAccess.Repositories.Abstractions;

public interface IFavoritesRepository
{
    Task<IEnumerable<string>> GetAllAsync(CancellationToken token);
    Task<bool> InsertAsync(string id, CancellationToken token);
    Task<bool> DeleteAsync(string id, CancellationToken token);
    Task SaveChangesAsync(CancellationToken token);
}