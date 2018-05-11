using Hyl.Core;
using Hyl.Core.Domain.MemberCenters;

namespace Hyl.Service.MemberCenters
{
    public interface IUserSnsRelationServics: IServicesDependency
    {
        UserSnsRelation Get(string openid);
        UserSnsRelation Get(string openid, long uid);

        long Insert(UserSnsRelation model);

    }
}
