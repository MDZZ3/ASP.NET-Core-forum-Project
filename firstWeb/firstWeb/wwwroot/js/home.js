window.onload = function () {
    var login = document.getElementById("LoginBtn");
    login.onclick = function () {
        $.ajax({
            url: "/Account/Login",
            type: "GET",
            success: function (data) {
               
            },
            error: function (data) {
                window.alert("登录按钮发生错误");
            }
        })
    }
}