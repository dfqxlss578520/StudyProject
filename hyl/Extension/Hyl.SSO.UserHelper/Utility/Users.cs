using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Users 
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 加密salt
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// 是否有效，true有效
        /// </summary>
        public bool IsValid { get; set; }

    }
}
