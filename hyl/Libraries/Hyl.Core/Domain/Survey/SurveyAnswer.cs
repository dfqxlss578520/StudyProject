using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 具体的一次调查
    /// </summary>
    [Table("SurveyAnswer")]
    public class SurveyAnswer : BaseEntity
    {
        /// <summary>
        /// 问卷ID
        /// </summary>
        public int DirId { get; set; }

        /// <summary>
        /// 回答者ID
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 回答时间
        /// </summary>
        public DateTime BeginAnswerDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 回答时间
        /// </summary>
        public DateTime EndAnswerDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 用时
        /// </summary>
        public float TotalTime { get; set; }

        /// <summary>
        /// 回答者IP
        /// </summary>
        public string IpAddr { get; set; }

        /// <summary>
        /// 回答者是详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 回答者城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 回答者MAC
        /// </summary>
        public string PcMac { get; set; }

        /// <summary>
        /// 回答的题数
        /// </summary>
        public int QuNum { get; set; }

        /** 回复的此条数据统计信息 **/

        /** 数据--完成情况    是否全部题都回答  **/
        /// <summary>
        /// 是否完成  1完成
        /// </summary>
        public int IsComplete { get; set; }

        /// <summary>
        /// 回答的题数
        /// </summary>
        public int CompleteNum { get; set; }

        /// <summary>
        /// 回答的题项目数 ---- 表示有些题下面会有多重回答
        /// </summary>
        public int CompleteItemNum { get; set; }

        /** 数据--有效情况   根据设计问卷时指定的必填项 **/
        /// <summary>
        /// 是否是有效数据  1有效
        /// </summary>
        public int IsEffective { get; set; }

        /** 数据--审核情况  **/
        /// <summary>
        /// 审核状态  0未处理 1通过 2不通过
        /// </summary>
        public int HandleState { get; set; }

        /** 不同来源数据 **/
        /// <summary>
        /// 数据来源  0泓源林  1录入数据 2移动数据 3导入数据
        /// </summary>
        public int DataSource { get; set; }





        #region  Answer
        
        [Write(false)]
        public List<AnswerRadio> AnswerRadio { get; set; } = new List<AnswerRadio>();

        [Write(false)]
        public List<AnswerCheckbox> AnswerCheckbox { get; set; } = new List<AnswerCheckbox>();

        [Write(false)]
        public List<AnswerFillblank> AnswerFillblank { get; set; } = new List<AnswerFillblank>();

        [Write(false)]
        public List<AnswerScore> AnswerScore { get; set; } = new List<AnswerScore>();

        [Write(false)]
        public List<AnswerOrder> AnswerOrder { get; set; } = new List<AnswerOrder>();
        
        [Write(false)]
        public List<AnswerMultiFillblank> AnswerMultiFillblank { get; set; } = new List<AnswerMultiFillblank>();
        
        [Write(false)]
        public List<AnswerChenRadio> AnswerChenRadio { get; set; } = new List<AnswerChenRadio>();

        [Write(false)]
        public List<AnswerChenCheckbox> AnswerChenCheckbox { get; set; } = new List<AnswerChenCheckbox>();
        
        [Write(false)]
        public List<AnswerChenScore> AnswerChenScore { get; set; } = new List<AnswerChenScore>();

        [Write(false)]
        public List<AnswerChenFillblank> AnswerChenFillblank { get; set; } = new List<AnswerChenFillblank>();

        #endregion

    }
}
