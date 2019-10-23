using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using firstWeb.Domain.Even;
using firstWeb.Domain.Services.accountService;
using firstWeb.Domain.Services.contern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace firstWeb.Controllers
{
    public class AccountController : Controller
    {

        private readonly IHttpClientFactory _factory;

        private readonly IAccountService _account;

        private readonly IConternService _contern;

        private readonly EvenHandlerContainer _container;

        private readonly ILogger<AccountController> _logger;

        public AccountController(IHttpClientFactory factory,IAccountService account,
            IConternService contern,
            ILogger<AccountController> logger,
            EvenHandlerContainer container)
        {
            _factory = factory;
            _account = account;
            _contern = contern;
            _logger = logger;
            _container = container;

        }

        [HttpGet]
        [Authorize]
        public IActionResult Login(string returnUrl)
        {
            string phone = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            return Redirect(returnUrl);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            return SignOut("Cookies","oidc");
        }

        [HttpGet]
        [Route("/Account/homepage-{tid}")]
        public IActionResult Personal_homepage(String tid)
        {
            var account = _account.GetAcccount(tid);
            if (account == null)
            {
                return View("Error404");
            }
            return View(account);
        }

        [HttpPost]
        [Authorize]
        [Route("/Concern/Create")]
        public async Task<IActionResult> AddConcern([FromBody]JObject jObject)
        {
            string auth_ID = jObject["auth_ID"]?.ToString();

            if (string.IsNullOrEmpty(auth_ID))
            {
                return Json(new { code = "400", message = "出现问题,重新尝试" });
            }
            //查看作者是否存在
            if (!_account.IsExist(auth_ID))
            {
                return Json(new { code = "400", message = "关注人不存在" });
            }

            string user_id = GetCurrentID();

            //防止关注自己
            if (user_id == auth_ID)
            {
                return Json(new { code = "400", message = "不可以关注自己" });
            }

            if (_contern.IsContern(auth_ID, user_id))
            {
                return Json(new { code = "400", message = "已关注" });
            }
            try
            {
               await  _contern.CreateConternAsync(auth_ID, user_id);
                //发布关注通知事件
                _container.Publish("CreateConcern", new ConcernSumbitEven() { category = 3, SubscriberId = auth_ID, publisherId = user_id });

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Json(new { code = "500", message = "出现问题，请重新刷新" });
            }

            return Json(new { code = "200", message = "关注成功" });
        }

        [HttpPost]
        [Authorize]
        [Route("/Concern/destroy")]
        public async Task<IActionResult> removeConcern([FromBody] JObject jObject)
        {
            string auth_ID = jObject["auth_ID"]?.ToString();
            if (string.IsNullOrEmpty(auth_ID))
            {
                return Json(new { code = "400", message = "出现错误，请重试" });
            }
            //检测取关人是否存在
            if (!_account.IsExist(auth_ID))
            {
                 return Json(new { code = "400", message = "取关人不存在" });
            }
            string user_id = GetCurrentID();

            //一般来说不会存在取关自己的情况，以防万一
            if (user_id == auth_ID)
            {
                return Json(new { code = "400", message = "不可以取关自己" });
            }
            if (!_contern.IsContern(auth_ID, user_id))
            {
                return Json(new { code = "400", message = "无法取关" });
            }
            try
            {
                await _contern.RemoveConternAsync(auth_ID, user_id);
            }catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Json(new { code = "500", message = "出现错误，请联系管理员" });
            }

            return Json(new { code = "200", message = "成功" });
        }

        /// <summary>
        /// 获取当前登录的用户ID
        /// </summary>
        /// <returns></returns>
        private string GetCurrentID()
        {
            //获取登录用户的ID
            string phone = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.OtherPhone)?.Value;
            return _account.GetUserID(phone);          
        }
    }
}