using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyl.Survey.Models
{
    public class CustomerLoginViewModel
    {
        public bool rst { get; set; }
        public List<DataItem> Data { get; set; }
        public DataItem DefaultModel => Data == null ? (DataItem)null : Data.FirstOrDefault();

    }

    public class DataItem
    {
        public int CatalogID { get; set; }

        public string CatalogName { get; set; }

        public string ParentID { get; set; }

        public int sfid { get; set; }

        public string FullName { get; set; }

        public int CustomerType { get; set; }

        public string CustomerTypeName { get; set; }

        public string CustTel { get; set; }

        public string HouseCode { get; set; }

        public string Tel1 { get; set; }

        public string ShortName { get; set; }
    }
    
}