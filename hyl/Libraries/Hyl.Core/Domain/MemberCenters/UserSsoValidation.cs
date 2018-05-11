namespace Hyl.Core.Domain.MemberCenters
{
    /// <summary>
    /// 单点登录验证结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UserSsoValidation<T> : BaseEntity
    {
        /// <summary>
        /// 返回执行结果
        /// </summary>
        public bool Rst { get; set; }
        /// <summary>
        /// 返回执行结果的提示
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 存储执行结果数据
        /// </summary>
        public T Data { get; set; }
    }
}
