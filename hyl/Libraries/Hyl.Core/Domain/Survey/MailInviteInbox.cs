using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{

    /// <summary>
    /// 答案 是非题结果保存表
    /// </summary>
    [Table("t_mail_invite_inbox")]
    public class MailInviteInbox : BaseEntity
    {

        public string surveyMailInviteId;
        public string userId;

        public string usContactsId;
        public string email;
        public string name;

        //sendclound返回的任务id
        public string sendcloudId;
        //0未发送 1已提交 2请求＝投递 3发送 4打开 5点击 
        //100发送失败
        //201取消订阅 202软退信 203垃圾举报 204无效邮件
        public int status = 0;
    }

}
