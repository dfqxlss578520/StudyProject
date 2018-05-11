using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using Hyl.Core.Domain.PageDomain;
using Newtonsoft.Json;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 问卷目录及问卷
    /// </summary>
    [Table("SurveyDirectory")]
    public class SurveyDirectory : BaseEntity
    {

        /// <summary>
        /// 创建者ID
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 父分类ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 活动名
        /// </summary>
        public string SurveyName { get; set; }

        /// <summary>
        /// 1目录，2问卷
        /// </summary>
        public int DirType { get; set; } = 1;

        /// <summary>
        /// 当dirType=2时，所对应的问卷详细信息表Id  
        /// </summary>
        public int SurveyDetailId { get; set; }

        /// <summary>
        /// 问卷所属的问卷模块   1问卷模块，2测评，3投票 4＝3.0版引用模板
        /// </summary>
        public int SurveyModel { get; set; } = 1;

        /// <summary>
        /// 问卷状态  0默认设计状态  1执行中 2结束 
        /// </summary>
        public int SurveyState { get; set; } 

        /// <summary>
        /// 问卷标识 默认 0待审核  1审核通过  2审核未通过
        /// </summary>
        public int SurveyTag { get; set; } = 1;

        /// <summary>
        /// 引用次数
        /// </summary>
        public int ExcerptNum { get; set; }

        /// <summary>
        /// 问卷下面有多少题目数
        /// </summary>
        public int SurveyQuNum { get; set; }
        
        /// <summary>
        /// 可以回答的最少选项数目
        /// </summary>
        public int AnItemLeastNum { get; set; }
        
        /// <summary>
        /// 回答次数
        /// </summary>
        public int AnswerNum { get; set; }
        
        /// <summary>
        /// 是否公开结果  0不  1公开
        /// </summary>
        public int ViewAnswer { get; set; }

        /// <summary>
        /// 是否共享问卷  0不共享 1共享 
        /// </summary>
        public bool IsShare { get; set; }
        
        /// <summary>
        /// 用于短链接的ID
        /// </summary>
        public string Sid { get; set; }
        
        /// <summary>
        /// 是否显示  1显示 0不显示
        /// </summary>
        public int IsValid { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
        [Write(false)]
        public DateTime CreateDate { get; set; }
        
        /// <summary>
        /// 静态HTML保存路径
        /// </summary>
        public string HtmlPath { get; set; }

            

        [Write(false)]
        public SurveyDetail SurveyDetail { get; set; } = new SurveyDetail();

        [Write(false)]
        public List<Question> Questions { get; set; } = new List<Question>();

        [Write(false)]
        public SurveyStyle SurveyStyle { get; set; } = new SurveyStyle();

        [JsonIgnore]
        [Write(false)]
        public Page<SurveyAnswer> PageSurveyAnswer { get; set; } = new Page<SurveyAnswer>();
    }

}
