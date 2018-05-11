using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyl.Web.Infrastructure
{
    public class AppsettingModel
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class ConnectionStrings
    {
        public string Default { get; set; }
    }
}
