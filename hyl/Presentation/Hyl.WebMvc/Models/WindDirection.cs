using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyl.WebMvc.Models
{
    public class WindDirection
    {
        public virtual int Id { get; set; }

        public virtual string Code { get; set; }

        public virtual string CnName { get; set; }

        public virtual string EnName { get; set; }
        
    }
}