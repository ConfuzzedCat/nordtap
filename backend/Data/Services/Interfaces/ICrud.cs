namespace backend.Data.Services.Interfaces;

public interface ICrud<TKey, TEntity>
{
    Task<TEntity> Create(TEntity entity);
    Task<TEntity?> Read(TKey entityId);
    Task<TEntity> Update(TEntity entity);
    Task<TEntity> Delete(TKey entityId);
}