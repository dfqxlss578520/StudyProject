using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 复合矩阵单选题
    /// </summary>
    [Table("AnswerCompChenRadio")]
    public class AnswerCompChenRadio : BaseEntity
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
        /// 对应的行ID
        /// </summary>
        public int QuRowId { get; set; }

        /// <summary>
        /// 对应的列ID
        /// </summary>
        public int QuColId { get; set; }

        /// <summary>
        /// 对应的结果id 即选项ID
        /// </summary>
        public int QuOptionId { get; set; }

        public bool IsValid { get; set; } = true;

        public DateTime CreateDate { get; set; } = DateTime.Now;

    }
}
