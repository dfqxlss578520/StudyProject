using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    [Table("QuestionMultiFillblank")]
    public class QuestionMultiFillblank : BaseEntity
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
        /// 选项标题
        /// </summary>
        public string OptionTitle { get; set; }
        
        /// <summary>
        /// 说明的验证方式
        /// </summary>
        public CheckType CheckType { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int OrderById { get; set; }

        /// <summary>
        /// 是否显示  false不显示 
        /// </summary>
        public bool IsValid { get; set; } = true;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Write(false)]
        public int AnswerCount { get; set; }
    }
}
