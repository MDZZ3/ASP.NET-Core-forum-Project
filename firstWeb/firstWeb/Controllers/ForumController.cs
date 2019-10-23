using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using firstWeb.Core.Helper;
using firstWeb.Domain.Even;
using firstWeb.Domain.Model;
using firstWeb.Domain.Receive;
using firstWeb.Domain.Services.accountService;
using firstWeb.Domain.Services.comment;
using firstWeb.Domain.Services.contern;
using firstWeb.Domain.Services.forum;
using firstWeb.Domain.Services.forum_Category;
using firstWeb.Domain.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace firstWeb.Controllers
{
    public class ForumController : BaseController
    {
        private readonly IHostingEnvironment _environment;

       // private readonly IforumCategoryService _categoryService;

        private readonly IHttpClientFactory _factory;

        private readonly ILogger<ForumController> _logger;

        private readonly IForumService _forumservice;

        private readonly EvenHandlerContainer _container;

        private readonly IConternService _conternService;

        private readonly IAccountService _accountService;

        public ForumController(IHostingEnvironment hosting,
            IConfiguration configuration,
            IHttpClientFactory clientFactory,
            ILogger<ForumController> Logger,
            IForumService forum,
            ICommentService comment,
            EvenHandlerContainer container,
            IConternService contern,
            IAccountService account
            ):base(configuration)
        {
            _forumservice = forum;
            _logger = Logger;
            _factory = clientFactory;
            _environment = hosting;
            _container = container;
            _conternService = contern;
            _accountService = account;
        }
        
        
        [HttpGet]
        [Authorize]
        public IActionResult Create_forum()
        {
            // List<forumCategory> forumCategories = _categoryService.GetCategoryList();
            //取出配置的分类
            Dictionary<string, string> forumCategories = _configuration.GetSection("Forum_Category:data").Get<Dictionary<string, string>>();

            var selectLists = GetSelectListItem(forumCategories);
            ViewBag.CategoryItem = selectLists;
           
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create_forum([FromBody] forumViewModel forumViewmodel)
        {
            int titleLength =int.Parse(_configuration.GetSection("Forum_limit:Title_Length").Value);
            int MaxCagegory =int.Parse(_configuration.GetSection("Forum_limit:MaxCategoryID").Value);
            if (string.IsNullOrEmpty(forumViewmodel.forum_Title)||forumViewmodel.forum_Title.Length >= titleLength|| forumViewmodel.forum_Title.Length <= 0)
            {
                //使用方法尚未理清，可以修改
                return StatusCode(400, new { Code = "400", Message = "题目不合格" });
            }

            if (forumViewmodel.forum_Category == 0 || forumViewmodel.forum_Category > MaxCagegory)
            {
                //使用方法尚未理清，可以修改
                return StatusCode(400, new { Code = "400", Message = "帖子分类不符合" });
            }

            //获取身份凭证
            string phone = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.OtherPhone).Value;

            HttpClient hc = _factory.CreateClient("forum_Server");
            string Cookie = Request.Headers.FirstOrDefault(h => h.Key == "Cookie").Value;
            hc.DefaultRequestHeaders.Add("Cookie", Cookie);

            string response = string.Empty;

            try
            {
                response= await hc.GetStringAsync($"/Userinfo/GetId?phone={phone}");
            }
            catch(Exception ex)
            {
                //如果请求过程出现异常，则写入日志
                _logger.LogError(ex.StackTrace);
            }
           
            //接收用户服务端的数据
            HttpReceive receive = JsonConvert.DeserializeObject<HttpReceive>(response);

            if (receive.Code != "200")
            {
                //返回方式可优化
                return StatusCode(int.Parse(receive.Code));
            }

            Forum forum = BuildForum(receive.data.ID,forumViewmodel);

            //触发添加帖子事件，使 用户帖子数量+1
            _container.Publish("CreateForum", new CreateForumSumbitEven() { userid= receive.data.ID });

            try
            {
                _forumservice.InsertForum(forum);
            }catch(Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return Json(new { Code = "500", Message = "添加失败" });
            }
          
            return Json(new { Code = "200", Message = "添加成功" });
        }

        /// <summary>
        /// 将forumViewModel转换成Forum类
        /// </summary>
        /// <param name="id">作者ID</param>
        /// <param name="forumViewmodel">源目标</param>
        /// <returns></returns>
        private Forum BuildForum(string id,forumViewModel forumViewmodel)
        {
            Forum forum = new Forum();
            //如果题目写成了按js代码(例如<script>alert("di")</script>),ASP.NET Core不会对其XSS过滤
            //但这里不用XSSHelper过滤也可以，
            //详细看https://docs.microsoft.com/zh-cn/aspnet/core/security/cross-site-scripting?view=aspnetcore-2.2
            //中Razor的HTML编码
            // forum.Title = XSSHelper.Sanitizer(forumViewmodel.forum_Title);
            forum.Title = forumViewmodel.forum_Title;

            //这里不用XSSHelper过滤，ASP.NET Core也会帮你过滤
            //（PS：这里不太懂XSS过滤机制，为什么Title属性没有XSS过滤，而forum_Content却XSS过滤了）
            //forum.Content = forumViewmodel.forum_Content;
            forum.Content = XSSHelper.Sanitizer(forumViewmodel.forum_Content);
            forum.CategoryId = forumViewmodel.forum_Category; 
            forum.Create_Time = DateTime.Now;
            forum.UserId = id;
            forum.IsElite = 0;
            return forum;                       
        }

        /// <summary>
        /// 将包含forumCategory类的List转换成List<SelectListItem>
        /// </summary>
        /// <param name="forumCategories">原数据</param>
        /// <returns>新数据</returns>
        private List<SelectListItem> GetSelectListItem(Dictionary<string, string> forumCategories)
        {          
            List<SelectListItem> selectLists = new List<SelectListItem>();
            foreach (var category in forumCategories)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = category.Value,
                    Value = category.Key
                };
                selectLists.Add(selectListItem);
            }
            return selectLists;
        }

        [HttpGet]
        [Route("Forum/{id:int}")]
        public async Task<IActionResult> GetForum(string id,string p = "1")
        {
            if (string.IsNullOrEmpty(id))
            {
                return View("Error404");
            }
            var forum = await _forumservice.GetForumAsync(id);
            if (forum == null)
            {
                return View("Error404");
            }
            
            var detailedforumViewModel = new DetailedforumViewModel()
            {
                forum = forum
            };

            if (User.Identity.IsAuthenticated)
            {
                //获取用户ID
                string phone = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.OtherPhone).Value;
                string user_id = _accountService.GetUserID(phone);

                if (user_id==forum.UserId)
                {
                    detailedforumViewModel.isConcern = 2;
                }
                else if (_conternService.IsContern(forum.UserId, user_id))
                {
                    detailedforumViewModel.isConcern = 1;
                }
                else
                {
                    detailedforumViewModel.isConcern = 0;
                }
            }
            else
            {
                detailedforumViewModel.isConcern = 0;
            }

            ViewData["currentPage"] = p;


            return View("Detailed_forum", detailedforumViewModel);
        }

       

        //[HttpPost]
        //[Route("Forum/reply")]
        //public IActionResult forumReply()
        //{

        //}

        /// <summary>
        /// 富文本图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/upload")]
        [Authorize]
        public async Task<IActionResult> Forum_pic()
        {
            var User = HttpContext.User;

            var phone = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.OtherPhone)?.Value;

            var Files = Request.Form.Files;

            if (Files.Count > 0)
            {
                List<string> FilePaths = new List<string>();
                var Folder=$@"{_environment.WebRootPath}\upload\{phone}";
                //检测文件夹有没有被创建，如果没有则创建一个文件夹
                if (!FileHelper.IsExistDirectory(Folder))
                {
                    FileHelper.CreateFolder(Folder);
                }
                
                foreach (var file in Files)
                {
                    //获取文件扩展名
                    string ExtensionName = FileHelper.GetExtensionName(file.FileName);
                    //定义保存路径
                    string GuidName = Guid.NewGuid().ToString();
                    string path = $@"\upload\{phone}\{ GuidName}.{ExtensionName}";
                    string Savepath =$@"{_environment.WebRootPath}\{path}";
                    //将文件写入服务器
                    using (var filesteam=new FileStream(Savepath, FileMode.Create))
                    {
                        await file.CopyToAsync(filesteam);
                    }

                    FilePaths.Add(path);

                }
                return Json(new { errno = 0, data = FilePaths });
            }
            return Json(new { errno = 400 });
            
        }

      
    }
}