using System.Linq.Expressions;
using HRIS.Models.Enums;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Repositories;

public class BaseRepository<TK, T> : IRepository<TK, T> where TK : class, IModel<T>
{
    protected readonly DatabaseContext _db;
    protected readonly DbSet<TK> _entity;

    public BaseRepository(DatabaseContext db)
    {
        _db = db;
        _entity = db.Set<TK>();
    }

    public async Task<T?> GetById(int id)
    {
        return (await _entity.FindAsync(id))!.ToDto();
    }

    public IQueryable<TK> Get(Expression<Func<TK, bool>>? criteria = null)
    {
        return criteria == null
            ? _entity.AsQueryable()
            : _entity.Where(criteria);
    }

    public async Task<List<T>> GetAll(Expression<Func<TK, bool>>? criteria = null)
    {
        return (criteria == null
                   ? await _entity.ToListAsync()
                   : await _entity.Where(criteria).ToListAsync())
               .Select(x => x.ToDto())
               .ToList();
    }

    public async Task<T> FirstOrDefault(Expression<Func<TK, bool>> criteria)
    {
        return (criteria == null
                ? await _entity.FirstOrDefaultAsync()
                : await _entity.Where(criteria).FirstOrDefaultAsync())!
            .ToDto();
    }

    public async Task<bool> Any(Expression<Func<TK, bool>> criteria)
    {
        return await _entity.AnyAsync(criteria);
    }


    public async Task<T> Add(TK entity)
    {
        var obj = await _entity.AddAsync(entity);
        await AddAuditLog(entity, CRUDOperations.Create);
        await _db.SaveChangesAsync();
        return obj.Entity.ToDto();
    }

    public async Task<T> Delete(int id)
    {
        var obj = await _entity.FindAsync(id);
        if (obj == null) throw new KeyNotFoundException($"Unable to find object of type {typeof(TK)} with id {id}");
        _entity.Remove(obj);
        await AddAuditLog(obj, CRUDOperations.Delete);
        await _db.SaveChangesAsync();
        return obj.ToDto();
    }

    public async Task<T> Update(TK entity)
    {
        var obj = _db
            .Entry(await _entity.FindAsync(entity.Id) ??
                   throw new Exception($"Unable to find object of type {typeof(TK)} with id {entity.Id}"));

        obj.CurrentValues.SetValues(entity);
        await AddAuditLog(entity, CRUDOperations.Update);
        await _db.SaveChangesAsync();
        return obj.Entity.ToDto();
    }

    public async Task AddRange(List<TK> entities)
    {
        await _entity.AddRangeAsync(entities);
        await _db.SaveChangesAsync();
    }

    private async Task AddAuditLog(TK entity, CRUDOperations operation)
    {
        if (!GlobalVariables.AreTestsRunning())
        {
            var log = new AuditLog
            {
                Date = DateTime.Now,
                CRUDOperation = operation,
                CreatedById = GlobalVariables.GetUserId(),
                Table = entity.GetType().Name,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(entity)
            };
        }
    }
}