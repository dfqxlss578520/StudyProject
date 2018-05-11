$('a[data-toggle=tooltip]').tooltip({
    animation: true,
    delay: 100,
    container: "body",
    trigger: 'hover'
});

//delete
$(".deleteSurvey").click(function () {
    if (confirm("确认删除吗？")) {
        return true;
    }
    return false;
});

$(".copySurvey").click(function () {
    if (confirm("确认复制吗？")) {
        return true;
    }
    return false;
});

$("#surveyAdd-a").click(function () {
    var surveyId = $(this).parents("tr").find("input[name='surveyId']").val();
    var titleValue = $(this).parents("tr").find(".titleTag").text();
    var targetUrl = $(this).attr('href');

    $("body").append("<div id=\"myDialogRoot\"><div class='dialogMessage' style='padding-top:40px;margin-left:20px;padding-bottom:0px;'>" +
        "<div>问卷标题：<input id='surTitleTemp' type='text' style='padding:5px;width:320px;color:rgb(14, 136, 158);' value=''></div></div></div>");

    var myDialog = $("#myDialogRoot").dialog({
        width: 500,
        height: 220,
        autoOpen: true,
        modal: true,
        position: ["center", "center"],
        title: "新建问卷、表单",
        resizable: false,
        draggable: false,
        closeOnEscape: false,
        show: { effect: "blind", direction: "up", duration: 500 },
        hide: { effect: "blind", direction: "left", duration: 200 },
        buttons: {
            "OK": {
                text: "确认新建",
                addClass: 'dialogMessageButton dialogBtn1',
                click: function () {
                    //执行发布
                    var surveyName = $("#surTitleTemp").val();
                    if (surveyName != null && surveyName.length > 0) {
                        surveyName = optionValue = escape(encodeURIComponent(surveyName));
                        var params = "surveyName=" + surveyName;
                        window.location.href = targetUrl + "?" + params;
                    }
                }
            },
            "CENCEL": {
                text: "取消",
                addClass: "dialogBtn1 dialogBtn1Cencel",
                click: function () {
                    $(this).dialog("close");
                }
            }
        },
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
            $("#surTitleTemp").val(titleValue + "");
        },
        close: function (event, ui) {
            $("#myDialogRoot").remove();
        }
    });
    return false;
});