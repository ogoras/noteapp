using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Infrastructure.Repositories
{
    public abstract class Repository<T> where T : class, IUpdateable<T>
    {
        protected AppDbContext _appDbContext;
        protected DbSet<T> dbSet;
        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task CreateAsync(T t)
        {
            try
            {
                dbSet.Add(t);
                _appDbContext.SaveChanges();
                await Task.CompletedTask;
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }
        public async Task DelAsync(T t, Func<T, bool> lambda)
        {
            try
            {
                _appDbContext.Remove(dbSet.FirstOrDefault(lambda));
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }
        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await Task.FromResult(dbSet);
        }
        public async Task<IEnumerable<T>> ReadAllAsync(Func<T, bool> lambda)
        {
            return await Task.FromResult(dbSet.Where(lambda));
        }
        public async Task<IEnumerable<T>> ReadAllAsync(IIncludableQueryable<T, object?> detailedSet, Func<T, bool> lambda)
        {
            return await Task.FromResult(detailedSet.Where(lambda));
        }
        public async Task<T> ReadAsync(Func<T, bool> lambda)
        {
            return await Task.FromResult(dbSet.FirstOrDefault(lambda));
        }
        public async Task<T> ReadAsync(IIncludableQueryable<T, object?> detailedSet, Func<T, bool> lambda)
        {
            return await Task.FromResult(detailedSet.FirstOrDefault(lambda));
        }
        public async Task UpdateAsync(T t, Func<T, bool> lambda)
        {
            try
            {
                T original = dbSet.FirstOrDefault(lambda);
                original.updateValues(t);
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                await Task.FromException(e);
            }
        }
    }
}
