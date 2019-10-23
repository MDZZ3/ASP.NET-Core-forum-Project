using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using firstWeb.Models;
using System.Text.Encodings.Web;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using firstWeb.Models.bean;
using Newtonsoft.Json;
using firstWeb.Domain.Services.forum;
using firstWeb.Domain.ViewModel;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace firstWeb.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _log;
        private readonly IHttpClientFactory _factory;
        private readonly IForumService _forumserver;


        public HomeController(IConfiguration configuration, ILogger<HomeController> Logger, IHttpClientFactory factory, IForumService forum) : base(configuration)
        {
            _log = Logger;
            _factory = factory;
            _forumserver = forum;
        }

        [HttpGet]
        public IActionResult Index(string category,int? page)
        {
            //取出默认的分类定向
            string defaultcategoryindex = _configuration.GetSection("Forum_Category:Dedault").Value;
            if (string.IsNullOrEmpty(category))
            {
                category = defaultcategoryindex;
            }

            //取出配置文件的分类，使用字典存储
            Dictionary<string,string> categorys = _configuration.GetSection("Forum_Category:data").Get<Dictionary<string, string>>();
            var flag = categorys.ContainsKey(category);

            //如果category不是分类的一个，又不是默认cateogry，那么就返回404页面
            if (!categorys.ContainsKey(category))
            {
                if (defaultcategoryindex != category)
                {
                    return View("Error404");
                }                
            }
          

            var forumItemViewModel = new forumItemViewModel();
            var forums = new List<forumViewModel>();

            int page_forum_count =int.Parse(_configuration.GetSection("page_Setup:page_forum_count").Value);

            //如果category是默认的话，返回全部帖子的分页版
            if (defaultcategoryindex == category)
            {
                forums = _forumserver.GetForumsItem(page, page_forum_count, out forumItemViewModel);
            }
            else
            {
                forums = _forumserver.GetCategoryItemForums(int.Parse(category), page, page_forum_count, out forumItemViewModel);
            }
           
            forumItemViewModel.forumViewModels = forums;
            //将分类索引赋值给viewmodel类
            forumItemViewModel.categoryindex = category;

            forumItemViewModel.CategoryList = categorys;

            ViewBag.ReturnUrl =WebUtility.UrlEncode(Request.Path.Value);

            return View(forumItemViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> New() 
        {
            var hc = _factory.CreateClient("client1");

            string Cookie = Request.Headers.FirstOrDefault(h => h.Key == "Cookie").Value;  
            hc.DefaultRequestHeaders.Add("Cookie", Cookie);

            var responseMessage = await hc.GetStringAsync("/Userinfo");

            UserinfoResponse response = JsonConvert.DeserializeObject<UserinfoResponse>(responseMessage);

            return View(response.data);


        }

    }
}
