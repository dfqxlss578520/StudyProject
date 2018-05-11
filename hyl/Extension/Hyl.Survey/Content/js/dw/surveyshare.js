$(function () {
    $(".collectTab").click(function () {
        $(this).siblings(".active").removeClass("active");
        $(this).addClass("active");

        $(".surveyCollectMiddleContent").each(function (index, item) {
            if (!$(this).hasClass("ui-helper-hidden")) {
                $(this).addClass("ui-helper-hidden");
            }
        });
        $("#" + $(this).attr("data-divid")).removeClass("ui-helper-hidden");
    });
    updateCode();
});

var clipboard = new Clipboard('.clipbtn');
clipboard.on('success', function (e) {
    console.info('Text:', e.text);
    notify("复制成功", 2000);
    e.clearSelection();
});
var clipboard2 = new Clipboard('.compTextareaCodeBtn');
clipboard2.on('success', function (e) {
    console.info('Text:', e.text);
    notify("复制成功", 2000);
    e.clearSelection();
});
function notify(msg, delayHid) {
    $(".notification").remove();
    if (delayHid == null) {
        delayHid = 5000;
    }
    $("<div>").appendTo(document.body).text(msg).addClass("notification ui-state-default ui-corner-bottom").position({
        my: "center top",
        at: "center top",
        of: window
    }).show({
        effect: "blind"
    }).delay(delayHid).hide();
}
$("input[name='floatPos']").change(function () {
    var thVal = $(this).val();
    if (thVal == "left") {
        $("#webSiteFixedLeft").show();
        $("#webSiteFixedRight").hide();
    } else if (thVal == "right") {
        $("#webSiteFixedLeft").hide();
        $("#webSiteFixedRight").show();
    }
    var docHeight = $(window).height();
    var bottomHeight = $("#showPosition").val();
    var posHeight = docHeight - bottomHeight;
    $(".websiteFixed").offset({ top: posHeight });
    updateCode();
});

$("#site_show_top_slider").slider({
    orientation: "vertical",
    range: "min",
    min: 80,
    max: 150,
    value: 20,
    slide: function (event, ui) {
        var docHeight = $(window).height();
        var bottomHeight = (docHeight * ui.value * 0.01);
        $("#showPosition").val(bottomHeight);
        var posHeight = docHeight - bottomHeight;
        $(".websiteFixed").offset({ top: posHeight });
        updateCode();
    }
});

//色彩选择器
$("#site_color_box").colpick({
    flat: true,
    layout: 'hex',
    submit: false,
    colorScheme: "light",
    onChange: function (hsb, hex, rgb, el) {
        var colorbox = $("input[name='colorbox']:checked").val();
        if (colorbox == "2") {
            $(".websiteAId").css('color', '#' + hex);
        } else {
            $(".websiteAId").css('background-color', '#' + hex);
        }
        updateCode();
    },
    onSubmit: function (hsb, hex, rgb, el) {
    }
});
//同步更新代码
function updateCode() {
    //webSiteLeftCode
    var thVal = $("input[name='floatPos']:checked").val();
    var copyCode = "";
    if (thVal === "left") {
        copyCode = $("#webSiteLeftCode").html();
    } else if (thVal === "right") {
        copyCode = $("#webSiteRightCode").html();
    }
    $("#compCodeText").text("<!-- 泓源林 BEGIN -->" + copyCode.trim().replace(/\s+/g, "") + "<!-- 泓源林 END -->");
    //bindClipBoard("compCodeText", "compTextareaCodeBtn", "compCopyMsg");
}

