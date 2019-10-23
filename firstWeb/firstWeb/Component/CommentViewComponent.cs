using firstWeb.Domain.Enums;
using firstWeb.Domain.Services.comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstWeb.Component
{
    public class CommentViewComponent:ViewComponent
    {
        private readonly ICommentService _commentService;

        private readonly ILogger<CommentViewComponent> _logger;

        private readonly IConfiguration _configuration;

        public CommentViewComponent(ICommentService comment,ILogger<CommentViewComponent> logger,IConfiguration configuration)
        {
            _commentService = comment;
            _logger = logger;
            _configuration = configuration;
        }

        public IViewComponentResult Invoke(string forum_id,string p)
        {
            int currentpage;

            if(!int.TryParse(p,out currentpage))
            {
                throw new ArgumentException("参数P出现问题");
            }
          
         
            //获取当前帖子的评价数量
            int commentcount = _commentService.GetCommentCount(forum_id);
            //获取每页规定的评价数量
            int RulesCount = int.Parse(_configuration.GetSection("page_Setup:page_comment_count").Value);
            //如果下面条件为true，证明p参数有问题;
            if ((currentpage - 1) * RulesCount > commentcount)
            {
                return View(new List<firstWeb.Domain.Model.Comment>());
            }        
            var comments = _commentService.GetItemComment(currentpage,RulesCount,forum_id);

            ViewData["currentpage"] = currentpage;
            ViewData["comment_page_count"] = (int)Math.Ceiling((double)commentcount / RulesCount);
            return View(comments);

        }
    }
}
