using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Consts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Base
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        // tracking
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        // get
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(int skip, int take);

        Task<T?> GetByIdAsync(int id);
        //find
        Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[]? includes);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[]? includes, int take, int skip);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[]? includes, int? take, int? skip,
            Expression<Func<T, object>>? orderBy, string orderByDirection = OrderBy.Ascending);
        //add
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        // update
        T Update(T entity);
        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
        // delete
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        // count
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
        // save changes
        Task SaveChangesAsync();

        // transactions
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollBackAsync();

    }
}
