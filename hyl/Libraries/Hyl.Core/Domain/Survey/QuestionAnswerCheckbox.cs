using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 答卷  多选题保存表
    /// </summary>
    [Table("QuestionAnswerCheckbox")]
    public class QuestionAnswerCheckbox : BaseEntity
    {
        /// <summary>
        /// 所属问卷ID
        /// </summary>
        public string BelongId { get; set; }
        /// <summary>
        /// 对应的答卷信息表
        /// </summary>
        public string BelongAnswerId { get; set; }
        /// <summary>
        /// 题目ID
        /// </summary>
        public string QuId { get; set; }
        /// <summary>
        /// 对应的结果ID
        /// </summary>
        public string QuItemId { get; set; }
        /// <summary>
        /// 复合的说明
        /// </summary>
        public string OtherText { get; set; }

        public int Visibility { get; set; } = 1;


    }
}
