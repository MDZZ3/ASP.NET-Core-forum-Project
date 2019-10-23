using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using firstWeb.Domain.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace firstWeb.Domain.Services.forum
{
    public class ForumService : IForumService
    {
        private readonly IRepository<Forum> _forumRepository;

        public ForumService(IRepository<Forum> forum)
        {
            _forumRepository = forum;
        }

        /// <summary>
        /// 获取某页的帖子
        /// </summary>
        /// <param name="pageIndex">页数</param>
        /// <param name="number">页数中数据的数量</param>
        /// <param name="itemViewModel">用于接收当前页数和总页数</param>
        /// <returns></returns>
        public List<forumViewModel> GetForumsItem(int? pageIndex, int number,out forumItemViewModel itemViewModel)
        {
            int index;
            if (pageIndex==null)
            {
                index = 1;
            }
            else
            {
                index = pageIndex.Value;
            }
            
            //编写获取所有帖子(特定属性)和帖子作者(特定属性)的Linq to SQL
            //PS：这里没有在Table后面使用Include("User"),原因可以查看有道云笔记中的问题总结
           var table = _forumRepository.Table.Select((f)=>new forumViewModel()
            {
                forum_ID=f.ID,
                forum_Title=f.Title,
                forum_Content=f.Content,
                forum_CreateTime=f.Create_Time,
                forum_author_Name=f.User.Nickname,
                forum_author_Id=f.User.ID,
                Reading_volume=f.Reading_volume,
                forum_author_HeaderAddress=f.User.HeadAddress

            });
            //不使用Count属性，因为Count会触发查询，使用Count方法不会
            int pagecount = table.Count();
           
            //赋值当前页数和总页数
            itemViewModel = new forumItemViewModel()
            {
                pageCount = (int)Math.Ceiling((double)pagecount/number),
                CurrentPage=index
            };
            //定义要跳多少个数据
            var skipnumber = (index - 1) * number;

            return table.Skip(skipnumber).Take(number).ToList();
        }

        public void InsertForum(Forum forum)
        {
            try
            {
                _forumRepository.Table.Add(forum);
                _forumRepository.SaveChanges();
            }catch(Exception ex)
            {
                throw ex;
            }
           
        }

        /// <summary>
        /// 根据唯一ID来查找是否存在
        /// </summary>
        /// <param name="id">唯一ID</param>
        /// <returns></returns>
        public async Task<Forum> GetForumAsync(string id)
        {
            var forum=await _forumRepository.Table.Include(f=>f.Comments).Include(f=>f.User).AsNoTracking().FirstOrDefaultAsync(f=>f.ID==id);
           

            return forum == null ? null : forum;
            
        }

        public async Task<bool> isExistAsync(string id)
        {
            return await _forumRepository.Table.Select(f => f.ID).FirstOrDefaultAsync(ID => ID == id) == null ? false : true;
        }

        public List<forumViewModel> GetCategoryItemForums(int categoryid, int? pageIndex, int number, out forumItemViewModel itemViewModel)
        {
            int index;
            if (pageIndex == null)
            {
                index = 1;
            }
            else
            {
                index = pageIndex.Value;
            }

            //编写获取所有帖子(特定属性)和帖子作者(特定属性)的Linq to SQL
            //PS：这里没有在Table后面使用Include("User"),原因可以查看有道云笔记中的问题总结
            var table = _forumRepository.Table.Where(f=>f.CategoryId==categoryid).Select((f) => new forumViewModel()
            {
                forum_ID = f.ID,
                forum_Title = f.Title,
                forum_Content = f.Content,
                forum_CreateTime = f.Create_Time,
                forum_author_Name = f.User.Nickname,
                forum_author_Id = f.User.ID,
                Reading_volume = f.Reading_volume,
                forum_author_HeaderAddress = f.User.HeadAddress

            });
            //不使用Count属性，因为Count会触发查询，使用Count方法不会
            int pagecount = table.Count();

            //赋值当前页数和总页数
            itemViewModel = new forumItemViewModel()
            {
                pageCount = (int)Math.Ceiling((double)pagecount / number),
                CurrentPage = index
            };
            //定义要跳多少个数据
            var skipnumber = (index - 1) * number;

            return table.Skip(skipnumber).Take(number).ToList();
        }
    }
}
