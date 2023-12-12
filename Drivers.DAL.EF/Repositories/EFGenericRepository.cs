using Drivers.DAL.EF.Data;
using Drivers.DAL.EF.Exceptions;
using Drivers.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Drivers.DAL.EF.Repositories;

public abstract class EFGenericRepository<TEntity> : IEFGenericRepository<TEntity> where TEntity : class
{
    // properties
    protected readonly DriversManagementContext databaseContext;
    protected readonly DbSet<TEntity> table;

    // constructor
    public EFGenericRepository(DriversManagementContext databaseContext)
    {
        this.databaseContext = databaseContext;
        table = this.databaseContext.Set<TEntity>();
    }

    /////////////////////////////////////////////////////////////////////
    ///   Generic CRUD
    /////////////////////////////////////////////////////////////////////

    /// <summary>
    /// GetByIdAsync
    /// </summary>
    /// <param name="id"></param>
    /// <returns>TEntity</returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public virtual async Task<TEntity> GetByIdAsync(int id)
    {
        return await table.FindAsync(id)
        ?? throw new EntityNotFoundException($"{typeof(TEntity).Name} with id {id} not found.");
    }

    /// <summary>
    /// GetAllAsync
    /// </summary>
    /// <returns>IEnumerable<TEntity></returns>
    /// <exception cref="Exception"></exception>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await table.ToListAsync()
        ?? throw new Exception($"Couldn't retrieve entities {typeof(TEntity).Name}");
    }

    /// <summary>
    /// AddAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(TEntity)} entity can't be null");
        }
        var added_entity = await table.AddAsync(entity);
        return added_entity.Entity;
    }

    /// <summary>
    /// UpdateAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(TEntity)} entity can't be null");
        }
        await Task.Run(() => table.Update(entity));
    }

    /// <summary>
    /// DeleteByIdAsync
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id) ?? throw new EntityNotFoundException($"{typeof(TEntity).Name} with id {id} not found. Cann't delete.");
        await Task.Run(() => table.Remove(entity));
    }

    /// <summary>
    /// DeleteAsync
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task DeleteAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
        }
        await Task.Run(() => table.Remove(entity));
    }

    /////////////////////////////////////////////////////////////////////
    ///   FILTERS
    /////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Пошук
    /// </summary>
    /// <returns></returns>
    public IQueryable<TEntity> FindAll()
    {
        return databaseContext.Set<TEntity>()
            .AsNoTracking();
    }

    /// <summary>
    /// метод специфікація
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public async Task<IQueryable<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> expression)
    {
        return await Task.Run(() => databaseContext.Set<TEntity>()
            .Where(expression)
            .AsNoTracking());
    }

    /////////////////////////////////////////////////////////////////////
    ///   Contracts
    /////////////////////////////////////////////////////////////////////

    /// <summary>
    /// GetCompleteEntityAsync
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public abstract Task<TEntity> GetCompleteEntityAsync(int id);
}
