using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 答卷  问答题保存表
    /// </summary>
    [Table("Answer")]
    public class Answer: BaseEntity
    {
        /// <summary>
        /// 所属问卷ID
        /// </summary>
        public int DirId { get; set; }

        /// <summary>
        /// 对应的答卷信息表ID
        /// </summary>
        public int BelongAnswerId { get; set; }

        /// <summary>
        /// 所属题目ID
        /// </summary>
        public int QuId { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public string Answers { get; set; }

        public bool IsValid { get; set; } = true;

        public DateTime CreateDate { get; set; } = DateTime.Now;

    }

}
