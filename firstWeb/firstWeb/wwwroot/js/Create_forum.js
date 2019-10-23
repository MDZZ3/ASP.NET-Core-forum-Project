
window.onload = function () {
    var E = window.wangEditor;
    var editor = new E("#editor");
    //设置上传的服务器地址
    editor.customConfig.uploadImgServer = "/upload";
    //设置上传超时时间
    editor.customConfig.uploadImgTimeout = 500000;
    //设置可以携带Cookie
    editor.customConfig.withCredentials = true;
    //设置一次最多可以上传5张
    editor.customConfig.uploadImgMaxLength = 5;

    editor.create();
   
    var createBtn = document.getElementById("Create")
    
    createBtn.onclick = function () {
        if (!$("#forum_Title_Div").hasClass("has-success")) {
            alert("题目不符合标准！");
        }
        var Title = $("#forum_Title").val();
        //获取类型的值
        var selectValue = $("#categoryItem select").val();
        //获取富文本的内容
        var Content = editor.txt.html();

        $.ajax({
            url: "/Forum/Create_forum",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({ forum_Title: Title, forum_Content: Content, forum_Category: selectValue }),
            success: function (res) {
                alert(res.message);
                window.location.href = "/home/index";
            },
            error: function (res) {
                alert(res.Message);
            }
        }); 

        return false;
    }
    $("#forum_Title").blur(function () {
        var number = $(this).val();
        if (number.length > 100 || number<=0) {
            //如果length大于10的话，证明不能通过验证，添加glyphicon-remove
            $("#forum_Title_Div").addClass("has-error has-feedback");           
            $("#forum_Title").next().addClass("glyphicon-remove");

            if (number.length > 100) {
                //在验证的Div中添加错误信息
                $("#Verification_Title span").html('<span class="glyphicon glyphicon-remove"></span>题目的字数超过了100！');
            } else {
                $("#Verification_Title span").html('<span class="glyphicon glyphicon-remove"></span>题目是必填项');
            }
            
        } else {
            //进入这里证明通过了验证，添加glyphicon-ok
            $("#forum_Title_Div").addClass("has-success has-feedback");     
            $("#forum_Title").next().addClass("glyphicon-ok");
        }
    });

    $("#forum_Title").focus(function () {
        //下面操作主要是消去glyphicon-remove或者glyphicon-ok和消去验证div中的文字
        $("#forum_Title_Div").removeClass("has-error has-feedback");
        //消去验证div中的文字
        $("#Verification_Title span").html('');


        //input获取到焦点的时候，检测input下面的span有哪一个类，就删除那个类（消去glyphicon-remove或者glyphicon-ok）
        if ($("#forum_Title").next().hasClass("glyphicon-remove")) {
            $("#forum_Title").next().removeClass("glyphicon-remove");
        } else {
            $("#forum_Title").next().removeClass("glyphicon-ok");
        }
    });
}