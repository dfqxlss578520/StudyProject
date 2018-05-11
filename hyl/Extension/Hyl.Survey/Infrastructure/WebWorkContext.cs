using System.Web;

namespace Hyl.Survey.Infrastructure
{
    public class WebWorkContext
    {
        #region  Admin login
        public static Hyl.SSO.ClientHelper.Users AdminUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["ssoUsername"] != null)
                {
                    return (Hyl.SSO.ClientHelper.Users)HttpContext.Current.Session["ssoUsername"];
                }
                return null;
            }
            set { HttpContext.Current.Session["ssoUsername"] = value; }
        }
        public static void LoginOut()
        {
            HttpContext.Current.Session["ssoUsername"] = null;
            AuthorityId = (int)AuthorityEnum.NoAuthority;
        }

        public static bool IsAdminLogin => HttpContext.Current != null && HttpContext.Current.Session["ssoUsername"] != null;

        #endregion

        #region Customer Login
        public static Models.DataItem CustomerUser
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["CustomerUser"] != null)
                {
                    return (Models.DataItem)HttpContext.Current.Session["CustomerUser"];
                }
                return null;
            }
            set { HttpContext.Current.Session["CustomerUser"] = value; }
        }
        public static void CustomerLoginOut()
        {
            HttpContext.Current.Session["CustomerUser"] = null;
            AuthorityId = (int)AuthorityEnum.NoAuthority;
        }

        public static bool IsCustomerUserLogin => HttpContext.Current != null && HttpContext.Current.Session["CustomerUser"] != null;
        #endregion

        public static int AuthorityId
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session["AuthorityId"] != null)
                {
                    return (int)HttpContext.Current.Session["AuthorityId"];
                }
                return (int)AuthorityEnum.NoAuthority;
            }
            set { HttpContext.Current.Session["AuthorityId"] = value; }
        }
    }

    public enum AuthorityEnum
    {
        Admin = 10,
        Customer = 100,
        NoAuthority = 1000000
    }
}