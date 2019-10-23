window.onload=function() {
    //var replynodes = $(".comment_list_footer");
    //console.log(replynodes[0]);
    //console.log($(replynodes[0]).children().eq(1));

    //for (i = 0; i < replynodes.length; i++) {
    //    $(replynodes[i]).children().eq(1).click(function () {
    //        //$(replynodes[i]).next().slideToggle();
    //        alert("则无");
    //    });
    //}
    var E = window.wangEditor;

    var editor = new E("#editor");
     
    editor.customConfig.withCredentials = true;
    editor.customConfig.menus = [
        "head",
        "bold",
        "fontSize",
        "fontName",
        "italic",
        "underline",
        "strikeThrough",
        "foreColor",
        "backColor",
        "justify",
        "code"
    ];
    editor.create();
    //console.log(editor);

    //给最后一个评价去掉底框
    var comment_list = document.getElementById("comment_list");
    if (comment_list != null) {
        $("#comment_list").children("li:last-child").addClass("no-border");
    }

    $(".Comment_Div").on("click", ".commentBtn_Div>.commentBtn", function () {
        var editor_Content = editor.txt.text()
        //这里条件限制的很差，如果这样写的话，用户输入空格也可以发送，只插入图片的话不能发送
        if (editor_Content == "") {
            alert("内容不能为空!");
        } else {
            editor_Content = editor.txt.html();
            var forum_id = $(this).parents(".theme").attr("id");
            $.ajax({
                url: "/Comment/AddComment",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({ forum_id: forum_id, content: editor_Content }),
                success: function (res) {
                    if (res.code == "200") {
                        alert("添加成功");
                        window.location.reload();
                    } else {
                        alert(res.message);
                    }
                },
                error: function (res) {
                    alert(res);
                }
            })
        }

    });

    $("#comment_list").on("click", "li>.comment_list_footer>.reply_div", function () {

        
        $(this).parent().next().slideToggle();
        if ($(this).html().startsWith("回复")) {
            var Comment_id = $(this).parent().parent().attr("id");
            var _this = $(this);
            var reply_list = _this.parent().next().children(".reply_list");
            if (reply_list.attr("isLoad") != "true") {
                var res = getItemReply(1, Comment_id);

                RenderReplys(res, reply_list);

                res.data.pagesum = res.data.pagesum == "0" ? "1" : res.data.pagesum;

                reply_list.next().prepend('<div class="paging"><div id="page_' + Comment_id + '"></div></div>' +
                    '<script>' +
                    '$("#page_' + Comment_id + '").paging({' +
                    'nowPage:' + res.data.nowpage + ',' +
                    'pageNum:' + res.data.pagesum + ',' +
                    'callback:function(num,node){' +
                    'var result2=getItemReply(num,"' +Comment_id + '");' +
                    'RenderReplys(result2,node.element.parents(".reply_text_div").prev())' +
                    '}})' +
                    '</script>');
                reply_list.attr("isLoad", true);

            }
           
            $(this).html("收起回复");
            $(this).parent().children().eq(2).hide(300);
            
            
        } else {
            $(this).html("回复");
            $(this).parent().children().eq(2).show(300);
        }
    });

    
    $(".reply_list").on("click", "li>.reply_list_con>.reply_list_footer>.reply", function () {
        var super_Evaluate = $(this).parent().parent().parent();

        var super_Evaluate_ID = super_Evaluate.attr("author_ID");

        var super_Evaluate_Name = super_Evaluate.attr("author_nickname");

        //找到编辑文本窗口对象
        var reply_text = $(super_Evaluate).parent().next().children().eq(2);

        //附加属性到编辑窗口的div上
        //console.log($(super_Evaluate_ID).parent().next().next().next().children(":first"));
        //$(super_Evaluate_ID).parent().next().next().next().attr({ "super_Evaluate_ID": super_Evaluate_ID_value, "Original_Evaluate_ID": Original_Evaluate_ID_value });
        $(reply_text).attr("placeholder", "回复" + super_Evaluate_Name + ":");
        $(reply_text).attr("super_Evaluate_ID", super_Evaluate_ID);
        //显示"取消回复"按钮
        console.log($(super_Evaluate).parent().next().children().eq(2));
        $(super_Evaluate).parent().next().children().eq(4).show();
    })

    //给取消回复按钮设置监控
    $(".reply_Contents").on("click", ".reply_text_div>.remove_reply", function () {

        $(this).hide();
        $(this).prevAll(".reply_text").removeAttr("super_evaluate_id");
        $(this).prevAll(".reply_text").removeAttr("placeholder");
    })

    //给回复按钮设置监控
    $(".reply_Contents").on("click", ".reply_text_div>.replyBtn", function () {         
        var reply_Txt = $(this).prev();
        var super_Evaluate_ID = $(reply_Txt).attr("super_Evaluate_ID");
        var Original_Evaluate_ID = $(reply_Txt).attr("original_evaluate_id");

        var Original_forum_ID = $(".theme").attr("id");

        var Content = $(reply_Txt).val();

        $.ajax({
            type: "POST",
            url: "/Comment/Addreply",
            contentType: "application/json",
            data: JSON.stringify({ super_Evaluate_ID: super_Evaluate_ID, Original_Evaluate_ID: Original_Evaluate_ID, Content: Content, Original_forum_ID:Original_forum_ID }),
            success: function (res) {
                if (res.code == "200") {
                    $(".reply_list").append('<li class="clearfix no-border" id="' + res.data.id + '" username="' + res.data.nickname + '">' +
                        '<div class="reply_list_img">' +
                        ' <img src="/' + res.data.headaddress+'" height="35" width="35" />' +
                        '</div>' +
                        '<div class="reply_list_con">' +
                        '<div class="auth_message">' +
                        '<a href="#">' + res.data.nickname + '</a>' +
                        '<span>回复<a href="#">' + res.data.super_evaluate_name + '</a></span>' +
                        '<span>于' + res.data.create_time + '</span></div>' +
                        '<div class="comment_list_txt">' +
                        res.data.content +
                        '</div>' +
                        '<div class="reply_list_footer pull-right">' +
                        '<a href="javascript:;"><span>举报&nbsp;&nbsp;&nbsp;</span></a>' +
                        '<a href="javascript:;" class="reply"><span >回复</span></a>' +
                        '</div></div>' +
                        '</li>');
                    $(reply_Txt).val('');
                    //触发取消回复
                    $(reply_Txt).next().next().trigger("click");
                    
                } else {
                    alert(res.message);
                }
            },
            error: function (res) {
                alert("出现错误，请重新操作");
            }
        })        
    })

    $(".Userinfo_Div").on("click", ".Userinfo_Div_Btn>.conternBtn", function () {
        var auth_id = $(this).parents(".Userinfo_Div").attr("auth_id");
        var _this = this;
        $.ajax({
            url: "/Concern/Create",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ auth_ID: auth_id }),
            success: function (res) {
                //如果是undefined的话，证明用户是未登录的，res是一个登录的页面代码(302success不拦截，浏览器
                //自动跳转并让ajax拦截跳转之后的登录代码)
                if (res.code == undefined) {
                    alert("登录之后才能关注哦");
                }
                else if (res.code == "200") {
                    $(_this).html("已关注");
                    $(_this).removeClass("conternBtn");
                    $(_this).addClass("unconternBtn attention");
                }
                else {
                    alert(res.message);
                }

            },
            error: function (res) {
                alert("发生错误，请联系管理员");
            }
        })
    });

    $(".Userinfo_Div").on("click", ".Userinfo_Div_Btn>.unconternBtn", function () {
        var auth_Id = $(this).parents(".Userinfo_Div").attr("auth_id");
        var _this = this;
        console.log(auth_Id);
        $.ajax({
            url: "/Concern/destroy",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ auth_ID: auth_Id }),
            success: function (res) {
                if (res.code == undefined) {
                    alert("登录之后才能取关哦");
                }
                else if (res.code == "200")
                {
                    $(_this).html("关注");
                    $(_this).removeClass("unconternBtn attention");
                    $(_this).addClass("conternBtn");
                }
            },
            error: function (res) {
                alert("发生错误，请联系管理员");
            }
        })
    })




}