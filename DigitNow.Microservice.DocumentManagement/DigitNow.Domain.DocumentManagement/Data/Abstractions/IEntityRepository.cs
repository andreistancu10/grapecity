using HTSS.Platform.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DigitNow.Domain.DocumentManagement.Data.Abstractions
{
    public interface IEntityRepository<T> where T : Entity
    {
        void Insert(T entity);
        void Insert(IEnumerable<T> entities);

        Task InsertAsync(T entity, CancellationToken token);
        Task InsertAsync(IEnumerable<T> entities, CancellationToken token);

        void Update(T entity);
        void Update(IEnumerable<T> entities);

        Task UpdateAsync(T entity, CancellationToken token);
        Task UpdateAsync(IEnumerable<T> entities, CancellationToken token);

        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
        void Delete(Expression<Func<T, bool>> predicate);

        Task DeleteAsync(T entity, CancellationToken token);
        Task DeleteAsync(IEnumerable<T> entities, CancellationToken token);
        Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken token);

        T Get(long id, params Expression<Func<T, object>>[] includes);
        T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task<T> GetAsync(long id, CancellationToken token, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken token, params Expression<Func<T, object>>[] includes);

        List<T> GetAll(params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(CancellationToken token, params Expression<Func<T, object>>[] includes);

        List<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate, CancellationToken token, params Expression<Func<T, object>>[] includes);

        bool Exists(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken token);

        int Count();
        int Count(Expression<Func<T, bool>> predicate);

        Task<int> CountAsync(CancellationToken token);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken token);

        T Max(Expression<Func<T, int>> predicate);
        Task<T> MaxAsync(Expression<Func<T, int>> predicate, CancellationToken token);

        void Commit();
        Task CommitAsync(CancellationToken token);
    }
}
