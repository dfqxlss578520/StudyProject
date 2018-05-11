using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.MemberCenters
{
    [Table("UserSnsRelation")]
    public class UserSnsRelation
    {
        public int Id { get; set; }

        public string OpenId { get; set; }

        public long Uid { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public bool IsValid { get; set; } = true;

        [Write(false)]
        public Users User { get; set; }

    }
}
