using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BW.DAL;

namespace BW.BLL.Repository
{
    public abstract class RepositoryBase<T, TId> : IDisposable where T : class
    {
        protected static MyContext dbContext;
        public virtual List<T> GetAll()
        {
            try
            {
                dbContext = new MyContext();
                return dbContext.Set<T>().ToList();
            }
            catch
            {
                throw;
            }
        }
        public virtual async Task<List<T>> GetAllAsync()
        {
            try
            {
                dbContext = new MyContext();
                return await dbContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual T GetById(TId id)
        {
            try
            {
                dbContext = new MyContext();
                return dbContext.Set<T>().Find(id);
            }
            catch
            {
                throw;
            }
        }
        public virtual async Task<T> GetByIdAsync(TId id)
        {
            try
            {
                dbContext = new MyContext();
                return await dbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual int Insert(T entity)
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                dbContext.Set<T>().Add(entity);
                return dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public virtual async Task<int> InsertAsync(T entity)
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                dbContext.Set<T>().Add(entity);
                return await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual int Delete(T entity)
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                dbContext.Set<T>().Remove(entity);
                return dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public virtual async Task<int> DeleteAsync(T entity)
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                dbContext.Set<T>().Remove(entity);
                return await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual int Update()
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                return dbContext.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
        public virtual async Task<int> UpdateAsync()
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                return await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual IQueryable<T> Queryable()
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                return dbContext.Set<T>().AsQueryable();
            }
            catch
            {
                throw;
            }
        }
        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
            dbContext = null;
        }
        public virtual ParallelQuery<T> Parallel()
        {
            try
            {
                dbContext = dbContext ?? new MyContext();
                return dbContext.Set<T>().AsParallel();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
