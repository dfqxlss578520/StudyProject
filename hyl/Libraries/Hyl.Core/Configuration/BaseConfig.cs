namespace Hyl.Core.Configuration
{
    public static class BaseConfig
    {
        public static string AuthorizationPolicy = "HlyPolicy";
        public static string AuthenticationScheme = "HylCookie";
        public static string LoginPath = "/Admin/Account/Login/";
        public static string AccessDeniedPath = "/Admin/Account/Forbidden/";
        public static string HylSessionKey = "Identity";


        #region SSO
        public static string HylSsoToken = "hylssotoken";
        public static string HylSsoReturnParameter = "hylid";
        public static string HylSsoTokenEncryptSalt = "f1114e97-f765-40a3-9acf-4c3951fc32f1";
        #endregion

        #region CacheTime
        public static int CacheMinute = 60;         //60;
        public static int CacheHour = 3600;         //60 * 60;
        public static int CacheDay = 86400;         //60 * 60 * 24;
        public static int CacheWeek = 604800;       //60 * 60 * 24 * 7;
        public static int CacheMonth = 2592000;     //60 * 60 * 24 * 30;
        public static int CacheYear = 31536000;     //60 * 60 * 24 * 365;
        #endregion

    }
}
