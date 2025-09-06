using CSharpFunctionalExtensions;

namespace backend.Data.Services.Interfaces;

public interface ICrud<in TKey, TEntity>
{
    Task<Result<TEntity>> Create(TEntity entity);
    Task<Maybe<TEntity>> Read(TKey entityId);
    Task<Result<TEntity>> Update(TEntity entity);
    Task<Result<TEntity>> Delete(TKey entityId);
}