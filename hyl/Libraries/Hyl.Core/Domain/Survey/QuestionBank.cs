using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{

    /// <summary>
    /// 题库
    /// </summary>
    [Table("t_question_bank")]
    public class QuestionBank : BaseEntity
    {

        public string parentId = "";
        public string bankName;
        //说明
        public string bankNote;
        //1目录，2题库
        public int dirType = 1;
        //创建时间
        public DateTime createDate;
        //是否显示  1显示 0不显示
        public int Visibility { get; set; } = 1;

        //是用户共享题库还是官方问卷库
        //共享题库 0 官方库   1用户共享
        public int bankTag = 0;
        //状态 0设计状态  1发布状态
        public int bankState = 0;
        //创建者
        public string userId;
        //问卷所属的组  功能分组
        public string groupId1;
        //分组2	行业分组
        public string groupId2;
        //题目数
        public int quNum = 0;
        //引用次数
        public int excerptNum = 0;

    }

}
