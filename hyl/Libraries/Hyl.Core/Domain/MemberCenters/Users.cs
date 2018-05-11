using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Hyl.Core.Domain.MemberCenters
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    [Table("Users")]
    public class Users
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        public long Uid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonIgnore]
        public string Password { get; set; }
        /// <summary>
        /// 加密salt
        /// </summary>
        [JsonIgnore]
        public string Salt { get; set; }
        /// <summary>
        /// 是否有效，true有效
        /// </summary>
        [JsonIgnore]
        public bool IsValid { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public int Company { get; set; }

    }

    public class UserValidateViewModel
    {
        public long Uid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
}
