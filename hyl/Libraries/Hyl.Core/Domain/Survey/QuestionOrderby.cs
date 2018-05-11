using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 评分题 行选项
    /// </summary>
    [Table("QuestionOrderby")]
    public class QuestionOrderby : BaseEntity
    {
        /// <summary>
        /// 创建者id
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 所属题
        /// </summary>
        public int QuId { get; set; }

        /// <summary>
        /// 选项内容
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string OptionTitle { get; set; }
        
        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderById { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否显示  0不显示 
        /// </summary>
        public int IsValid { get; set; } = 1;

        [Write(false)]
        public int SumNum { get; set; }
    }


}
