namespace Hyl.Core.Domain.Survey
{

    /// <summary>
    /// 系统smtp服务器
    /// </summary>
    public class SmtpServer : BaseEntity
    {
        //SMTP服务器
        public string smtpServer;
        //端口
        public string post;
        //验证
        public int isChecked;
        //发信人邮件地址
        public string sendEmail;
        //SMTP 身份验证用户名
        public string smtpUserName;
        //SMTP 身份验证密码
        public string smtpPassword;
    }

}
