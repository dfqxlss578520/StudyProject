using System.Collections.Generic;
using Hyl.Core.Caching;
using Hyl.Core.Configuration;
using Hyl.Core.Domain.MemberCenters;
using Hyl.Repository;

namespace Hyl.Service.MemberCenters
{
    public class UsersServices : IUsersServices
    {
        private readonly IRepository<Users> _repository;
        private readonly IDbQuery _dbQuery;
        private readonly ICacheManager _cacheManager;
        public UsersServices(IRepository<Users> repository, IDbQuery dbQuery, ICacheManager cacheManager)
        {
            _repository = repository;
            _dbQuery = dbQuery;
            _cacheManager = cacheManager;
        }

        #region Method
        public long Insert(Users sitemanager)
        {
            return _repository.Add(sitemanager);
        }
        public void InsertAddBatch(List<Users> sitemanager)
        {
            _repository.AddBatch(sitemanager);
        }

        public Users GetById(long id)
        {
            if (_cacheManager.IsSet(id.ToString()))
            {
                return _cacheManager.Get<Users>(id.ToString());
            }
            var model = _repository.Get(id);
            _cacheManager.Set(id.ToString(), model, BaseConfig.CacheWeek);
            return model;
        }

        public Users GetByUsername(string username)
        {
            return _dbQuery.QuerySingle<Users>("SELECT top 1 * FROM dbo.Users WHERE IsValid =1 AND UserName =@username", new { username = username });
        }

        public Users GetByUsernameCompany(string username, int companyid)
        {
            return _dbQuery.QuerySingle<Users>("SELECT top 1 * FROM dbo.Users WHERE IsValid =1 AND UserName =@username AND (Company =@companyid or Company = 0)",
                new { username = username, companyid = companyid });
        }

        //public List<Users> GetList()
        //{
        //    return _repository.GetAllList();
        //}

        //public List<Users> GetPagedList(Page<Users> page)
        //{
        //    return _repository.GetPagedList(page);
        //}

        //public Page<Users> GetPaged(Page<Users> page)
        //{
        //    return _repository.GetPaged(page);
        //}



        //public bool Delete(Users sitemanager)
        //{
        //    return _repository.Delete(sitemanager);
        //}

        //public bool Update(Users sitemanager)
        //{
        //    return _repository.Update(sitemanager);
        //} 
        #endregion

        #region Extendsion

        public Users UserLogin(Users users)
        {
            return _dbQuery.QuerySingle<Users>("SELECT * FROM dbo.Users WHERE IsValid =1 AND UserName =@username AND [Password] = @password;", new { username = users.UserName, password = users.Password });
        }
        #endregion
    }
}
