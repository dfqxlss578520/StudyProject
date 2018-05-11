using System.Collections.Generic;

namespace Hyl.Core.Domain.PageDomain
{
    /// <summary>
    /// 分页模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T> where T : class
    {
        //原始语句（sql server）
        //SELECT* FROM(SELECT ROW_NUMBER() OVER(ORDER BY { OrderBy}) AS PagedNumber, {SelectColumns} FROM {TableName} {WhereClause}) AS u WHERE PagedNUMBER BETWEEN(({ PageNumber}-1) * {RowsPerPage} + 1) AND({ PageNumber} * {RowsPerPage});SELECT COUNT(1) FROM {TableName} {WhereClause};

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页数据大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// 排序字段 示例：Id 或 Id Desc 或 Id desc,Sort asc
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 查询字段,示例： uid,name,password
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 筛选条件,示例：where uid = @uid and isvalid = 1 ，参数在Parameter中赋值
        /// </summary>
        public string Conditions { get; set; }

        /// <summary>
        /// 参数 示例：{A = 1, B = "b"} // A will be mapped to the param @A, B to the param @B 
        /// </summary>
        public object Parameters { get; set; }

        /// <summary>
        /// 查询结果
        /// </summary>
        public List<T> Items { get; set; }
        
        public Page<T> Current => this;
    }
}
