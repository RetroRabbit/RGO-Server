using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Repositories;

public interface IRepository<T> where T : IModel
{
    Task<T?> GetById(int id);
    IQueryable<T> Get(Expression<Func<T, bool>>? criteria = null);
    Task<List<T>> GetAll(Expression<Func<T, bool>>? criteria = null);
    Task<T?> FirstOrDefault(Expression<Func<T, bool>> criteria);
    Task<bool> Any(Expression<Func<T, bool>> criteria);
    Task<T> Add(T entity);
    Task<T> Delete(int id);
    Task<T> Update(T entity);
    Task AddRange(List<T> entities);
}

public class BaseRepository<T> : IRepository<T> where T : class, IModel
{
    private readonly DatabaseContext _db;
    private readonly DbSet<T> _entity;

    public BaseRepository(DatabaseContext db)
    {
        _db = db;
        _entity = db.Set<T>();
    }

    public async Task<T?> GetById(int id)
    {
        return (await _entity.FindAsync(id));
    }

    public IQueryable<T> Get(Expression<Func<T, bool>>? criteria = null)
    {
        return criteria == null
            ? _entity.AsQueryable()
            : _entity.Where(criteria);
    }

    public async Task<List<T>> GetAll(Expression<Func<T, bool>>? criteria = null)
    {
        return (criteria == null
                   ? await _entity.ToListAsync()
                   : await _entity.Where(criteria).ToListAsync())
               .Select(x => x)
               .ToList();
    }

    public async Task<T?> FirstOrDefault(Expression<Func<T, bool>> criteria)
    {
        var value = (criteria == null
            ? await _entity.FirstOrDefaultAsync()
            : await _entity.Where(criteria).FirstOrDefaultAsync());

        return value;
    }

    public async Task<bool> Any(Expression<Func<T, bool>> criteria)
    {
        return await _entity.AnyAsync(criteria);
    }

    public async Task<T> Add(T entity)
    {
        var obj = await _entity.AddAsync(entity);
        await _db.SaveChangesAsync();
        return obj.Entity;
    }

    public async Task<T> Delete(int id)
    {
        var obj = await _entity.FindAsync(id);
        if (obj == null) throw new KeyNotFoundException($"Unable to find object of type {typeof(T)} with id {id}");
        _entity.Remove(obj);
        await _db.SaveChangesAsync();
        return obj;
    }

    public async Task<T> Update(T entity)
    {
        var obj = _db
            .Entry(await _entity.FindAsync(entity.Id) ??
                   throw new Exception($"Unable to find object of type {typeof(T)} with id {entity.Id}"));

        obj.CurrentValues.SetValues(entity);
        await _db.SaveChangesAsync();
        return obj.Entity;
    }

    public async Task AddRange(List<T> entities)
    {
        await _entity.AddRangeAsync(entities);
        await _db.SaveChangesAsync();
    }
}