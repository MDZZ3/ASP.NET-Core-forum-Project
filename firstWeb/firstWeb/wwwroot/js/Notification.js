window.onload = function () {
    //获取url的参数
    var activeparam = getQueryString("active");
     //转换成数字
    var number = parseInt(activeparam) - 1201;
    //给对应的li加上active
    var li_active = $(".Menu>.nav").children("li").eq(number);

    li_active.addClass("active");

    var url;
    if (li_active.hasClass("comment_notice")) {
        url = "/Notification/comment_notice?page=1";
    }
    else if (li_active.hasClass("concern_notice")) {
        url = "/Notification/concern_notice?page=1";
    }
    else
    {
        url = "/Notification/System_notice";
    }
    $.get(url, function (data) {
        $(".Container_Content").html(data);
    })

    $(".comment_notice").click(function () {
        if (!$(this).hasClass("active")) {
            $(this).addClass("active");
            li_active.removeClass("active");
            li_active = $(this);

            $.get("/Notification/comment_notice?page=1", function (data) {
                $(".Container_Content").html(data);
            });
        }
    });
    $(".concern_notice").click(function () {
        if (!$(this).hasClass("active")) {
            $(this).addClass("active");
            li_active.removeClass("active");
            li_active = $(this);

            $.get("/Notification/concern_notice?page=1", function (data) {
                $(".Container_Content").html(data);
            });
        }
    });

    $(".System_notice").click(function () {
        if (!$(this).hasClass("active")) {
            $(this).addClass("active");
            li_active.removeClass("active");
            li_active = $(this);

            $.get("/Notification/System_notice", function (data) {
                $(".Container_Content").html(data);
            });
        }
    });
}