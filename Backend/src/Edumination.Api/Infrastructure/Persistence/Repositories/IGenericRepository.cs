using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Edumination.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(long id);
    Task<IQueryable<T>> GetQueryable();
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<int> SaveChangesAsync();
    IQueryable<T> Find(Expression<Func<T, bool>> predicate);
}