using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    [Table("AnswerChenFillblank")]
    public class AnswerChenFillblank : BaseEntity
    {
        #region Model Base
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
        /// 对应的列ID
        /// </summary>
        public int QuColId { get; set; }

        //结果值
        public string AnswerValue { get; set; }

        public bool IsValid { get; set; } = true;

        public DateTime CreateDate { get; set; } = DateTime.Now; 
        #endregion

        [Write(false)]
        public int Count { get; set; }
    }
}
