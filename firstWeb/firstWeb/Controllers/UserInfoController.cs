using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace firstWeb.Controllers
{
    public class UserInfoController : Controller
    {
        [HttpGet]
        public IActionResult UserInfo_Container(string active)
        {
            if (string.IsNullOrEmpty(active))
            {
                return View("Error404");
            }

            int number;
            //查看active是否是数字
            if (int.TryParse(active,out number))
            {
                //查看active是否是1或2
                if (number != 1102 && number != 1103)
                {
                    return View("Error404");
                }
            }
            else
            {
                return View("Error404");
            }

            return View();
        }

        [HttpGet]
        public IActionResult User_forums()
        {
            
            return View("UserInfo_forums", "我的帖子");
        }

        [HttpGet]
        public IActionResult User_Comments()
        {
            return View("UserInfo_Comments", "我的评论");
        }
    }
}