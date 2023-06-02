using Core.Entities.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework.Repository
{
    public interface IRepository<T> where T : class, IEntity,IBaseEntity, new()
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter);

        Task<T> FindAsync(object id);

        Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false);

        IQueryable<T> GetQuery();
        IQueryable<T> GetAll();

        void Remove(T entity);

        Task CreateAsync(T entity);
        Task<long> CreateAsyncReturnId(T entity);

        void Update(T entity, T unchanged);
        Task UpdateAll(T entity);
  
    }
}
