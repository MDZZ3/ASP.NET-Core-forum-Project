using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using forum.DAL.Entity;
using forum.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace forum_SSO_Server.Controllers
{
    public class UserinfoController : Controller
    {
        private readonly IAccountServices _account;

        public UserinfoController(IAccountServices account)
        {
            _account = account;
        }

        [HttpGet]
        [Authorize]
        [Route("/Userinfo/GetId")]
        public IActionResult GetId(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return Json(new { Code = "400", Message = "参数不合格" });
            }

            string id = null;

            try
            {
                 id = _account.GetUserID(phone);
            }
            catch (Exception ex)
            {
                return Json(new { code = "400", Message = "查无此人" });
            }
            return Json(new { Code = "200", Message = "成功", data = new { ID = id } });
           
        }
    }
}