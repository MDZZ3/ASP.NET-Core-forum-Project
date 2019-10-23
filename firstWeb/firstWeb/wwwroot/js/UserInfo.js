
window.onload = function () {
    //获取url的参数
    var activeparam = getQueryString("active");
    //转换成数字
    var number = parseInt(activeparam) - 1102;
    //给对应的li加上active
    var li_active = $(".Menu>.nav").children("li").eq(number);

    li_active.addClass("active");

    var url;
    if (li_active.hasClass("User_forum")) {
        url = "/UserInfo/User_forums";
    } else {
        url = "/UserInfo/User_Comments";
    }

    $.get(url, function (data) {
        $(".Container_Content").html(data)
    });
    
    $(".User_forum").click(function () {
        if (!$(this).hasClass("active")) {
            $(this).addClass("active");
            li_active.removeClass("active");
            li_active = $(this);

            $.get("/UserInfo/User_forums", function (data) {
                $(".Container_Content").html(data);
            });
        }
    });

    $(".User_Comment").click(function () {
        if (!$(this).hasClass("active")) {
            $(this).addClass("active");
            li_active.removeClass("active");
            li_active = $(this);

            $.get("/UserInfo/User_Comments", function (data) {
                $(".Container_Content").html(data);
            });
        }
    })

   
}