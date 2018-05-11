using System;
using Hyl.Core.Caching;
using Hyl.Core.Configuration;
using Hyl.Core.Domain.MemberCenters;
using Hyl.Core.Helpers.Utility;
using Hyl.Repository;

namespace Hyl.Service.MemberCenters
{
    public class UsersSsoTokensServices : IUsersSsoTokensServices
    {
        private readonly IDbQuery _dbQuery;
        private readonly ICacheManager _cacheManager;
        public UsersSsoTokensServices(ICacheManager cacheManager, IDbQuery dbQuery)
        {
            _cacheManager = cacheManager;
            _dbQuery = dbQuery;
        }

        #region Extendsion
        /// <summary>
        /// 如果没有token则新增，如果有则查询返回
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public UsersSsoTokens GetUserToken(Users users)
        {
            string encrypttoken = Encrypt.MD5Java(string.Format("{0}|{1}", users.Uid, Guid.NewGuid()));
            return _dbQuery.QuerySingle<UsersSsoTokens>("IF NOT EXISTS ( SELECT StId FROM UsersSsoTokens WHERE Uid = @uid AND IsValid = 1 ) BEGIN INSERT INTO dbo.UsersSsoTokens( Uid, Token, CreateDate, IsValid ) VALUES ( @uid, @token, GETDATE(), 1 ); SELECT * FROM dbo.UsersSsoTokens WHERE Uid = @uid AND IsValid=1 END ELSE SELECT * FROM dbo.UsersSsoTokens WHERE Uid = @uid AND IsValid = 1;", new { uid = users.Uid, token = encrypttoken });
        }

        public UsersSsoTokens GetByToken(string token)
        {
            if (_cacheManager.IsSet(token))
            {
                return _cacheManager.Get<UsersSsoTokens>(token);
            }
            var tokenModel = _dbQuery.QuerySingle<UsersSsoTokens>("SELECT * FROM dbo.UsersSsoTokens WHERE Token =@token AND IsValid =1;", new { token = token });
            _cacheManager.Set(token, tokenModel, BaseConfig.CacheWeek);
            return tokenModel;
        }

        #endregion
    }
}
