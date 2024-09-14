using System.Linq.Expressions;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class GenericRepository<T, M>(DataContext context, IMapper mapper) : IGenericRepository<T, M> where T : class where M : class
{
    public async Task Create(T entity)
    => await context.Set<T>().AddAsync(entity);

    public void Delete(T entity) => context.Set<T>().Remove(entity);

    public async Task<IEnumerable<T>> GetAll() => await context.Set<T>().ToListAsync();

    public async Task<T?> GetOne(int id) => await context.Set<T>().FindAsync(id);

    public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();

    public void Update(T entity)
    => context.Set<T>().Update(entity);

    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate) =>
    await context.Set<T>().Where(predicate).ToListAsync();

    public async Task<T?> FindOne(Expression<Func<T, bool>> predicate)
    => await context.Set<T>().FirstOrDefaultAsync(predicate);

    public async Task<IEnumerable<T>> GetAllAndPopulateAsync(Expression<Func<T, object>> includeExpression) =>
    await context.Set<T>().Include(includeExpression).ToListAsync();

    public async Task<T?> FindOneAndPopulateAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> includeExpression)
    => await context.Set<T>().Include(includeExpression).FirstOrDefaultAsync(predicate);


    public async Task<M?> FindOneMapped(Expression<Func<T, bool>> predicate)
    => await context.Set<T>().
        Where(predicate).
        ProjectTo<M>(mapper.ConfigurationProvider).
        SingleOrDefaultAsync();
    public async Task<IEnumerable<M>> GetAllMapped()
    => await context.Set<T>().
    ProjectTo<M>(mapper.ConfigurationProvider).
    ToListAsync();


}
