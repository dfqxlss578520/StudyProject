using Hyl.Core;
using Hyl.Core.Domain.MemberCenters;

namespace Hyl.Service.MemberCenters
{
    public interface IUsersSsoTokensServices : IServicesDependency
    {
        UsersSsoTokens GetUserToken(Users users);
        UsersSsoTokens GetByToken(string token);
    }
}
