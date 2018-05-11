using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using Hyl.Core.Helpers.Utility;
using Hyl.Core.Domain.PageDomain;
using System.Threading.Tasks;

namespace Hyl.Repository.Nhibernate
{
    public class NhibernateRepository<T> : IRepository<T> where T : class
    {
        private string HibernateXmlFileName = "hibernate.cfg.xml";
        private ISessionFactory sessionFactory;
        public NhibernateRepository()
        {
            sessionFactory = new Configuration().Configure().BuildSessionFactory();
        }

        public ISession GetSession()
        {
            return sessionFactory.OpenSession();
        }
        public IStatelessSession GetStatelessSession()
        {
            return sessionFactory.OpenStatelessSession();
        }


        public long Add(T entity)
        {
            using (var session = GetStatelessSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var newId = session.Insert(entity);
                    transaction.Commit();
                    return Utils.StrToLong(newId);
                }
            }
        }

        public long AddBatch(IEnumerable<T> entitys)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (T item in entitys)
                    {
                        session.Insert(item);
                    }
                    transaction.Commit();
                    return 1;
                }
            }
        }

        public bool Delete(T entity)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                    return true;
                }
            }
        }

        public bool Delete(string Id)
        {
            var model = Get(Id);
            if (model != null)
            {
                return Delete(model);
            }
            return false;
        }

        public bool Delete(int Id)
        {
            var model = Get(Id);
            if (model != null)
            {
                return Delete(model);
            }
            return false;
        }

        public bool Delete(Guid Id)
        {
            var model = Get(Id);
            if (model != null)
            {
                return Delete(model);
            }
            return false;
        }

        public T Get(string Id)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var model = session.Get<T>(Id);
                    transaction.Commit();
                    return model;
                }
            }
        }

        public T Get(long Id)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var model = session.Get<T>(Id);
                    transaction.Commit();
                    return model;
                }
            }
        }

        public T Get(Guid Id)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var model = session.Get<T>(Id);
                    transaction.Commit();
                    return model;
                }
            }
        }

        public T Get(int Id)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var model = session.Get<T>(Id);
                    transaction.Commit();
                    return model;
                }
            }
        }

        public T Get(T entity)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var model = session.Get<T>(entity);
                    transaction.Commit();
                    return model;
                }
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var model = (IEnumerable<T>)session.CreateCriteria<T>().List();
                    transaction.Commit();
                    return model;
                }
            }
        }

        public List<T> GetAllList()
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var model = (List<T>)session.CreateCriteria<T>().List();
                    transaction.Commit();
                    return model;
                }
            }
        }

        public Core.Domain.PageDomain.Page<T> GetPaged(Core.Domain.PageDomain.Page<T> pageinfo)
        {
            //需要写拓展方法，暂未实现
            return null;
        }

        public List<T> GetPagedList(Core.Domain.PageDomain.Page<T> pageinfo)
        {
            //需要写拓展方法，暂未实现
            return null;
        }

        public bool Update(T entity)
        {
            using (var session = GetStatelessSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(entity);
                    transaction.Commit();
                    return true;
                }
            }
        }

        public Task<long> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<long> AddBatchAsync(IEnumerable<T> entitys)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(long Id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetPagedListAsync(Page<T> pageinfo)
        {
            throw new NotImplementedException();
        }

        public Task<Page<T>> GetPagedAsync(Page<T> pageinfo)
        {
            throw new NotImplementedException();
        }
    }
}
