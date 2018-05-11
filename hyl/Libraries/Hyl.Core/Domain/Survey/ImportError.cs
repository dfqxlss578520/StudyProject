using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{

    /// <summary>
    /// 答案 是非题结果保存表
    /// </summary>
    [Table("t_importError")]
    public class ImportError : BaseEntity
    {

        public string userId;
        //对应的数据ID
        public string dbId;
        //对应的表
        public string tableName;
        //对应的导入文件名
        public string fileName;
        //对应的行号
        public int rowIndex;
        //对应的第一列内容
        public string cell1Value;
        //对应的第二列内容
        public string cell2Value;
        //时间
        public DateTime createDate;

    }


}
