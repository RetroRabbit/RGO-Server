using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class BaseRepository<TK, T> : IRepository<TK, T> where TK : class, IModel<T>
{
    private readonly DbContext _db;
    private readonly DbSet<TK> _entity;

    public BaseRepository(DbContext db)
    {
        _db = db;
        _entity = db.Set<TK>();
    }

    public async Task<T?> GetById(int id)
    {
        return (await _entity.FindAsync(id)).ToDto();
    }

    public IQueryable<TK> Get(Expression<Func<TK, bool>> criteria = null)
    {
        return criteria == null
            ? _entity.AsQueryable()
            : _entity.Where(criteria);
    }

    public async Task<List<T>> GetAll(Expression<Func<TK, bool>> criteria = null)
    {
        return ( criteria == null
            ? await _entity.ToListAsync()
            : await _entity.Where(criteria).ToListAsync())
            .Select(x => x.ToDto())
            .ToList();
    }

    public async Task<bool> Any(Expression<Func<TK, bool>> criteria)
    {
        return await _entity.AnyAsync(criteria);
    }

    public async Task<T> Add(TK entity)
    {
        var obj = await _entity.AddAsync(entity);
        await this._db.SaveChangesAsync();
        return obj.Entity.ToDto();
    }

    public async Task<T> Delete(int id)
    {
        var obj = await _entity.FindAsync(id);
        if(obj == null) throw new KeyNotFoundException($"Unable to find object of type {typeof(TK)} with id {id}");
        _entity.Remove(obj);
        await this._db.SaveChangesAsync();
        return obj.ToDto();
    }

    public async Task<T> Update(TK entity)
    {
        var obj = this._db
            .Entry<TK>(await _entity.FindAsync(entity.Id) ?? throw new Exception($"Unable to find object of type {typeof(TK)} with id {entity.Id}"));

            obj.CurrentValues.SetValues(entity);
            await this._db.SaveChangesAsync();
            return obj.Entity.ToDto();
    }

    public async Task AddRange(List<TK> entities)
    {
        await _entity.AddRangeAsync(entities);
    }
}