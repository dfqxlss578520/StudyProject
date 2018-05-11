using System;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace Hyl.Core.Domain.Survey
{
    [Table("Question")]
    public class Question : BaseEntity
    {
        #region Model Base
        /// <summary>
        /// 创建者id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 所属问卷或题库
        /// </summary>
        public int DirId { get; set; }

        /// <summary>
        /// 题目名称
        /// </summary>
        public string QuName { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        public string QuTitle { get; set; }

        /// <summary>
        /// 题目说明
        /// </summary>
        public string QuNote { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }


        /// <summary>
        /// 题目类型
        /// </summary>
        public QuType QuType { get; set; }

        /// <summary>
        ///  标记     1题库中的题   2问卷中的题
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int OrderById { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否是大小题    1默认题  2大题  3大题下面的小题
        /// </summary>
        public int QuTag { get; set; } = 1;

        /// <summary>
        /// 所属大题  只有小题才有此属性 即quTag=3的题
        /// </summary>
        public int ParentQuId { get; set; }

        /// <summary>
        /// 是非题的选项
        /// </summary>
        public YesnoOption YesnoOption { get; set; }

        /// <summary>
        /// 是否必答 0非必答 1必答
        /// </summary>
        public int IsRequired { get; set; }

        /// <summary>
        /// 说明的验证方式
        /// </summary>
        public CheckType CheckType { get; set; }

        /// <summary>
        /// 枚举题 枚举项数目 ,评分题起始分值
        /// </summary>
        public int ParamInt01 { get; set; } = 3;

        /// <summary>
        /// 评分题，最大分值
        /// </summary>
        public int ParamInt02 { get; set; } = 10;
        
        /// <summary>
        /// 如果是复制的题，则有复制于那一题
        /// </summary>
        public int CopyFromId { get; set; }

        /// <summary>
        /// 控制性属性 
        /// 1水平显示 2垂直显示
        /// </summary>
        public int Hv { get; set; } = 2;

        /// <summary>
        /// 选项随机排列  1随机排列 0不随机排列
        /// </summary>
        public int RandOrder { get; set; }

        /// <summary>
        /// 按列显示时，列数
        /// </summary>
        public int CellCount { get; set; }

        /// <summary>
        /// 联系人属性
        /// 1关联到联系人属性  0不关联到联系人属性
        /// </summary>
        public int ContactsAttr { get; set; }

        /// <summary>
        /// 关联的联系人字段
        /// </summary>
        public string ContactsField { get; set; }

        /// <summary>
        /// 填空的input
        /// </summary>
        public int AnswerInputWidth { get; set; }
        public int AnswerInputRow { get; set; }

        public bool IsValid { get; set; } = true;
        #endregion


        #region Answer
        [Write(false)]
        public List<AnswerRadio> AnRadios { get; set; } = new List<AnswerRadio>();
        [Write(false)]
        public List<AnswerCheckbox> AnCheckboxs { get; set; } = new List<AnswerCheckbox>();
        [Write(false)]
        public List<AnswerFillblank> AnFillblanks { get; set; } = new List<AnswerFillblank>();
        [Write(false)]
        public List<AnswerScore> AnScores { get; set; } = new List<AnswerScore>();
        [Write(false)]
        public List<AnswerOrder> AnOrders { get; set; } = new List<AnswerOrder>();
        [Write(false)]
        public List<AnswerMultiFillblank> AnMultiFillblanks { get; set; } = new List<AnswerMultiFillblank>();
        

        [Write(false)]
        public List<AnswerChenRadio> AnChenRadio { get; set; } = new List<AnswerChenRadio>();
        [Write(false)]
        public List<AnswerChenCheckbox> AnChenCheckbox { get; set; } = new List<AnswerChenCheckbox>();
        [Write(false)]
        public List<AnswerChenScore> AnChenScore { get; set; } = new List<AnswerChenScore>();
        [Write(false)]
        public List<AnswerChenFillblank> AnChenFillblank { get; set; } = new List<AnswerChenFillblank>();
        #endregion

        #region question 

        [Write(false)]
        public List<QuestionLogic> QuestionLogics { get; set; } = new List<QuestionLogic>();

        [Write(false)]
        public List<QuestionRadio> QuRadios { get; set; } = new List<QuestionRadio>();

        [Write(false)]
        public List<QuestionCheckbox> QuCheckboxes { get; set; } = new List<QuestionCheckbox>();

        [Write(false)]
        public List<QuestionScore> QuScores { get; set; } = new List<QuestionScore>();

        [Write(false)]
        public List<QuestionOrderby> QuOrderbies { get; set; } = new List<QuestionOrderby>();

        [Write(false)]
        public List<QuestionMultiFillblank> QuMultiFillblanks { get; set; } = new List<QuestionMultiFillblank>();

        [Write(false)]
        public List<QuestionChenRow> QuChenRows { get; set; } = new List<QuestionChenRow>();

        [Write(false)]
        public List<QuestionChenColumn> QuChenColumns { get; set; } = new List<QuestionChenColumn>();

        //[Write(false)]
        //public List<QuestionChenOption> QuChenOptions { get; set; } = new List<QuestionChenOption>();

        #endregion

        #region Extend Property
        /// <summary>
        /// 统计列表拓展字段
        /// </summary>
        [Write(false)]
        public int EmptyCount { get; set; }
        [Write(false)]
        public int NoEmptyCount { get; set; }
        /// <summary>
        /// 回答结果数
        /// </summary>
        [Write(false)]
        public int AnswerCount { get; set; }
        /// <summary>
        /// 统计json
        /// </summary>
        [Write(false)]
        public string StatJson { get; set; }
        #endregion


    }

}
