using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 答卷  多行填空题保存表
    /// </summary>
    [Table("AnswerMultiFillblank")]
    public class AnswerMultiFillblank : BaseEntity
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
        /// 对应的填空项ID
        /// </summary>
        public int QuItemId { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answers { get; set; }

        public bool IsValid { get; set; } = true;

        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}
