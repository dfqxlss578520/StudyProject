using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    [Table("t_survey_mail_invite")]
    public class SurveyMailInvite : BaseEntity
    {
        //所对应的联系
        public string surveyId;
        public string userId;

        public string subject;
        //问卷名称
        public string dwSurveyName;
        //问卷答卷地址
        public string dwSurveyLink;
        //发件人名称 
        public string dwSendUserName;
        //发件人邮箱
        public string dwSendUserMail;

        public DateTime createDate;


        public string sendcloudMsgId;
        //审核 0未审核  1审核通过
        public int audit;
        //状态 0未发送 1正在发送 2发送完成 3发送失败  4发送异常
        public int status = 0;
        public string errorMsg;

        //总收件人数
        public int inboxNum;
        //已经发送的数
        public int sendNum;
        //发送中成功的数
        public int successNum;
        //发送中失败的数
        public int failNum;

    }

}
