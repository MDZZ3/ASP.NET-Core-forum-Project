using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using forum_SSO_Server.Models;
using Microsoft.AspNetCore.Mvc;
using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Models;
using forum.DAL.Repository;
using forum.DAL.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using forum.Domain.Services;
using firstWeb.Core.Helper;
using System.Security.Cryptography;
using forum.Domain.Helper;

namespace forum_SSO_Server.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _IdentityInteractionServce;
        private readonly IClientStore _ClientStore;
        private readonly IAccountServices _user;
        

        public AccountController(IIdentityServerInteractionService interactionService,IClientStore clientStore
            ,IRepository<User>user, IAccountServices account)
        {
            _IdentityInteractionServce = interactionService;
            _ClientStore = clientStore;    
            _user = account;
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginViewModel viewModel)
        {
            if (viewModel == null)
            {
                return StatusCode(400);
            }
            if (string.IsNullOrEmpty(viewModel.PassWord)||string.IsNullOrEmpty(viewModel.UserName))
            {
                return StatusCode(400);
            }
            
            if (!string.IsNullOrEmpty(viewModel.returnUri))
            {
                AuthorizationRequest authorizationRequest = await _IdentityInteractionServce.GetAuthorizationContextAsync(viewModel.returnUri);
                if (authorizationRequest == null)
                {
                    return Json(new { state = "401", Message = "参数错误" });
                }
                if (await _ClientStore.FindClientByIdAsync(authorizationRequest.ClientId) == null)
                {
                    return Json(new { state = "401", Message = "参数错误" });
                }
                forum.DAL.Entity.User user = null;
                //try
                //{
                   user = _user.GetAccount(viewModel.UserName, MD5Helper.GetMD5(viewModel.PassWord));
                //}
                //catch(ArgumentException )
                //{
                //    return Json(new { state = "401", Message = "账号或密码为空" });
                //}
                //catch(Exception ex)
                //{
                //    return Json(new { state = "500", Message = "出现错误，请重新刷新页面" });
                //}

                if (user != null)
                {
                    Claim[] claims = new Claim[]
                    {
                       new Claim(ClaimTypes.Name, user.Nickname),
                       new Claim(ClaimTypes.OtherPhone,user.Phone),
                       new Claim(ClaimTypes.PrimarySid,user.ID)
                    };

                    await HttpContext.SignInAsync(viewModel.UserName, claims);
                    
                    return Json(new { state = "302", location = viewModel.returnUri });
                    
                }
                return Json(new { state = "401", Message = "账号或密码错误" });
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterViewModel viewmodel)
        {
            if (viewmodel == null)
            {
                return Json(new { state = "401", Message = "参数错误" });
            }
            if (string.IsNullOrEmpty(viewmodel.username) || string.IsNullOrEmpty(viewmodel.password) || string.IsNullOrEmpty(viewmodel.code))
            {
                return Json(new { state = "401", Message = "参数错误" });
            }

            if (_user.isExist(viewmodel.username))
            {
                return Json(new { state = "403", Message = "手机号已被注册" });
            }

            User user = new User()
            {
                ID = Snowflake.Instance().GetId().ToString(),
                Nickname = viewmodel.username,
                PassWord = MD5Helper.GetMD5(viewmodel.password),
                Phone = viewmodel.username,
                HeadAddress= "images/dadultHead.jpg",
                Register_Time=DateTime.Now,
            };
            try
            {
                _user.RegisterUser(user);
            }
            catch (Exception ex)
            {
                return Json(new { state = "500", Message = "出现错误，请重新刷新页面" });
            }

            return Json(new { state = "200", Message = "注册成功" });
        }


        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var logout= await _IdentityInteractionServce.GetLogoutContextAsync(logoutId);
            await HttpContext.SignOutAsync();
            if (logout.PostLogoutRedirectUri != null)
            {
                return Redirect(logout.PostLogoutRedirectUri);
            }
            var redirectUrl = HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(redirectUrl);
        }

        [HttpGet]
        [Route("Userinfo")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            
            string phone = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.OtherPhone).Value;
            User user = _user.GetUserInfo(phone);
            if (user == null) 
            {
                return Json(new { State = "401", Message = "身份错误" });
            }

            var data = new
            {
                NickName = user.Nickname,
                Last_Login_Time = user.Last_Login_Time.ToShortDateString(),
                Login_Time = user.Login_Time.ToShortDateString(),
            };
            return Json(new { state = "200",Message="成功",data = data });
        }
    }
}