using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.MemberCenters
{
    /// <summary>
    /// 应用信息
    /// </summary>
    [Table("AppInfos")]
    public class AppInfos
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        [MaxLength(32)]
        public string AppKey { get; set; }

        /// <summary>
        /// 应用秘钥
        /// </summary>
        [MaxLength(32)]
        public string AppSecret { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        [MaxLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 应用默认返回地址
        /// </summary>
        [MaxLength(1024)]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200)]
        public string Remark { get; set; }

        /// <summary>
        /// 应用图标或logo
        /// </summary>
        [MaxLength(1024)]
        public string Icon { get; set; }

        /// <summary>
        /// 是否有效，true有效
        /// </summary>
        public bool IsValid { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
