using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 矩陈题-行选项
    /// </summary>
    [Table("QuestionChenRow")]
    public class QuestionChenRow : BaseEntity
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


        [Write(false)]
        public int AnswerCount { get; set; }
        /// <summary>
        /// Avg计算结果
        /// </summary>
        [Write(false)]
        public float AvgNum { get; set; }

    }
}
