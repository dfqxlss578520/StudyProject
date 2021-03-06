﻿using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 答卷  多选题保存表
    /// </summary>
    [Table("AnswerCheckbox")]
    public class AnswerCheckbox : BaseEntity
    {
        /// <summary>
        /// 所属问卷ID
        /// </summary>
        public int DirId { get; set; }

        /// <summary>
        /// 题目ID
        /// </summary>
        public int QuId { get; set; }

        /// <summary>
        /// 对应的结果ID
        /// </summary>
        public int QuItemId { get; set; }

        /// <summary>
        /// 对应的答卷信息表
        /// </summary>
        public long BelongAnswerId { get; set; }

        /// <summary>
        /// 复合的说明
        /// </summary>
        public string OtherText { get; set; }

        public bool IsValid { get; set; } = true;

        public DateTime CreateDate { get; set; } = DateTime.Now;


    }
}
