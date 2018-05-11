namespace Hyl.Core.Domain.Survey
{
    public enum CheckType
    {
        /// <summary>
        /// 无验证
        /// </summary>
        NO,
        /// <summary>
        /// Email
        /// </summary>
        EMAIL,
        /// <summary>
        /// 字符长度
        /// </summary>
        STRLEN,
        /// <summary>
        /// 禁止中文
        /// </summary>
        UNSTRCN,
        /// <summary>
        /// 仅许中文
        /// </summary>
        STRCN,
        /// <summary>
        /// 数值
        /// </summary>
        NUM,
        /// <summary>
        /// 电话号码
        /// </summary>
        TELENUM,
        /// <summary>
        /// 手机号码
        /// </summary>
        PHONENUM,
        /// <summary>
        /// 日期
        /// </summary>
        DATE,
        /// <summary>
        /// 身份证号
        /// </summary>
        IDENTCODE,
        /// <summary>
        /// 邮政编码
        /// </summary>
        ZIPCODE,
        /// <summary>
        /// 网址
        /// </summary>
        URL,
        /// <summary>
        /// 电话或手机号
        /// </summary>
        TELE_PHONE_NUM


    }
}
