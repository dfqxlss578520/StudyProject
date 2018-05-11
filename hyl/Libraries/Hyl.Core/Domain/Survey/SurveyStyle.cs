using System;
using Dapper.Contrib.Extensions;

namespace Hyl.Core.Domain.Survey
{
    [Table("SurveyStyle")]
    public class SurveyStyle : BaseEntity
    {
        public int DirId { get; set; }
        /// <summary>
        /// 0属于某个问卷的 、1 样式模板
        /// </summary>
        public int SurveyStyleType { get; set; }

        /// <summary>
        /// logo图片
        /// </summary>
        public String SurveyLogoImage { get; set; } = string.Empty;
        public int ShowSurveyLogo { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        public String BodyBgColor { get; set; }
        /// <summary>
        /// 背景图
        /// </summary>
        public String BodyBgImage { get; set; }
        /// <summary>
        /// 内容中间样式
        /// </summary>
        public int ShowBodyBi { get; set; }

        /// <summary>
        /// 按钮背景色
        /// </summary>
        public String SurveyBtnBgColor { get; set; }

        /// <summary>
        /// 问卷宽度
        /// </summary>
        public int SurveyWidth { get; set; }
        /// <summary>
        /// 问卷区背景色
        /// </summary>
        public String SurveyBgColor { get; set; }
        /// <summary>
        /// 问卷区背景图
        /// </summary>
        public String SurveyBgImage { get; set; }

        public int ShowSurveyBi { get; set; }

        /// <summary>
        /// 问卷内边距
        /// </summary>
        public String SurveyPaddingTop { get; set; }
        public String SurveyPaddingBottom { get; set; }
        public String SurveyPaddingLeft { get; set; }
        public String SurveyPaddingRight { get; set; }

        /// <summary>
        /// 是否显示问卷表头
        /// </summary>
        public int ShowSurveyHaed { get; set; }
        /// <summary>
        /// 头部背景色
        /// </summary>
        public String SurveyHeadBgColor { get; set; }
        /// <summary>
        /// 头部背景图
        /// </summary>
        public String SurveyHeadBgImage { get; set; }
        /// <summary>
        /// 表头样式
        /// </summary>
        public int ShowSurveyHbgi { get; set; }
        /// <summary>
        /// 头部宽
        /// </summary>
        public int SurveyHeadWidth { get; set; }
        public int SurveyHeadHeight { get; set; }
        public int SurveyHeadPaddingTop { get; set; }
        public int SurveyHeadPaddingBottom { get; set; }
        public int SurveyHeadPaddingLeft { get; set; }
        public int SurveyHeadPaddingRight { get; set; }

        /// <summary>
        /// 显示问卷标题
        /// </summary>
        public int ShowSurTitle { get; set; } = 1;
        /// <summary>
        /// 显示问卷副标题
        /// </summary>
        public int ShowSurNote { get; set; } = 1;

        public String SurveyContentBgColorTop { get; set; }
        public String SurveyContentBgColorMiddle { get; set; }
        public String SurveyContentBgColorBottom { get; set; }
        public String SurveyContentBgImageTop { get; set; }
        public String SurveyContentBgImageMiddle { get; set; }
        public int ShowSurveyCbim { get; set; }
        public String SurveyContentBgImageBottom { get; set; }
        public int SurveyContentWidth { get; set; }
        public int SurveyContentPaddingTop { get; set; }
        public int SurveyContentPaddingBottom { get; set; }
        public int SurveyContentPaddingLeft { get; set; }
        public int SurveyContentPaddingRight { get; set; }

        /// <summary>
        /// 文本样式
        /// </summary>
        public String QuestionTitleTextColor { get; set; } = "color: rgb(85, 87, 89)";

        public String QuestionOptionTextColor { get; set; } = "rgb(99, 101, 102)";
        public String SurveyTitleTextColor { get; set; } = "rgb(34, 34, 34)";
        public String SurveyNoteTextColor { get; set; } = "rgb(112, 114, 115)";

        /// <summary>
        /// 显示进度条
        /// </summary>
        public int ShowProgressbar { get; set; }
        /// <summary>
        /// 显示头序号
        /// </summary>
        public int ShowTiNum { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public bool IsValid { get; set; } = true;
    }

}
