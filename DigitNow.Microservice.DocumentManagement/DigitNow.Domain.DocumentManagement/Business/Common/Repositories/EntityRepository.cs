using DigitNow.Domain.DocumentManagement.Data;
using HTSS.Platform.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Business.Common.Repositories
{
    public class EntityRepository<T> : IEntityRepository<T> where T : Entity
    {
        private DbSet<T> _dbSet;
        private DbContext _dbContext;

        public EntityRepository(DocumentManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public void Insert(T entity)
        {
            _dbContext.Add(entity);
        }

        public void Insert(IEnumerable<T> entities)
        {
            _dbContext.AddRange(entities);
        }

        public async Task InsertAsync(T entity, CancellationToken token)
        {
            await _dbContext.AddAsync(entity, token);
        }

        public Task InsertAsync(IEnumerable<T> entities, CancellationToken token)
        {
            return _dbContext.AddRangeAsync(entities, token);
        }

        public void Update(T entity)
        {
            _dbContext.Update(entity);
        }

        public void Update(IEnumerable<T> entities)
        {
            _dbContext.UpdateRange(entities);
        }

        public Task UpdateAsync(T entity, CancellationToken token)
        {
            if (token.IsCancellationRequested) return Task.CompletedTask;
            _dbContext.Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(IEnumerable<T> entities, CancellationToken token)
        {
            if (token.IsCancellationRequested) return Task.CompletedTask;
            _dbContext.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public void Delete(T entity)
        {
            _dbContext.Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbContext.RemoveRange(entities);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            foreach (var entity in _dbSet.AsNoTracking().Where(predicate).ToList())
            {
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
        }

        public Task DeleteAsync(T entity, CancellationToken token)
        {
            if (token.IsCancellationRequested) return Task.CompletedTask;
            Delete(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(IEnumerable<T> entities, CancellationToken token)
        {
            if (token.IsCancellationRequested) return Task.CompletedTask;
            Delete(entities);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            var entities = await _dbSet.AsNoTracking().Where(predicate).ToListAsync(token);
            foreach (var entity in entities)
            {
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
        }

        public T Get(long id)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(predicate);
        }

        public Task<T> GetAsync(long id, CancellationToken token)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            return _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, token);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public Task<List<T>> GetAllAsync(CancellationToken token)
        {
            return _dbSet.AsNoTracking().ToListAsync(token);
        }

        public List<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToListAsync(token);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Count(predicate) > 0;
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            var count = await _dbSet.AsNoTracking().CountAsync(predicate, token);
            return count > 0;
        }

        public int Count()
        {
            return _dbSet.AsNoTracking().Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Count(predicate);
        }

        public Task<int> CountAsync(CancellationToken token)
        {
            return _dbSet.AsNoTracking().CountAsync(token);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            return _dbSet.AsNoTracking().CountAsync(predicate, token);
        }

        public T Max(Expression<Func<T, int>> predicate)
        {
            return _dbSet.AsNoTracking().OrderByDescending(predicate).FirstOrDefault();
        }

        public Task<T> MaxAsync(Expression<Func<T, int>> predicate, CancellationToken token)
        {
            return _dbSet.AsNoTracking().OrderByDescending(predicate).FirstOrDefaultAsync(token);
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public Task CommitAsync(CancellationToken token)
        {
            return _dbContext.SaveChangesAsync(token);
        }
    }
}
