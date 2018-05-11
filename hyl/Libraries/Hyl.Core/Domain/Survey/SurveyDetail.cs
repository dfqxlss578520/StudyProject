using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 具体的一次调查
    /// </summary>
    [Table("SurveyDetail")]
    public class SurveyDetail : BaseEntity
    {
        /// <summary>
        /// 所对应的surveyDirectory的ID
        /// </summary>
        public int DirId{ get; set; }

        /// <summary>
        /// 问卷有效性 限制 --------- 1不限制， 2使用Cookie技术， 3使用来源IP检测
        /// 4 每台电脑或手机只能答一次
        /// </summary>
        public int Effective { get; set; } = 1;

        /// <summary>
        /// 有效性间隔时间
        /// </summary>
        public int EffectiveTime { get; set; } = 5;

        /// <summary>
        /// 每个IP只能答一次 1是 0否
        /// </summary>
        public int EffectiveIp { get; set; } = 0;

        /// <summary>
        /// 防刷新  1启用 0不启用
        /// </summary>
        public int Refresh { get; set; } = 1;
        public int RefreshNum { get; set; } = 3;

        /// <summary>
        /// 调查规则  --------  1公开, 2私有, 3令牌 
        /// 3 表示启用访问密码
        /// </summary>
        public int Rule { get; set; } = 1;
        public string RuleCode { get; set; } = "令牌";

        /// <summary>
        /// 结束方式  ---------- 1手动结束   2依据结束时间  3依据收到的份数
        /// </summary>
        public int EndType { get; set; } = 1;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime{ get; set; } =DateTime.Now;

        /// <summary>
        /// 收到的份数
        /// </summary>
        public int EndNum { get; set; } = 1000;

        /// <summary>
        /// 问卷说明
        /// </summary>
        public string SurveyNote { get; set; } = string.Empty;

        /// <summary>
        /// 是否依据收到的份数结束
        /// </summary>
        public int YnEndNum { get; set; } = 0;
        public int YnEndTime { get; set; } = 0;

        /// <summary>
        /// 问卷
        /// 问卷下面有多少题目数  ---  
        /// </summary>
        public int SurveyQuNum { get; set; } = 0;

        /// <summary>
        /// 可以回答的最少选项数目 
        /// </summary>
        public int AnItemLeastNum { get; set; } = 0;

        /// <summary>
        /// 可以回答的最多选项数目
        /// </summary>
        public int AnItemMostNum { get; set; } = 0;

        /// <summary>
        /// 只有邮件邀请唯一链接的受访者可回答  1启用 0不启用
        /// </summary>
        public int MailOnly { get; set; } = 0;

        /// <summary>
        /// 显示分享
        /// </summary>
        public int ShowShareSurvey { get; set; } = 1;
        public int ShowAnswerDa { get; set; } = 0;

    }

}
