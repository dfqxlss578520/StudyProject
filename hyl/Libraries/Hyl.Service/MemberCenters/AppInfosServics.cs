using Hyl.Core.Caching;
using Hyl.Core.Configuration;
using Hyl.Core.Domain.MemberCenters;
using Hyl.Repository;

namespace Hyl.Service.MemberCenters
{
    public class AppInfosServics : IAppInfosServics
    {
        private readonly IDbQuery _dbQuery;
        private readonly ICacheManager _cacheManager;
        public AppInfosServics(IDbQuery dbQuery, ICacheManager cacheManager)
        {
            _dbQuery = dbQuery;
            _cacheManager = cacheManager;
        }


        #region Method

        public AppInfos GetByAppId(string appkey)
        {
            if (_cacheManager.IsSet(appkey))
            {
                return _cacheManager.Get<AppInfos>(appkey);
            }
            var appinfo = _dbQuery.QuerySingle<AppInfos>("SELECT AppKey,AppSecret FROM dbo.AppInfos WHERE AppKey =@appkey", new { appkey = appkey });
            _cacheManager.Set(appkey, appinfo, BaseConfig.CacheWeek);

            return appinfo;
        }
        #endregion


    }
}
