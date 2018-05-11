using System.ComponentModel;

namespace Hyl.Core.Domain.Survey
{
    public enum QuType
    {
        //不要在题目中间新增题型，如果有需要，请在最后加（数据库中保存的是题型索引）
        [Description("单选题")]
        RADIO,

        [Description("多选题")]
        CHECKBOX,

        [Description("填空题")]
        FILLBLANK,

        [Description("多项填空题")]
        MULTIFILLBLANK,
        
        [Description("评分题")]
        SCORE,


        [Description("排序题")]
        ORDERQU,

        [Description("矩阵单选题")]
        CHENRADIO,

        [Description("矩阵填空题")]
        CHENFBK,

        [Description("矩阵多选题")]
        CHENCHECKBOX,

        [Description("分页标记")]
        PAGETAG,

        [Description("段落说明")]
        PARAGRAPH,

        [Description("矩阵评分题")]
        CHENSCORE

        //[Description("是非题")]
        //YESNO,

        //[Description("复合矩阵单选题")]
        //COMPCHENRADIO,

        //[Description("文件上传题")]
        //UPLOADFILE,

        //[Description("多行填空题")]
        //ANSWER,//原问答题

        //[Description("大题")]
        //BIGQU,

        //[Description("枚举题")]
        //ENUMQU,

        //[Description("比重题")]
        //PROPORTION,

        //[Description("复合单选")]
        //COMPRADIO,

        //[Description("复合多选")]
        //COMPCHECKBOX,
    }
}
