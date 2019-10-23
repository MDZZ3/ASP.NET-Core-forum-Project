    window.onload= function () {
        var usernamereg = /^1[3|4|5|7|8][0-9]{9}$/;
        var passwordreg  = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,}$/;

        var LoginBtn = document.getElementById("btnLogin");
        var RegisterBtn = document.getElementById("btnRegister");
        var returnUrl = document.getElementById("returnUrl").value;

    //获取登录注册tab
    var TabBtn = document.getElementsByClassName("tab");

    //获取登录面板
    var Login_panel = document.getElementById("managers_Login");

    //获取注册面板
    var Register_panel = document.getElementById("managers_register");

    //显示登录
    TabBtn[0].onclick = function () {
        Login_panel.style.display = "block";
        Register_panel.style.display = "none";

        if (!this.classList.contains("active")) {
            $("#managers_register>.text-center>.username_Text input").val("");

            $("#managers_register>.text-center>.password_Text input").val("");
            $("#managers_register>.text-center>.Code_Text input").val("");
            this.classList.add("active");
        }

        TabBtn[1].classList.remove("active");
    }
    //显示注册
    TabBtn[1].onclick = function () {
        Register_panel.style.display = "block";
        Login_panel.style.display = "none";
        if (!this.classList.contains("active"))
        {
            $("#managers_Login>.text-center>.username_Text input").val("");

            $("#managers_Login>.text-center>.password_Text input").val("");
            
            this.classList.add("active");
          
        }
        TabBtn[0].classList.remove("active");
    }



    LoginBtn.onclick=function () {

        //获取输入框的账号
        var username = document.getElementsByName("username")[0].value;
        //获取输入框的密码
        var password = document.getElementsByName("password")[0].value;

        var flag = Verification(username, usernamereg);
        var flag2 = Verification(password, passwordreg);

        if (username == "" || password == "") {
            alert("账号或密码不能为空");
        } else if (!flag) {
            alert("账号必须是电话");
        } else if (!flag2) {
            alert("密码长度要大于6位，由数字和字母组成");
        }
        else {
           var user = { UserName: username, PassWord: password, returnUri:returnUrl};

           $.ajax({
               url:"/Account/Login",
               type: "Post",
               data: JSON.stringify(user),
               contentType:"application/json",
               success: function (res) {
                   if (res.state == "302") {
                       location.href = res.location;
                   } else {
                       alert(res.message);
                   }
               }
           });
           return false;
       }
        };
        RegisterBtn.onclick = function () {
            var username = document.getElementsByName("username")[1].value;

            var password = document.getElementsByName("password")[1].value;

            var code = document.getElementsByName("code")[0].value;

            if (username == "" || password == "" || code == "") {
                alert("有项为空，请重新填写");
            } else if (!Verification(username, usernamereg)) {
                alert("账号必须是电话");
            } else if (!Verification(password, passwordreg)) {
                alert("密码长度要大于6位，由数字和字母组成");
            }else {
                $.ajax({
                    url: "/Account/Register",
                    type: "POST",
                    contentType: "application/json",
                    data: JSON.stringify({ username: username, password: password, code: code, returnUrl: returnUrl }),
                    success: function (res) {
                        if (res.state == "200") {
                            TabBtn[0].onclick();
                        }
                        else if (res.state == "403") {
                            alert("电话号码已被注册");
                        } else {
                            alert(res.Message);
                        }
                    },
                    error: function (res) {
                        alert("发生错误，请重新刷新页面");
                    }
                })
            }

          
        }


    /**
     *
     * 用来验证输入的字符串是否符合要求
     * @param {any} str    要验证（匹配）的字符串
     * @param {any} regex  正则表达式的对象
     */
    function Verification(str, regex) {
        return regex.test(str);
    }

}

