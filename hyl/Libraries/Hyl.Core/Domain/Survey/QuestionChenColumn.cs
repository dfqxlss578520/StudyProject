using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 矩陈题-列选项
    /// </summary>
    [Table("QuestionChenColumn")]
    public class QuestionChenColumn : BaseEntity
    {
        /// <summary>
        /// 用户Uid
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 所属题
        /// </summary>
        public int QuId { get; set; }

        /// <summary>
        /// 选项问题
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int OrderById { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否显示  false不显示 
        /// </summary>
        public bool IsValid { get; set; } = true;

    }

}
