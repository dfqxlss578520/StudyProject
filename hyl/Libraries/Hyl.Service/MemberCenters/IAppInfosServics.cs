using Hyl.Core;
using Hyl.Core.Domain.MemberCenters;

namespace Hyl.Service.MemberCenters
{
    public interface IAppInfosServics : IServicesDependency
    {
        //long Insert(Users sitemanager);
        //bool Delete(Users sitemanager);
        //bool Update(Users sitemanager);

        AppInfos GetByAppId(string appkey);


        //List<Users> GetPagedList(Page<Users> page);
        //Page<Users> GetPaged(Page<Users> page);
        //List<Users> GetList();
    }
}
