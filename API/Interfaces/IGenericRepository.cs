using System;
using System.Linq.Expressions;

namespace API.Interfaces;

public interface IGenericRepository<T, M> where T : class where M : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T?> GetOne(int id);
    void Update(T entity);
    Task Create(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    Task<T?> FindOne(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAndPopulateAsync(Expression<Func<T, object>> includeExpression);
    Task<T?> FindOneAndPopulateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> includeExpression);
    Task<M?> FindOneMapped(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<M>> GetAllMapped();
}
