using System.Data;
using System.Linq;
using Hyl.Core.Domain.MemberCenters;
using Hyl.Repository;
using StackExchange.Profiling.Helpers.Dapper;

namespace Hyl.Service.MemberCenters
{
    public class UserSnsRelationServics : IUserSnsRelationServics
    {
        private readonly IRepository<UserSnsRelation> _repository;
        private readonly IDbQuery _dbQuery;
        public UserSnsRelationServics(IRepository<UserSnsRelation> repository, IDbQuery dbQuery)
        {
            _repository = repository;
            _dbQuery = dbQuery;
        }

        public UserSnsRelation Get(string openid)
        {
            var conn = _dbQuery.GetDbConnection();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn.Query<UserSnsRelation, Users, UserSnsRelation>(
                "SELECT a.Id,a.OpenId,b.UserName,b.Uid FROM dbo.UserSnsRelation a INNER JOIN dbo.Users b ON a.Uid= b.Uid WHERE OpenId = @OpenId AND a.IsValid =1",
                (sns, user) =>
                {
                    sns.User = user;
                    return sns;
                }, new { OpenId = openid },splitOn: "UserName").FirstOrDefault();
            //return _dbQuery.ge("SELECT * FROM dbo.UserSnsRelation a INNER JOIN dbo.Users b ON a.Uid= b.Uid WHERE OpenId = @OpenId AND a.IsValid =1");
        }

        public UserSnsRelation Get(string openid, long uid)
        {
            return _dbQuery.QuerySingle<UserSnsRelation>(
                "SELECT * FROM dbo.UserSnsRelation WHERE OpenId = @OpenId AND Uid = @Uid AND IsValid =1",
                new { OpenId = openid, Uid = uid });
        }

        public long Insert(UserSnsRelation model)
        {
            return _repository.Add(model);
        }
    }
}
