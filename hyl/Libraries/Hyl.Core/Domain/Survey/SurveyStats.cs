using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 单个问卷的全局统计信息
    /// </summary>
    [Table("t_survey_stats")]
    public class SurveyStats
    {

        //所属的surveyId
        public string surveyId;

        /**************************************************统计信息 ************************************************/
        /** 时间信息 **/
        //第一条回答数据时间
        public DateTime firstAnswer;
        //最后一条回答数据时间
        public DateTime lastAnswer;
        //回复样本总数  包括所有回复数据
        public int answerNum = 0;
        //回复最少用时  秒
        public int anMinTime = 0;
        //回复平均用时 秒
        public int anAvgTime = 0;

        /** 数据--完成情况    是否全部题都回答  **/
        //未完成的数据
        public int unCompleteNum = 0;
        //完成的数据 
        public int completeNum = 0;

        /** 数据--有效情况   根据设计问卷时指定的必填项 **/
        //无效数据
        public int unEffectiveNum = 0;
        //有效数据
        public int effectiveNum = 0;

        /** 数据--审核情况  **/
        //未处理
        public int unHandleNum = 0;
        //通过
        public int handlePassNum = 0;
        //未通过
        public int handleUnPassNum = 0;

        /** 不同来源数据 **/
        //网调数据
        public int onlineNum = 0;
        //录入数据
        public int inputNum = 0;
        //移动数据
        public int mobileNum = 0;
        //导入数据
        public int importNum = 0;

        //标识是否是最新数据  0不是 1是
        public int isNewData = 0;

    }
}
