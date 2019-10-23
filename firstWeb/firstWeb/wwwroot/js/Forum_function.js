function getItemReply(page, Comment_id) {
    var result;
    $.ajax({
        url: "/Comment/getreply?page=" + page + "&comment_id=" + Comment_id,
        type: "GET",
        async: false,
        success: function (res) {
            result = res;
        },
        error: function (res) {
            alert("发生错误，请重新刷新下页面");
        }

    })
    return result;
};
function RenderReplys(res, node) {
    if (res.code == "200") {
        node.html('');
        $.each(res.data.replys, function (index, value) {

            var content = '<li class="clearfix" id="' + value.id + '" author_NickName="' + value.reply_nickname + '" author_ID="' + value.userid + '">' + '<div class="reply_list_img">' +
                '<img src="/' + value.reply_headeraddress + '" height="35" width="35" />' +
                '</div>' +
                '<div class="reply_list_con">' +
                '<div class="auth_message">' +
                '<a>' + value.reply_nickname + '</a>' +
                '<span>回复</span>' +
                '<span> <a href="#">' + value.super_evaluate_name + '</a></span>' +
                '<span>于' + value.add_time + '</span>' +
                '</div>' +
                '<div class="comment_list_txt">' +
                value.reply_content +
                '</div>' +
                '<div class="reply_list_footer pull-right">' +
                '<a href="javascript:;"><span>举报&nbsp;&nbsp;&nbsp;</span></a>' +
                '<a href="javascript:;" class="reply"><span>回复</span></a>' +
                '</div>' +
                '</div>' +
                '</li>';

            node.append(content);
        })

    } else {
        alert(res.message);
    }
}
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}