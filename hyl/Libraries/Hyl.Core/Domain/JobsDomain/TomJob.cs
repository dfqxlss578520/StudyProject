using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.JobsDomain
{
    [Table("TomJob")]
    public class TomJob : BaseEntity
    {
        public string CONTENT { get; set; }
        public DateTime createDate { get; set; }

    }
}
