$(document).ready(function () {
    $(".pc_themeContentScorll").mCustomScrollbar({
        theme: "dark"
    });
    $("#confirgDevSuvey").click(function () {
        window.location.href = "@Url.Action("Publish", "Survey", new {surveyId = Model.Id})";
    });

    $("#preview_head .leftTabbar ul li").hover(function () {
        var visibleDialog = $(this).find(".tabbarDialog:visible");
        if (!visibleDialog[0]) {
            $(this).addClass("hover");
        }
    }, function () {
        $(this).removeClass("hover");
    });

    $("#dw_body").hover(function () {
        var visibleDialog = $("#preview_head").find(".tabbarDialog:visible");
        if (visibleDialog[0]) {
            var visibleDialogClass = visibleDialog.attr("class");
            if (visibleDialogClass.indexOf("tabbarDialogTheme") >= 0) {
                visibleDialog.css({ opacity: 0.2, borderWidth: "2px" });
            }
        }
    }, function () {
        var visibleDialog = $("#preview_head").find(".tabbarDialog:visible");
        if (visibleDialog[0]) {
            var visibleDialogClass = visibleDialog.attr("class");
            if (visibleDialogClass.indexOf("tabbarDialogTheme") >= 0) {
                visibleDialog.css({ opacity: 1 });
            }
        }
    });

    var leftTabbarLiClickStatus = 0;
    $(".tabbarDialog").click(function () {
        leftTabbarLiClickStatus = 1;
    });

    $("#preview_head .leftTabbar ul li").click(function () {
        if (leftTabbarLiClickStatus != 1) {
            $(".tabbarDialog").not($(this).find(".tabbarDialog")).hide();
            $(this).find(".tabbarDialog").toggle();
            $(".js-tabselected").removeClass("js-tabselected");
            $(this).removeClass("hover");
            var visibleDialog = $(this).find(".tabbarDialog:visible");
            if (visibleDialog[0]) {
                $(this).addClass("js-tabselected");
                var visibleDialogClass = visibleDialog.attr("class");
                if (visibleDialogClass.indexOf("tabbarDialogTheme") >= 0) {
                    visibleDialog.css({ opacity: 1, borderWidth: "1px" });
                    visibleDialog.find(".tabbarDialogContent").css({ visibility: "visible" });
                }
            }
        }
        leftTabbarLiClickStatus = 2;
    });

    var inputTempVal = null;
    $(".surveyPaddingInput").focus(function () {
        inputTempVal = $(this).val();
    });
    $(".surveyPaddingInput").blur(function () {
        var thVal = $(this).val();
        if (/^\d+$/.test(thVal)) {
            setSurveyStyle($(this));
        } else {
            $(this).val(inputTempVal);
        }
    });

    $(".surveyStyleChange").change(function () {
        setSurveyStyle($(this));
    });

    function setSurveyStyle(eventObj) {
        var objName = $(eventObj).attr("name");
        var thPaddingVal = $(eventObj).val();
        if (objName == "surveyHeadPaddingTop") {
            $("#dwSurveyHeader").css({ "padding-top": thPaddingVal + "px" });
        } else if (objName == "surveyHeadPaddingBottom") {
            $("#dwSurveyHeader").css({ "padding-bottom": thPaddingVal + "px" });
        } else if (objName == "surveyContentPaddingLeft") {
            $("#dwSurveyQuContentBg").css({ "padding-left": thPaddingVal + "px" });
        } else if (objName == "surveyContentPaddingRight") {
            $("#dwSurveyQuContentBg").css({ "padding-right": thPaddingVal + "px" });
        } else if (objName == "surveyWidth") {
            $("#dw_body_content").width(thPaddingVal);
        }
    }

    $(document).click(function () {
        if (leftTabbarLiClickStatus == 0) {
            $(".tabbarDialog").hide();
            $(".js-tabselected").removeClass("js-tabselected");
        }
        leftTabbarLiClickStatus = 0;
    });

    //分页设置 nextPage_a prevPage_a
    $(".nextPage_a").click(function () {
        var thParent = $(this).parent();
        var nextPageNo = thParent.find("input[name='nextPageNo']").val();
        $(".li_surveyQuItemBody").hide();
        $(".surveyQu_" + nextPageNo).fadeIn("slow");
        //$(window).scrollTop(10);
        $("html,body").animate({ scrollTop: 10 }, 500);
        return false;
    });
    $(".prevPage_a").click(function () {
        var thParent = $(this).parent();
        var prevPageNo = thParent.find("input[name='prevPageNo']").val();
        $(".li_surveyQuItemBody").hide();
        $(".surveyQu_" + prevPageNo).fadeIn("slow");
        $(window).scrollTop(10);
        return false;
    });

    $("#saveStyleDev").click(function () {
        var url = '@Url.Action("StyleSave", "Survey")';
        var surveyId = $("#id").val();
        var bodyBgColor = $("input[name='bodyBgColor']").val();
        var bodyBgImage = $("input[name='bodyBgImage']").val();
        var showBodyBi = $("input[name='showBodyBi']").prop("checked") ? 1 : 0;
        var survey = new Object();
        survey.id = surveyId;
        survey.SurveyStyle = new Object();
        survey.SurveyStyle.DirId = surveyId;
        survey.SurveyStyle.SurveyStyleType = 0;
        survey.SurveyStyle.BodyBgColor = bodyBgColor;
        survey.SurveyStyle.BodyBgImage = bodyBgImage;
        survey.SurveyStyle.showBodyBi = showBodyBi;

        survey.SurveyDetail = new Object();
        survey.SurveyDetail.DirId = surveyId;

        //收集规则
        var effective = $("input[name='effective']:checked")[0] ? "4" : "0";
        var effectiveIp = $("input[name='effectiveIp']:checked")[0] ? "1" : "0";
        var rule = $("input[name='rule']:checked")[0] ? "3" : "0";
        var ruleCode = $("input[name='ruleCode']").val();
        var refresh = $("input[name='refresh']:checked")[0] ? "1" : "0";
        var mailOnly = $("input[name='mailOnly']:checked")[0] ? "1" : "0";
        var ynEndNum = $("input[name='ynEndNum']:checked")[0] ? "1" : "0";
        var ynEndTime = $("input[name='ynEndTime']:checked")[0] ? "1" : "0";
        var endTime = $("input[name='endTime']").val();
        var endNum = $("input[name='endNum']").val();
        var showShareSurvey = $("input[name='showShareSurvey']:checked")[0] ? "1" : "0";
        var showAnswerDa = $("input[name='showAnswerDa']:checked")[0] ? "1" : "0";


        survey.SurveyDetail.effective = effective;
        survey.SurveyDetail.effectiveIp = effectiveIp;
        survey.SurveyDetail.rule = rule;
        survey.SurveyDetail.refresh = refresh;
        survey.SurveyDetail.ruleCode = ruleCode;
        survey.SurveyDetail.mailOnly = mailOnly;
        survey.SurveyDetail.ynEndNum = ynEndNum;
        survey.SurveyDetail.ynEndTime = ynEndTime;
        survey.SurveyDetail.endTime = endTime;
        survey.SurveyDetail.endNum = endNum;
        survey.SurveyDetail.showShareSurvey = showShareSurvey;
        survey.SurveyDetail.showAnswerDa = showAnswerDa;


        var surveyWidth = $("select[name='surveyWidth']").val();
        if (surveyWidth == null || surveyWidth == "") {
            surveyWidth = 900;
        }
        //data += "&surveyWidth=" + surveyWidth;
        survey.SurveyStyle.surveyWidth = surveyWidth;

        var surveyHeadBgColor = $("input[name='surveyHeadBgColor']").val();
        var surveyHeadBgImage = $("input[name='surveyHeadBgImage']").val();
        //表头边距
        var surveyHeadPaddingTop = $("input[name='surveyHeadPaddingTop']").val();
        var surveyHeadPaddingBottom = $("input[name='surveyHeadPaddingBottom']").val();
        var showSurveyHbgi = $("input[name='showSurveyHbgi']").prop("checked") ? 1 : 0;
        var surveyBtnBgColor = $("input[name='surveyBtnBgColor']").val();

        //data += "&surveyHeadBgColor=" + surveyHeadBgColor + "&surveyHeadBgImage=" + surveyHeadBgImage + "&surveyHeadPaddingTop=" + surveyHeadPaddingTop + "&surveyHeadPaddingBottom=" + surveyHeadPaddingBottom + "&showSurveyHbgi=" + showSurveyHbgi;
        //data += "&surveyBtnBgColor=" + surveyBtnBgColor;
        survey.SurveyStyle.surveyHeadBgColor = surveyHeadBgColor;
        survey.SurveyStyle.surveyHeadBgImage = surveyHeadBgImage;
        survey.SurveyStyle.surveyHeadPaddingTop = surveyHeadPaddingTop;
        survey.SurveyStyle.surveyHeadPaddingBottom = surveyHeadPaddingBottom;
        survey.SurveyStyle.showSurveyHbgi = showSurveyHbgi;
        survey.SurveyStyle.surveyBtnBgColor = surveyBtnBgColor;

        survey.SurveyStyle.surveyWidth = surveyWidth;
        survey.SurveyStyle.surveyWidth = surveyWidth;

        var surveyContentBgColorMiddle = $("input[name='surveyContentBgColorMiddle']").val();
        var surveyContentBgImageMiddle = $("input[name='surveyContentBgImageMiddle']").val();
        var showSurveyCbim = $("input[name='showSurveyCbim']").prop("checked") ? 1 : 0;
        //data += "&surveyContentBgColorMiddle=" + surveyContentBgColorMiddle + "&surveyContentBgImageMiddle=" + surveyContentBgImageMiddle + "&showSurveyCbim=" + showSurveyCbim;

        survey.SurveyStyle.surveyContentBgColorMiddle = surveyContentBgColorMiddle;
        survey.SurveyStyle.surveyContentBgImageMiddle = surveyContentBgImageMiddle;
        survey.SurveyStyle.showSurveyCbim = showSurveyCbim;

        var surveyLogoImage = $("input[name='surveyLogoImage']").val();
        var showSurveyLogo = $("input[name='showSurveyLogo']").prop("checked") ? 1 : 0;
        //data += "&surveyLogoImage=" + surveyLogoImage + "&showSurveyLogo=" + showSurveyLogo;
        survey.SurveyStyle.surveyLogoImage = surveyLogoImage;
        survey.SurveyStyle.showSurveyLogo = showSurveyLogo;

        //文本样式
        var questionTitleTextColor = $("input[name='questionTitleTextColor']").val();
        var questionOptionTextColor = $("input[name='questionOptionTextColor']").val();
        var surveyTitleTextColor = $("input[name='surveyTitleTextColor']").val();
        var surveyNoteTextColor = $("input[name='surveyNoteTextColor']").val();
        //data += "&questionTitleTextColor=" + questionTitleTextColor + "&questionOptionTextColor=" + questionOptionTextColor + "&surveyTitleTextColor=" + surveyTitleTextColor + "&surveyNoteTextColor=" + surveyNoteTextColor;

        survey.SurveyStyle.questionTitleTextColor = questionTitleTextColor;
        survey.SurveyStyle.questionOptionTextColor = questionOptionTextColor;
        survey.SurveyStyle.surveyTitleTextColor = surveyTitleTextColor;
        survey.SurveyStyle.surveyNoteTextColor = surveyNoteTextColor;

        //显示头序号,进度条
        var showTiNum = $("input[name='showTiNum']:checked")[0] ? "1" : "0";
        var showProgressbar = $("input[name='showProgressbar']:checked")[0] ? "1" : "0";
        //data += "&showTiNum=" + showTiNum + "&showProgressbar=" + showProgressbar;

        survey.SurveyStyle.showTiNum = showTiNum;
        survey.SurveyStyle.showProgressbar = showProgressbar;

        //表头文本显示
        var showSurTitle = $("input[name='showSurTitle']:checked")[0] ? "1" : "0";
        var showSurNote = $("input[name='showSurNote']:checked")[0] ? "1" : "0";
        //data += "&showSurTitle=" + showSurTitle + "&showSurNote=" + showSurNote;
        survey.SurveyStyle.showSurTitle = showSurTitle;
        survey.SurveyStyle.showSurNote = showSurNote;

        $.ajax({
            url: url,
            data: survey, //data,
            type: "post",
            success: function (msg) {
                //alert(msg);
                notify("保存成功！", 5000);
            }
        });
        return false;
    });


    var prevHost = $("#prevHost").val();
    //初始化弹出窗口参数
    var surveyStyleId = $("#surveyStyleId").val();
    if (surveyStyleId != "" && surveyStyleId != "0") {
        /** 背景样式 **/
        //surveyStyle.showBodyBi
        var showBodyBi = "@Model.SurveyStyle.ShowBodyBi";

        //surveyStyle.bodyBgColor
        var bodyBgColor = "@Model.SurveyStyle.BodyBgColor";
        var bodyBgColorObj = $("input[name='bodyBgColor']");
        bodyBgColorObj.val(bodyBgColor);
        var bodyBCThemeParamObj = bodyBgColorObj.parents(".theme_param");
        bodyBCThemeParamObj.find(".color_box").css({ "background-color": bodyBgColor });
        //$("#wrap").css({"background-color":bodyBgColor});
        $("body").css({ "background-color": bodyBgColor });

        //surveyStyle.bodyBgImage
        var bodyBgImage = "@Model.SurveyStyle.BodyBgImage";
        var bodyBgImageObj = $("input[name='bodyBgImage']");
        var bodyBIThemeParamObj = bodyBgImageObj.parents(".theme_param");
        bodyBgImageObj.val(bodyBgImage);
        bodyBIThemeParamObj.find(".previewImage").attr("src", prevHost + bodyBgImage);
        if (showBodyBi == 1) {
            //$("#wrap").css({"background-image":"url("+bodyBgImage+")"});
            $("body").css({ "background-image": "url(" + prevHost + bodyBgImage + ")" });
            $("input[name='showBodyBi']").prop("checked", true);
        }

        /** 表头样式 **/
        //surveyStyle.showBodyBi
        var showSurveyHbgi = "@Model.SurveyStyle.ShowSurveyHbgi";

        //surveyStyle.bodyBgColor
        var surveyHeadBgColor = "@Model.SurveyStyle.SurveyHeadBgColor";
        var surveyHeadBgColorObj = $("input[name='surveyHeadBgColor']");
        var surveyHBCThemeParamObj = surveyHeadBgColorObj.parents(".theme_param");
        surveyHeadBgColorObj.val(surveyHeadBgColor);
        surveyHBCThemeParamObj.find(".color_box").css({ "background-color": surveyHeadBgColor });
        $("#dwSurveyHeader").css({ "background-color": surveyHeadBgColor });

        //surveyStyle.bodyBgImage
        var surveyHeadBgImage = "@Model.SurveyStyle.SurveyHeadBgImage";
        var surveyHeadBgImageObj = $("input[name='surveyHeadBgImage']");
        var surveyHBIThemeParamObj = surveyHeadBgImageObj.parents(".theme_param");
        surveyHeadBgImageObj.val(surveyHeadBgImage);
        surveyHBIThemeParamObj.find(".previewImage").attr("src", prevHost + surveyHeadBgImage);
        if (showSurveyHbgi == 1) {
            $("#dwSurveyHeader").css({ "background-image": "url(" + prevHost + surveyHeadBgImage + ")" });
            $("input[name='showSurveyHbgi']").prop("checked", true);
        }


        //表头边距
        var surveyHeadPaddingTop = "@Model.SurveyStyle.SurveyHeadPaddingTop";
        var surveyHeadPaddingBottom = "@Model.SurveyStyle.SurveyHeadPaddingBottom";
        $("input[name='surveyHeadPaddingTop']").val(surveyHeadPaddingTop);
        $("input[name='surveyHeadPaddingBottom']").val(surveyHeadPaddingBottom);

        $("#dwSurveyHeader").css({ "padding-top": surveyHeadPaddingTop + "px" });
        $("#dwSurveyHeader").css({ "padding-bottom": surveyHeadPaddingBottom + "px" });

        /** 内容中间样式 **/
        //surveyStyle.showBodyBi
        var showSurveyCbim = "@Model.SurveyStyle.ShowSurveyCbim";

        //surveyStyle.bodyBgColor
        var surveyContentBgColorMiddle = "@Model.SurveyStyle.SurveyContentBgColorMiddle";
        var surveyContentBgColorMiddleObj = $("input[name='surveyContentBgColorMiddle']");
        var surveyCBCMThemeParamObj = surveyContentBgColorMiddleObj.parents(".theme_param");
        surveyContentBgColorMiddleObj.val(surveyContentBgColorMiddle);
        surveyCBCMThemeParamObj.find(".color_box").css({ "background-color": surveyContentBgColorMiddle });;
        $("#dwSurveyQuContentBg").css({ "background-color": surveyContentBgColorMiddle });

        //surveyStyle.bodyBgImage
        var surveyContentBgImageMiddle = "@Model.SurveyStyle.SurveyContentBgImageMiddle";
        var surveyContentBgImageMiddleObj = $("input[name='surveyContentBgImageMiddle']");
        var surveyCBIMThemeParamObj = surveyContentBgImageMiddleObj.parents(".theme_param");
        surveyContentBgImageMiddleObj.val(surveyContentBgImageMiddle);
        surveyCBIMThemeParamObj.find(".previewImage").attr("src", prevHost + surveyContentBgImageMiddle);
        if (showSurveyCbim == 1) {
            $("#dwSurveyQuContentBg").css({ "background-image": "url(" + prevHost + surveyContentBgImageMiddle + ")" });
            $("input[name='showSurveyCbim']").prop("checked", true);
        }

        /** 文本样式 **/
        var questionTitleTextColor = "@Model.SurveyStyle.QuestionTitleTextColor";
        var questionTitleTextColorObj = $("input[name='questionTitleTextColor']");
        var questionTTCThemeParamObj = questionTitleTextColorObj.parents(".theme_param");
        questionTitleTextColorObj.val(questionTitleTextColor);
        questionTTCThemeParamObj.find(".color_box").css({ "background-color": questionTitleTextColor });
        $(".quCoTitle").css({ "color": questionTitleTextColor });

        var questionOptionTextColor = "@Model.SurveyStyle.QuestionOptionTextColor";
        var questionOptionTextColorObj = $("input[name='questionOptionTextColor']");
        var questionOTCThemeParamObj = questionOptionTextColorObj.parents(".theme_param");
        questionOptionTextColorObj.val(questionOptionTextColor);
        questionOTCThemeParamObj.find(".color_box").css({ "background-color": questionOptionTextColor });
        $(".quCoOptionEdit").css({ "color": questionOptionTextColor });

        var surveyTitleTextColor = "@Model.SurveyStyle.QuestionTitleTextColor";
        var surveyTitleTextColorObj = $("input[name='surveyTitleTextColor']");
        var surveyTTCThemeParamObj = surveyTitleTextColorObj.parents(".theme_param");
        surveyTitleTextColorObj.val(surveyTitleTextColor);
        surveyTTCThemeParamObj.find(".color_box").css({ "background-color": surveyTitleTextColor });
        $("#dwSurveyTitle").css({ "color": surveyTitleTextColor });

        var surveyNoteTextColor = "@Model.SurveyStyle.SurveyNoteTextColor";
        var surveyNoteTextColorObj = $("input[name='surveyNoteTextColor']");
        var surveyNTCThemeParamObj = surveyNoteTextColorObj.parents(".theme_param");
        surveyNoteTextColorObj.val(surveyNoteTextColor);
        surveyNTCThemeParamObj.find(".color_box").css({ "background-color": surveyNoteTextColor });
        $("#dwSurveyNoteEdit").css({ "color": surveyNoteTextColor });

        var surveyWidth = "@Model.SurveyStyle.SurveyWidth";
        $("#dw_body_content").width(surveyWidth);
        $("select[name='surveyWidth']").val(surveyWidth);

        var surveyBtnBgColor = "@Model.SurveyStyle.SurveyBtnBgColor";
        if (surveyBtnBgColor !== "") {
            $("#dw_body_content .sbtn24").css({ "background": "none" });
            $("#dw_body_content .sbtn24,.progressbarDiv .ui-progressbar-value").css({ "background-color": surveyBtnBgColor });
            $(".progressbarDiv").css({ "border-color": surveyBtnBgColor });
            $(".progress-label ").css({ "color": surveyBtnBgColor });
            var surveyBtnBgColorObj = $("input[name='surveyBtnBgColor']");
            surveyBtnBgColorObj.val(surveyBtnBgColor);
            var btnBcThemeParamObj = surveyBtnBgColorObj.parents(".theme_param");
            btnBcThemeParamObj.find(".color_box").css({ "background-color": surveyBtnBgColor });
        }

        //显示序号及进度条
        var showTiNum = "@Model.SurveyStyle.ShowTiNum";
        var showProgressbar = "@Model.SurveyStyle.ShowProgressbar";
        if (showTiNum == 0) {
            $("input[name='showTiNum']").prop("checked", false);
            $(".quCoNum").hide();
        }
        if (showProgressbar == 0) {
            $("input[name='showProgressbar']").prop("checked", false);
            $("#resultProgressRoot").hide();
        }

        //表头文本显示
        var showSurTitle = "@Model.SurveyStyle.ShowSurTitle";
        var showSurNote = "@Model.SurveyStyle.ShowSurNote";
        if (showSurTitle == 0) {
            $("input[name='showSurTitle']").prop("checked", false);
            $("#dwSurveyTitle").hide();
        }
        if (showSurNote == 0) {
            $("input[name='showSurNote']").prop("checked", false);
            $("#dwSurveyNote").hide();
        }
    }

    $(".themenail").click(function () {
        var styleModelId = $(this).find(".styleModelId").val();
        //alert(styleModelId);
        //应用模板样式
        var url = '@Url.RouteUrl("styleget")/' + styleModelId;//"/content/design/my-survey-style!ajaxGetStyle.action";
        //var data = "id=" + styleModelId;
        $.ajax({
            url: url,
            //data: data,
            type: "post",
            success: function (msg) {
                //alert(msg);
                var surveyStyle = msg;//eval("(" + msg + ")");

                var showBodyBi = surveyStyle.ShowBodyBi;

                //surveyStyle.bodyBgColor
                var bodyBgColor = surveyStyle.BodyBgColor;
                var bodyBgColorObj = $("input[name='bodyBgColor']");
                bodyBgColorObj.val(bodyBgColor);
                var bodyBCThemeParamObj = bodyBgColorObj.parents(".theme_param");
                bodyBCThemeParamObj.find(".color_box").css({ "background-color": bodyBgColor });
                //$("#wrap").css({"background-color":bodyBgColor});
                $("body").css({ "background-color": bodyBgColor });

                //surveyStyle.bodyBgImage
                var bodyBgImage = surveyStyle.BodyBgImage;
                var bodyBgImageObj = $("input[name='bodyBgImage']");
                var bodyBIThemeParamObj = bodyBgImageObj.parents(".theme_param");
                bodyBgImageObj.val(bodyBgImage);
                bodyBIThemeParamObj.find(".previewImage").attr("src", "/content" + bodyBgImage);
                if (showBodyBi == 1) {
                    //$("#wrap").css({"background-image":"url("+bodyBgImage+")"});
                    $("body").css({ "background-image": "url(" + "/content" + bodyBgImage + ")" });
                    $("input[name='showBodyBi']").prop("checked", true);
                }

                /** 表头样式 **/
                //surveyStyle.showBodyBi
                var showSurveyHbgi = surveyStyle.ShowSurveyHbgi;

                //surveyStyle.bodyBgColor
                var surveyHeadBgColor = surveyStyle.SurveyHeadBgColor;
                var surveyHeadBgColorObj = $("input[name='surveyHeadBgColor']");
                var surveyHBCThemeParamObj = surveyHeadBgColorObj.parents(".theme_param");
                surveyHeadBgColorObj.val(surveyHeadBgColor);
                surveyHBCThemeParamObj.find(".color_box").css({ "background-color": surveyHeadBgColor });
                $("#dwSurveyHeader").css({ "background-color": surveyHeadBgColor });

                //surveyStyle.bodyBgImage
                var surveyHeadBgImage = surveyStyle.SurveyHeadBgImage;
                var surveyHeadBgImageObj = $("input[name='surveyHeadBgImage']");
                var surveyHBIThemeParamObj = surveyHeadBgImageObj.parents(".theme_param");
                surveyHeadBgImageObj.val(surveyHeadBgImage);
                surveyHBIThemeParamObj.find(".previewImage").attr("src", "/content" + surveyHeadBgImage);
                if (showSurveyHbgi == 1) {
                    $("#dwSurveyHeader").css({ "background-image": "url(" + "/content" + surveyHeadBgImage + ")" });
                    $("input[name='showSurveyHbgi']").prop("checked", true);
                }

                /** 内容中间样式 **/
                //surveyStyle.showBodyBi
                var showSurveyCbim = surveyStyle.ShowSurveyCbim;

                //surveyStyle.bodyBgColor
                var surveyContentBgColorMiddle = surveyStyle.SurveyContentBgColorMiddle;
                var surveyContentBgColorMiddleObj = $("input[name='surveyContentBgColorMiddle']");
                var surveyCBCMThemeParamObj = surveyContentBgColorMiddleObj.parents(".theme_param");
                surveyContentBgColorMiddleObj.val(surveyContentBgColorMiddle);
                surveyCBCMThemeParamObj.find(".color_box").css({ "background-color": surveyContentBgColorMiddle });;
                $("#dwSurveyQuContentBg").css({ "background-color": surveyContentBgColorMiddle });

                //surveyStyle.bodyBgImage
                var surveyContentBgImageMiddle = surveyStyle.SurveyContentBgImageMiddle;
                var surveyContentBgImageMiddleObj = $("input[name='surveyContentBgImageMiddle']");
                var surveyCBIMThemeParamObj = surveyContentBgImageMiddleObj.parents(".theme_param");
                surveyContentBgImageMiddleObj.val(surveyContentBgImageMiddle);
                surveyCBIMThemeParamObj.find(".previewImage").attr("src", "/content" + surveyContentBgImageMiddle);
                if (showSurveyCbim == 1) {
                    $("#dwSurveyQuContentBg").css({ "background-image": "url(" + "/content" + surveyContentBgImageMiddle + ")" });
                    $("input[name='showSurveyCbim']").prop("checked", true);
                }

                /** 文本样式 **/
                var questionTitleTextColor = surveyStyle.QuestionTitleTextColor;
                var questionTitleTextColorObj = $("input[name='questionTitleTextColor']");
                var questionTTCThemeParamObj = questionTitleTextColorObj.parents(".theme_param");
                questionTitleTextColorObj.val(questionTitleTextColor);
                questionTTCThemeParamObj.find(".color_box").css({ "background-color": questionTitleTextColor });
                $(".quCoTitle").css({ "color": questionTitleTextColor });

                var questionOptionTextColor = surveyStyle.QuestionOptionTextColor;
                var questionOptionTextColorObj = $("input[name='questionOptionTextColor']");
                var questionOTCThemeParamObj = questionOptionTextColorObj.parents(".theme_param");
                questionOptionTextColorObj.val(questionOptionTextColor);
                questionOTCThemeParamObj.find(".color_box").css({ "background-color": questionOptionTextColor });
                $(".quCoOptionEdit").css({ "color": questionOptionTextColor });

                var surveyTitleTextColor = surveyStyle.SurveyTitleTextColor;
                var surveyTitleTextColorObj = $("input[name='surveyTitleTextColor']");
                var surveyTTCThemeParamObj = surveyTitleTextColorObj.parents(".theme_param");
                surveyTitleTextColorObj.val(surveyTitleTextColor);
                surveyTTCThemeParamObj.find(".color_box").css({ "background-color": surveyTitleTextColor });
                $("#dwSurveyTitle").css({ "color": surveyTitleTextColor });

                var surveyNoteTextColor = surveyStyle.SurveyNoteTextColor;
                var surveyNoteTextColorObj = $("input[name='surveyNoteTextColor']");
                var surveyNTCThemeParamObj = surveyNoteTextColorObj.parents(".theme_param");
                surveyNoteTextColorObj.val(surveyNoteTextColor);
                surveyNTCThemeParamObj.find(".color_box").css({ "background-color": surveyNoteTextColor });
                $("#dwSurveyNoteEdit").css({ "color": surveyNoteTextColor });

                var surveyBtnBgColor = surveyStyle.SurveyBtnBgColor;
                if (surveyBtnBgColor !== "") {
                    $("#dw_body_content .sbtn24").css({ "background": "none" });
                    $("#dw_body_content .sbtn24,.progressbarDiv .ui-progressbar-value").css({ "background-color": surveyBtnBgColor });
                    $(".progressbarDiv").css({ "border-color": surveyBtnBgColor });
                    $(".progress-label ").css({ "color": surveyBtnBgColor });
                    var surveyBtnBgColorObj = $("input[name='surveyBtnBgColor']");
                    surveyBtnBgColorObj.val(surveyBtnBgColor);
                    var btnBcThemeParamObj = surveyBtnBgColorObj.parents(".theme_param");
                    btnBcThemeParamObj.find(".color_box").css({ "background-color": surveyBtnBgColor });
                }

            }
        });
        return false;
    });

    $("input[name='showTiNum']").change(function () {
        if ($(this).prop("checked")) {
            //$("#resultProgressRoot").show();
            $(".quCoNum").show();
        } else {
            $(".quCoNum").hide();
            //$("#resultProgressRoot").hide();
        }
        return false;
    });

    $("input[name='showProgressbar']").change(function () {
        if ($(this).prop("checked")) {
            $("#resultProgressRoot").show();
        } else {
            $("#resultProgressRoot").hide();
        }
        return false;
    });

    //问卷标题
    $("input[name='showSurTitle']").change(function () {
        if ($(this).prop("checked")) {
            $("#dwSurveyTitle").show();
        } else {
            $("#dwSurveyTitle").hide();
        }
        return false;
    });

    $("input[name='showSurNote']").change(function () {
        if ($(this).prop("checked")) {
            $("#dwSurveyNote").show();
        } else {
            $("#dwSurveyNote").hide();
        }
        return false;
    });

    $("input[name='showSurHead']").change(function () {
        if ($(this).prop("checked")) {
            $("#dwSurveyHeader").show();
        } else {
            $("#dwSurveyHeader").hide();
        }
        return false;
    });

    if (@Model.SurveyDetail.Effective > 1) {
    $("input[name='effective']").attr("checked", true);
} else {
    $("input[name='effective']").attr("checked", false);
}
$("input[name='effectiveIp'][value='@Model.SurveyDetail.EffectiveIp']").attr("checked", true);
$("input[name='rule'][value='@Model.SurveyDetail.Rule']").attr("checked", true);
$("input[name='ruleCode']").val("@Model.SurveyDetail.RuleCode");
$("input[name='refresh'][value='@Model.SurveyDetail.Refresh']").attr("checked", true);
$("input[name='mailOnly'][value='@Model.SurveyDetail.MailOnly']").attr("checked", true);
$("input[name='ynEndNum'][value='@Model.SurveyDetail.YnEndNum']").attr("checked", true);
$("input[name='endNum']").val("@Model.SurveyDetail.EndNum");
$("input[name='ynEndTime'][value='@Model.SurveyDetail.YnEndTime']").attr("checked", true);
$("input[name='endTime']").val("@Model.SurveyDetail.EndTime");
$("input[name='showShareSurvey'][value='@Model.SurveyDetail.ShowShareSurvey']").attr("checked", true);
$("input[name='showAnswerDa'][value='@Model.SurveyDetail.ShowAnswerDa']").attr("checked", true);


        });