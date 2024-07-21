using DLL.Entities;

namespace DLL.Interface;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    Task DeleteByIdAsync(Guid id);
    void Update(TEntity entity);
}