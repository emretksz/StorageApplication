using Core.Entities.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework.Repository
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class, IEntity, IBaseEntity, new()
        where TContext : DbContext, new()
    {
       

        public async Task<List<TEntity>> GetAllAsync()
        {
            using (TContext _context = new TContext())
            {
                return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
            }
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext _context = new TContext())
            {
                return await _context.Set<TEntity>().Where(filter).AsNoTracking().ToListAsync();
            }
        }
        public async Task<TEntity> FindAsync(object id)
        {
            using (TContext _context = new TContext())
            {
                return await _context.Set<TEntity>().FindAsync(id);
            }
        }

        public async Task<TEntity> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, bool asNoTracking = false)
        {
            using (TContext _context = new TContext())
            {
                return !asNoTracking ? await _context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(filter) : await _context.Set<TEntity>().SingleOrDefaultAsync(filter);

            }
        }

        public IQueryable<TEntity> GetQuery()
        {
           
            TContext _context = new TContext();
                return _context.Set<TEntity>().AsQueryable();
        }

        public void Remove(TEntity entity)
        {
            using (TContext _context = new TContext())
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();
            }
        }

        public async Task CreateAsync(TEntity entity)
        {
            using (TContext _context = new TContext())
            {
                var added = _context.Entry(entity);
                added.State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<long> CreateAsyncReturnId(TEntity entity)
        {
            try
            {
                using (TContext _context = new TContext())
                {
                    var added = _context.Entry(entity);
                    added.State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    return entity.Id;
                }
            }
            catch (Exception ex )
            {

                throw;
            }

        }

        public async void Update(TEntity entity, TEntity unchanged)
        {
            using (TContext _context = new TContext())
            {
                _context.Entry(unchanged).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task  UpdateAll(TEntity entity)
        {
            using (TContext _context = new TContext())
            {
                try
                {

                    var update = _context.Entry(entity);
                    update.State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
 
        

        }

        public  IQueryable<TEntity> GetAll()
        {
            try
            {
                TContext _context = new TContext();
                return _context.Set<TEntity>().AsQueryable();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
