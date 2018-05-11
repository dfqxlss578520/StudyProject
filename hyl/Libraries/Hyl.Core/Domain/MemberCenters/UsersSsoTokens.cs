using System.Web.UI.WebControls;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.MemberCenters
{
    /// <summary>
    /// 用户登录成功分配token
    /// </summary>
    [Table("UsersSsoTokens")]
    public class UsersSsoTokens 
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long StId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 分配token值
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 是否有效，true有效
        /// </summary>
        public bool IsValid { get; set; }

    }
}
