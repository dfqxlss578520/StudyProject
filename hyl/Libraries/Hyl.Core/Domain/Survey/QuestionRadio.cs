using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 单选题选项
    /// </summary>
    [Table("QuestionRadio")]
    public class QuestionRadio : BaseEntity
    {
        /// <summary>
        /// 创建者id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 所属题
        /// </summary>
        public int QuId{ get; set; }

        /// <summary>
        /// 选项标题
        /// </summary>
        public string OptionTitle{ get; set; }

        /// <summary>
        /// 选项内容
        /// </summary>
        public string OptionName{ get; set; }

        /// <summary>
        /// 是否带说明  0否  1是
        /// </summary>
        public int IsNote{ get; set; }

        /// <summary>
        /// 说明的验证方式
        /// </summary>
        public CheckType CheckType{ get; set; }


        /// <summary>
        /// 说明内容是否必填
        /// </summary>
        public int IsRequiredFill{ get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int OrderById{ get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否显示  0不显示 
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// 回答结果数
        /// </summary>
        [Write(false)]
        public int AnswerCount { get; set; }

    }

}
