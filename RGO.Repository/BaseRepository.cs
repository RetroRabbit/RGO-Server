using Microsoft.EntityFrameworkCore;
using RGO.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace RGO.Repository;

public class BaseRepository<TK> : IRepository<TK> where TK : class
{
    private readonly DbContext _db;
    private readonly DbSet<TK> _entity;

    public BaseRepository(DbContext db)
    {
        _db = db;
        _entity = db.Set<TK>();
    }

    public async Task<TK?> GetById(int id)
    {
        return await _entity.FindAsync(id);
    }

    public IQueryable<TK> Get(Expression<Func<TK, bool>> criteria = null)
    {
        return criteria == null
            ? _entity.AsQueryable()
            : _entity.Where(criteria);
    }

    public async Task<bool> Any(Expression<Func<TK, bool>> criteria)
    {
        return await _entity.AnyAsync(criteria);
    }

    public async Task Add(TK entity)
    {
        await _entity.AddAsync(entity);
    }

    public async Task Delete(int id)
    {
        var obj = await GetById(id);
        if (obj == null) return;
        _entity.Remove(obj);
    }

    public async Task Update(int id, TK entity)
    {
        this._db
            .Entry<TK>(await GetById(id) ?? throw new Exception($"Unable to find object of type {typeof(TK)} with id {id}"))
            .CurrentValues
            .SetValues(entity);
        await this.Save();
    }

    public async Task AddRange(List<TK> entities)
    {
        await _entity.AddRangeAsync(entities);
    }

    public async Task Save()
    {
        await this._db.SaveChangesAsync();
    }
}