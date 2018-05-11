namespace Hyl.SSO.ClientHelper
{
    public class ResponseModel<T>
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

    public class Users
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long Uid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        
    }
}
