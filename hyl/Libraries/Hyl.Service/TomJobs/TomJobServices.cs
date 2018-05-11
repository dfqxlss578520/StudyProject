using Hyl.Repository;
using System;
using System.Collections.Generic;
using Hyl.Core.Domain.JobsDomain;
using Hyl.Core.Domain.PageDomain;

namespace Hyl.Service.SiteManagers
{
    public class TomJobServices : ITomJobServices
    {
        private readonly IRepository<TomJob> _repository;
        public TomJobServices(IRepository<TomJob> repository)
        {
            _repository = repository;
        }




        public long Insert(TomJob sitemanager)
        {
            return _repository.Add(sitemanager);
        }

        public bool Update(TomJob sitemanager)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TomJob sitemanager)
        {
            return _repository.Delete(sitemanager);
        }




        public TomJob GetById(int id)
        {
            return _repository.Get(id);
        }

        public List<TomJob> GetPagedList()
        {
            return _repository.GetPagedList(new Page<TomJob>() {PageIndex = 2, PageSize = 3});
        }
        public Page<TomJob> GetPaged()
        {
            return _repository.GetPaged(new Page<TomJob>() { PageIndex = 2, PageSize = 3 });
        }

        public List<TomJob> GetList()
        {
            return _repository.GetAllList();
        }

    }
}
