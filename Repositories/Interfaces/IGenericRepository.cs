
using System.Linq.Expressions;

namespace EmployeeInfo.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetByIdAsync(int id);
    }
}
