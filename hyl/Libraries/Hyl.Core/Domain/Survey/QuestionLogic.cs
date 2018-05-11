using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 题目逻辑设置
    /// </summary>
    [Table("QuestionLogic")]
    public class QuestionLogic : BaseEntity
    {
        /// <summary>
        /// 创建者id
        /// </summary>
        public long Uid { get; set; }
        
        /// <summary>
        /// 回答选择的题ID
        /// </summary>
        public int QuId { get; set; }

        /// <summary>
        /// 回答选择题的选项ID  （0任意选项）
        /// </summary>
        public int QuItemId { get; set; }

        /// <summary>
        /// 要跳转的题  (end1提前结束-计入结果  end2提前结束-不计结果)
        /// </summary>
        public int SkipToQuId { get; set; }

        /// <summary>
        /// 逻辑类型  (1=跳转,2显示)
        /// </summary>
        public string LogicType { get; set; } = "1";

        /// <summary>
        /// 评分题 ge大于，le小于
        /// </summary>
        public string GeLe { get; set; }

        public int ScoreNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否显示  1有效 0无效
        /// </summary>
        public int IsValid { get; set; } = 1;

        /// <summary>
        /// 排序
        /// </summary>
        [Write(false)]
        public string Title { get; set; }

    }

}
