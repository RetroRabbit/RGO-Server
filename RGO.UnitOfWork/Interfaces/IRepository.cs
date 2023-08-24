using System.Linq.Expressions;

namespace RGO.UnitOfWork.Interfaces;

public interface IRepository<TK, T> where TK : IModel<T>
{
    Task<T?> GetById(int id);
    IQueryable<TK> Get(Expression<Func<TK, bool>> criteria = null);
    Task<List<T>> GetAll(Expression<Func<TK, bool>> criteria = null);
    Task<bool> Any(Expression<Func<TK, bool>> criteria);
    Task<T> Add(TK entity);
    Task<T> Delete(int id);
    Task<T> Update(TK entity);
    Task AddRange(List<TK> entities);
}