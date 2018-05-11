using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{

    /// <summary>
    /// 评分题
    /// </summary>
    [Table("AnswerScore")]
    public class AnswerScore : BaseEntity
    {
        /// <summary>
        /// 所属问卷ID
        /// </summary>
        public int DirId { get; set; }

        /// <summary>
        /// 对应的答卷信息表ID
        /// </summary>
        public long BelongAnswerId { get; set; }

        /// <summary>
        /// 所属题目ID
        /// </summary>
        public int QuId { get; set; }

        /// <summary>
        /// 对应的行ID
        /// </summary>
        public int QuRowId { get; set; }

        /// <summary>
        /// 对应的结果，即分值 
        /// </summary>
        public int AnswserScore { get; set; }

        public bool IsValid { get; set; } = true;

        public DateTime CreateDate { get; set; } = DateTime.Now;
    }

}
