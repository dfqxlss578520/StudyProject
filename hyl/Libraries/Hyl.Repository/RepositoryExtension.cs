using Hyl.Core.Domain.PageDomain;
using Webdiyer.WebControls.Mvc;

namespace Hyl.Repository
{
    public static class RepositoryExtension
    {
        public static PagedList<T> ToMvcPager<T>(this Page<T> pageinfo) where T : class
        {
            return new PagedList<T>(pageinfo.Items, pageinfo.PageIndex, pageinfo.PageSize, pageinfo.TotalItems);
        }
    }
}
