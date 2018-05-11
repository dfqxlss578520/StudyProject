using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Hyl.Core.Domain.PageDomain;
using Hyl.Repository.Dapper;
using System.Threading.Tasks;

namespace Hyl.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected IDbConnection Conn { get; private set; }
        public Repository(IDbConnection dbconn)
        {
            Conn = dbconn;
        }

        public void SetDbConnection(IDbConnection conn)
        {
            Conn = conn;
        }

        #region Sync
        public long Add(T entity)
        {
            return Conn.Insert<T>(entity);
        }

        public long AddBatch(IEnumerable<T> entitys)
        {
            return Conn.Insert(entitys);
        }

        public bool Update(T entity)
        {
            return Conn.Update(entity);
        }

        public bool Delete(T entity)
        {
            return Conn.Delete(entity);
        }

        public bool Delete(string Id)
        {
            var entity = Get(Id);
            if (entity == null) return false;

            return Delete(entity);
        }

        public bool Delete(int Id)
        {
            var entity = Get(Id);
            if (entity == null) return false;

            return Delete(entity);
        }
        public bool Delete(Guid Id)
        {
            var entity = Get(Id);
            if (entity == null) return false;

            return Delete(entity);
        }

        public T Get(T entity)
        {
            return Conn.Get<T>(entity);
        }

        public T Get(Guid Id)
        {
            return Conn.Get<T>(Id);
        }

        public T Get(string Id)
        {
            return Conn.Get<T>(Id);
        }

        public T Get(int Id)
        {
            return Conn.Get<T>(Id);
        }

        public T Get(long Id)
        {
            return Conn.Get<T>(Id);
        }

        public IEnumerable<T> GetAll()
        {
            return Conn.GetAll<T>();
        }
        public List<T> GetAllList()
        {
            return GetAll().ToList();
        }

        public List<T> GetPagedList(Page<T> pageinfo)
        {
            return Conn.GetPagedList<T>(pageinfo.PageIndex, pageinfo.PageSize, pageinfo.Fields, pageinfo.Conditions, pageinfo.OrderBy, pageinfo.Parameters).ToList();
        }

        public Page<T> GetPaged(Page<T> pageinfo)
        {
            var tuple = Conn.GetPaged<T>(pageinfo.PageIndex, pageinfo.PageSize, pageinfo.Fields, pageinfo.Conditions, pageinfo.OrderBy, pageinfo.Parameters);

            pageinfo.Items = tuple.Item2.ToList();

            pageinfo.TotalItems = tuple.Item1;
            pageinfo.TotalPages = tuple.Item1 / pageinfo.PageSize + (tuple.Item1 % pageinfo.PageSize > 0 ? 1 : 0);

            return pageinfo;
        } 
        #endregion


        #region Async
        public async Task<long> AddAsync(T entity)
        {
            return await Conn.InsertAsync<T>(entity);
        }
        public async Task<long> AddBatchAsync(IEnumerable<T> entitys)
        {
            return await Conn.InsertAsync(entitys);
        }
        public async Task<bool> UpdateAsync(T entity)
        {
            return await Conn.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            return await Conn.DeleteAsync(entity);
        }

        public async Task<bool> DeleteAsync(string Id)
        {
            var entity = await GetAsync(Id);
            if (entity == null) return false;

            return await DeleteAsync(entity);
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var entity = await GetAsync(Id);
            if (entity == null) return false;

            return await DeleteAsync(entity);
        }
        public async Task<bool> DeleteAsync(Guid Id)
        {
            var entity = await GetAsync(Id);
            if (entity == null) return false;

            return await DeleteAsync(entity);
        }

        public async Task<T> GetAsync(T entity)
        {
            return await Conn.GetAsync<T>(entity);
        }

        public async Task<T> GetAsync(Guid Id)
        {
            return await Conn.GetAsync<T>(Id);
        }

        public async Task<T> GetAsync(string Id)
        {
            return await Conn.GetAsync<T>(Id);
        }

        public async Task<T> GetAsync(int Id)
        {
            return await Conn.GetAsync<T>(Id);
        }

        public async Task<T> GetAsync(long Id)
        {
            return await Conn.GetAsync<T>(Id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Conn.GetAllAsync<T>();
        }
        public async Task<List<T>> GetAllListAsync()
        {
            return (await GetAllAsync()).ToList();
        }

        public async Task<List<T>> GetPagedListAsync(Page<T> pageinfo)
        {
            return (await Conn.GetPagedListAsync<T>(pageinfo.PageIndex, pageinfo.PageSize, pageinfo.Fields, pageinfo.Conditions, pageinfo.OrderBy, pageinfo.Parameters)).ToList();
        }

        public async Task<Page<T>> GetPagedAsync(Page<T> pageinfo)
        {
            var tuple = await Conn.GetPagedAsync<T>(pageinfo.PageIndex, pageinfo.PageSize, pageinfo.Fields, pageinfo.Conditions, pageinfo.OrderBy, pageinfo.Parameters);
            pageinfo.Items = tuple.Item2.ToList();

            pageinfo.TotalItems = tuple.Item1;
            pageinfo.TotalPages = tuple.Item1 / pageinfo.PageSize + (tuple.Item1 % pageinfo.PageSize > 0 ? 1 : 0);

            return pageinfo;
        } 
        #endregion
    }
}
