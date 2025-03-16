using DataAccess.Entities.Abstractions;

namespace DataAccess.Repositories.Abstractions;

public interface IBaseApiRepository<T> where T : IBaseEntity
{
    Task<T?> GetByIdAsync(string id, CancellationToken token);
}
