using System.Collections.Generic;
using Hyl.Core;
using Hyl.Core.Domain.MemberCenters;

namespace Hyl.Service.MemberCenters
{
    public interface IUsersServices : IServicesDependency
    {
        void InsertAddBatch(List<Users> sitemanager);
        long Insert(Users sitemanager);
        //bool Delete(Users sitemanager);
        //bool Update(Users sitemanager);

        Users GetById(long id);
        Users GetByUsername(string username);
        Users GetByUsernameCompany(string username,int companyid);
        Users UserLogin(Users users);


        //List<Users> GetPagedList(Page<Users> page);
        //Page<Users> GetPaged(Page<Users> page);
        //List<Users> GetList();
    }
}
