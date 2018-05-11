using System;
using System.Collections.Generic;
using Hyl.Core.Domain.PageDomain;
using System.Threading.Tasks;

namespace Hyl.Repository
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : class
    {
        long Add(T entity);
        long AddBatch(IEnumerable<T> entitys);
        bool Update(T entity);
        bool Delete(T entity);
        bool Delete(string Id);
        bool Delete(int Id);
        bool Delete(Guid Id);
        T Get(string Id);
        T Get(long Id);
        T Get(Guid Id);
        T Get(int Id);
        T Get(T entity);
        IEnumerable<T> GetAll();
        List<T> GetAllList();
        
        List<T> GetPagedList(Page<T> pageinfo);

        Page<T> GetPaged(Page<T> pageinfo);



        Task<long> AddAsync(T entity);
        Task<long> AddBatchAsync(IEnumerable<T> entitys);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteAsync(string Id);
        Task<bool> DeleteAsync(int Id);
        Task<bool> DeleteAsync(Guid Id);
        Task<T> GetAsync(string Id);
        Task<T> GetAsync(long Id);
        Task<T> GetAsync(Guid Id);
        Task<T> GetAsync(int Id);
        Task<T> GetAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<List<T>> GetAllListAsync();

        Task<List<T>> GetPagedListAsync(Page<T> pageinfo);

        Task<Page<T>> GetPagedAsync(Page<T> pageinfo);
    }
}
