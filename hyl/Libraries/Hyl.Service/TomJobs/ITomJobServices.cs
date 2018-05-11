using Hyl.Core;
using System.Collections.Generic;
using Hyl.Core.Domain.JobsDomain;
using Hyl.Core.Domain.PageDomain;

namespace Hyl.Service.SiteManagers
{
    public interface ITomJobServices : IServicesDependency
    {
        List<TomJob> GetPagedList();
        Page<TomJob> GetPaged();
        List<TomJob> GetList();

        TomJob GetById(int id);

        long Insert(TomJob sitemanager);

        bool Delete(TomJob sitemanager);

        bool Update(TomJob sitemanager);
    }
}
