using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using firstWeb.Domain.Even;
using firstWeb.Domain.Model;
using firstWeb.Domain.Services.accountService;
using firstWeb.Domain.Services.comment;
using firstWeb.Domain.Services.forum;
using firstWeb.Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace firstWeb.Controllers
{
    public class CommentController : BaseController
    {

        private readonly ICommentService _commentService;

        private readonly IForumService _forumSerivce;

        private readonly IAccountService _accountService;

        private readonly EvenHandlerContainer _container;

        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService comment,IAccountService account,
            ILogger<CommentController> logger,
            EvenHandlerContainer container,
            IForumService forum,
            IConfiguration configuration):base(configuration)
        {
            _commentService = comment;
            _accountService = account;
            _logger = logger;
            _container = container;
            _forumSerivce = forum;
        }

        [HttpPost]
        [Route("Comment/AddComment")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] JObject jObject)
        {
            string forum_id = jObject["forum_id"].ToString();
            string Content = jObject["content"].ToString();
            if (string.IsNullOrEmpty(forum_id) || string.IsNullOrWhiteSpace(Content))
            {
                return Json(new { code = "400",message="格式错误" });
            }
            //获取用户ID
            string phone = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.OtherPhone).Value;
            string user_id = _accountService.GetUserID(phone);

            Forum forum = null;
            if(!await _forumSerivce.isExistAsync(forum_id))
            {
                return Json(new { code = "400", message = "帖子ID存在问题" });
            }
            forum = await _forumSerivce.GetForumAsync(forum_id);
          

            int commentcount = _commentService.GetCommentCount(forum_id);

            //获取每页评价的数量
            int pagecount =int.Parse( _configuration.GetSection("page_Setup:page_comment_count").Value);

            //设置评价所在页
            int Locationpage;
            //如果帖子的评价数量小于规定的数量
            if (commentcount < pagecount)
            {  
                //设置所在页为1;
                Locationpage =1;
            }
            else
            {
                //查看规定的数量(pagecount)能否除尽帖子数量(commentcount),如果除尽，证明下面将要添加的评论会在下一页显示
                //所以所在页要在相除的数字基础上+1，如果除不尽，证明下面将要添加的评论还在相除的数字页面上
               // Locationpage = commentcount % pagecount == 0 ? (commentcount / pagecount) + 1 : commentcount / pagecount;
               //下面是上面的简化
                Locationpage = (commentcount / pagecount) + 1;
            }

            Comment comment = new Comment();
            comment.forumID = forum_id;
            comment.Content = Content;
            comment.ID = Guid.NewGuid().ToString("N");
            comment.UserID = user_id;
            comment.Create_Time = DateTime.Now;
            comment.LocationPage = Locationpage;

           

            try
            {
               await  _commentService.AddCommentAsync(comment);
              //发布AddComment事件，使帖子的评价个数+1;
              _container.Publish("AddComment", new CommentSubmitEven() { category=1, forumID = forum_id, publisherId = user_id, SubscriberId = forum.UserId, Comment_Id = comment.ID });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Json(new { code = "500", message = "发生错误" });
            }
            return Json(new { code = "200",message="添加成功" });
        }
        

        [HttpPost]
        [Route("Comment/Addreply")]
        [Authorize]
        public async Task<IActionResult> Addreply([FromBody]replyViewModel replyViewModel)
        {
            if (string.IsNullOrEmpty(replyViewModel.Original_Evaluate_ID))
            {
                return Json(new { code = "400", message = "找不到原评论ID" });
            }
            if (string.IsNullOrWhiteSpace(replyViewModel.Content) || string.IsNullOrEmpty(replyViewModel.Content))
            {
                return Json(new { code = "400", message = "内容不能为空！" });
            }

            string super_Evaluate_name = null;
            if (replyViewModel.super_Evaluate_ID != null)
            {
                super_Evaluate_name = _accountService.GetNickName(replyViewModel.super_Evaluate_ID);
                if (super_Evaluate_name == null)
                {
                    return Json(new { code = "400", message = "被回复人ID错误" });
                }              
            }
           

            //获取用户ID
            string phone = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.OtherPhone).Value;
            string user_id = _accountService.GetUserID(phone);
            ////获取名字
            //string user_name = _accountService.GetNickName(user_id);

            User user = _accountService.GetAcccount(user_id);

            //防止自己回复自己
            replyViewModel.super_Evaluate_ID = replyViewModel.super_Evaluate_ID == user.ID ? null : replyViewModel.super_Evaluate_ID;
            super_Evaluate_name = super_Evaluate_name == user.Nickname ? null : super_Evaluate_name;

            ////获取原评论
            //Comment comment = await _commentService.GetCommentAsync(replyViewModel.Original_Evaluate_ID);
            //if (comment == null)
            //{
            //    return Json(new { code = "400", message = "原评论ID出现问题" });
            //}
            ////使评论+1（这里可以优化成事件）
            //comment.reply_Count += 1;

            //检查客户端传输的ID是不是错误的
            if(!await _commentService.IsExistAsync(replyViewModel.Original_Evaluate_ID, Domain.Enums.CommentCatetory.evaluate))
            {
                return Json(new { code = "400", message = "原评论ID出现问题" });
            }

          
            string Create_replyid = Guid.NewGuid().ToString("N");

            
            DateTime Create_Time = DateTime.Now;

            Reply reply = new Reply()
            {
                ID = Create_replyid,
                UserID = user_id,
                Original_Evaluate_ID = replyViewModel.Original_Evaluate_ID,
                Add_Time = Create_Time,
                //防止自己回复自己
                Super_Evaluate_ID = replyViewModel.super_Evaluate_ID,
                //防止自己回复自己
                Super_Evaluate_Name = super_Evaluate_name,
                Original_Forum_ID=replyViewModel.Original_Forum_ID,  
                Reply_Content=replyViewModel.Content
            };

            try
            {
               await _commentService.AddReplyAsync(reply);
               //发布AddReply事件，使评论的ReplyCount+1
               _container.Publish("AddReply", new ReplySumbitEven() {category=2,SubscriberId= replyViewModel.super_Evaluate_ID, publisherId=user_id,forumID=replyViewModel.Original_Forum_ID,commentId = replyViewModel.Original_Evaluate_ID });

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Json(new { code = "500", message = "出现错误" });
            }

            replyViewModel.NickName = user.Nickname;
            replyViewModel.super_Evaluate_Name = super_Evaluate_name;
            replyViewModel.Id = Create_replyid;
            replyViewModel.Create_Time = Create_Time.ToShortDateString();
            replyViewModel.HeadAddress = user.HeadAddress;

            return Json(new { code = "200",message="回复成功",data=replyViewModel });
        }

        [HttpGet]
        [Route("Comment/getreply")]
        public async Task<IActionResult> GetReplys(string comment_id, int?page)
        {
            int index = 0;
            if (page == null)
            {
                index = 1;
            }
            else
            {
                index = page.Value;
            }

            int pageNumber = 5;

            int replyCount = 0;
            List<Reply> replys = null;
            try
            {
                 replyCount = _commentService.GetReplyCount(comment_id);

                 replys = await _commentService.GetItemReplyAsync(index, pageNumber,comment_id);

            }catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Json(new { Code = "400", Message = "失败" });
            }

            int pagesum = (int)Math.Ceiling((double)replyCount / pageNumber);


            return Json(new { Code = "200", Message = "成功", data =new { pageSum=pagesum,nowPage=index,replys=replys } });

        }
    }
}
