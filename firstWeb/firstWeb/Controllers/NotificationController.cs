using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using firstWeb.Domain.Model;
using firstWeb.Domain.Services.accountService;
using firstWeb.Domain.Services.notice;
using firstWeb.Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace firstWeb.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly INoticeService _noticeService;

        private readonly IAccountService _accountService;

        public NotificationController(IConfiguration configuration,INoticeService notice,IAccountService account) :base(configuration)
        {
            _noticeService = notice;
            _accountService = account;
            
        }

        [HttpGet]
        [Authorize]
        [Route("/Notification/Container")]
        public IActionResult Notification_Container(string active)
        {
            if (string.IsNullOrEmpty(active))
            {
                return View("Error404");
            }
            int activenumber;
            if (!int.TryParse(active,out activenumber))
            {
                return View("Error404");
            }
            int concern_active_number = int.Parse(_configuration.GetSection("page_active_number:Notice:concern").Value);
            int comment_active_number = int.Parse(_configuration.GetSection("page_active_number:Notice:comment").Value);
            int System_notice_active_number = int.Parse(_configuration.GetSection("page_active_number:Notice:system_notice").Value);
            if (activenumber != concern_active_number && activenumber != comment_active_number && activenumber != System_notice_active_number)
            {
                return View("Error404");
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult comment_notice(string page)
        {
            if (string.IsNullOrEmpty(page))
            {
                return StatusCode(400);
            }
            int currentpage;
            if (!int.TryParse(page,out currentpage))
            {
                return StatusCode(400);
            }
            int RulesCount =int.Parse(_configuration.GetSection("page_Setup:page_comment_reply_noticeCount").Value);
            //获取用户ID
            string phone = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.OtherPhone).Value;
            string user_id = _accountService.GetUserID(phone);

            var notices = _noticeService.GetItemCommentAndReplyNotice(currentpage, RulesCount, user_id);

            var noticesCount = _noticeService.GetCommentAndReplyNoticeCount(user_id);

            //记录当前页面和页面总数
            ViewData["currentpage"] = currentpage;
            ViewData["pageCount"] = (int)Math.Ceiling((double)noticesCount / RulesCount);

            //通过获取的List<Notice>，构建Viewmodel
            List<NoticeViewModel> viewmodel = BuilderNoticeViewModel(notices);

            return View(viewmodel);
        }

      

        [HttpGet]
        public IActionResult concern_notice(string page)
        {
            if (string.IsNullOrEmpty(page))
            {
                return View("Error404");
            }
            int currentpage;
            if (!int.TryParse(page,out currentpage))
            {
                return View("Error404");
            }
            int RulesCount = int.Parse(_configuration.GetSection("page_Setup:page_concern_noticeCount").Value);
            //获取用户ID
            string phone = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.OtherPhone).Value;
            string user_id = _accountService.GetUserID(phone);

            var notices = _noticeService.GetItemConcernNotice(currentpage, RulesCount, user_id);

            var noticesCount = _noticeService.GetConcernNoticeCount(user_id);

            //记录当前页面和页面总数
            ViewData["currentpage"] = currentpage;
            ViewData["pageCount"] = (int)Math.Ceiling((double)noticesCount / RulesCount);

            //通过获取的List<Notice>，构建Viewmodel
            List<NoticeViewModel> viewmodel = BuilderNoticeViewModel(notices);

            return View(viewmodel);
        }

        [HttpGet]
        public IActionResult System_notice()
        {
            return View();
        }

        private List<NoticeViewModel> BuilderNoticeViewModel(List<Notice> notices)
        {
            List<NoticeViewModel> result = new List<NoticeViewModel>();
            foreach (Notice notice in notices)
            {
                string publisher_Nickname = _accountService.GetNickName(notice.PublisherID);
                NoticeViewModel model = new NoticeViewModel()
                {
                    Nickname = publisher_Nickname,
                    notice = notice
                };
                result.Add(model);
            }
            return result;
        }
    }
}