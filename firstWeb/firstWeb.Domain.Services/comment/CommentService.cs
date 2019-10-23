using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using firstWeb.Domain.Enums;
using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace firstWeb.Domain.Services.comment
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repository;

        private readonly IRepository<Reply> _replyrepository;

        private readonly IRepository<User> _userrepository;

        public CommentService(IRepository<Comment> repository,IRepository<Reply> reply,IRepository<User> user)
        {
            _replyrepository = reply;
            _repository = repository;
            _userrepository = user;
        }

        /// <summary>
        /// 根据某ID查找相关评论(默认是降序)
        /// </summary>
        /// <param name="id">id值</param>
        /// <param name="dType">枚举类，里面有两个值</param>
        /// <returns>与ID相关的评论</returns>
        public async Task<List<Comment>> GetCommentsAsync(string id, IDType dType)
        {
            return await GetCommentsAsync(id, dType, SortOrder.Ascending_order);
        }

        public List<Comment> GetComments(string id, IDType dType)
        {
            return GetComments(id, dType, SortOrder.Ascending_order);
        }

        /// <summary>
        /// 根据某ID查找相关评论
        /// </summary>
        /// <param name="id">id值</param>
        /// <param name="dType">枚举类，里面有两个值</param>
        /// <param name="order">枚举类，里面有两个值</param>
        /// <returns></returns>
        public async Task<List<Comment>> GetCommentsAsync(string id, IDType dType, SortOrder order)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id为Null或为空");
            }

            if (dType == IDType.UserID)
            {
                if (order == SortOrder.Ascending_order)
                {
                    return await _repository.Table.Where(c => c.UserID == id).Include(c => c.User).OrderBy(c=>c.Create_Time).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _repository.Table.Where(c => c.UserID == id).Include(c => c.User).OrderByDescending(c => c.Create_Time).AsNoTracking().ToListAsync();
                }
               
            }
            else
            {
                if (order == SortOrder.Ascending_order)
                {
                    return await _repository.Table.Where(c => c.forumID == id).OrderBy(c => c.Create_Time).AsNoTracking().ToListAsync();
                }
                else
                {
                   return await _repository.Table.Where(c => c.forumID == id).OrderByDescending(c=>c.Create_Time).AsNoTracking().ToListAsync();
                }
                
            }
        }

        public List<Comment> GetComments(string id, IDType dType, SortOrder order)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id为Null或为空");
            }

            if (dType == IDType.UserID)
            {
                if (order == SortOrder.Ascending_order)
                {
                    return  _repository.Table.Where(c => c.UserID == id).Include(c=>c.User).OrderBy(c => c.Create_Time).AsNoTracking().ToList();
                }
                else
                {
                    return   _repository.Table.Where(c => c.UserID == id).Include(c => c.User).OrderByDescending(c => c.Create_Time).AsNoTracking().ToList();
                }

            }
            else
            {
                if (order == SortOrder.Ascending_order)
                {
                    return  _repository.Table.Where(c => c.forumID == id).Include(c => c.User).OrderBy(c => c.Create_Time).AsNoTracking().ToList();
                }
                else
                {
                    return  _repository.Table.Where(c => c.forumID == id).Include(c => c.User).OrderByDescending(c => c.Create_Time).AsNoTracking().ToList();
                }

            }
        }


        public async Task<List<Reply>> GetReplysAsync(string Comment_ID)
        {
            return await GetReplysAsync(Comment_ID, SortOrder.Ascending_order);
        }

        public async Task<List<Reply>> GetReplysAsync(string comment_ID, SortOrder order)
        {
            if (order == SortOrder.Ascending_order)
            {
               return await GetOrderByReplyAsync(comment_ID);
            }
            else
            {
                return await GetOrderByDescendingReplyAsync(comment_ID);
            }
        }

        private async Task<List<Reply>> GetOrderByReplyAsync(string comment_ID)
        {
            return await _replyrepository.Table.Where(r => r.Original_Evaluate_ID == comment_ID).OrderBy(r => r.Add_Time).Join(_userrepository.Table, (Reply r) => r.UserID, (User c) => c.ID, (Reply r, User u) => new Reply
            {
                ID = r.ID,
                reply_NickName = u.Nickname,
                Reply_Content = r.Reply_Content,
                Add_Time = r.Add_Time,
                Original_Evaluate_ID = r.Original_Evaluate_ID,
                UserID = r.UserID,
                Super_Evaluate_ID = r.Super_Evaluate_ID,
                Original_Forum_ID = r.Original_Forum_ID,
                Super_Evaluate_Name = r.Super_Evaluate_Name,
                reply_HeaderAddress = u.HeadAddress
            }).AsNoTracking().ToListAsync();

        }

        private async Task<List<Reply>> GetOrderByDescendingReplyAsync(string comment_ID)
        {
            return await _replyrepository.Table.Where(r => r.Original_Evaluate_ID == comment_ID).OrderByDescending(r => r.Add_Time).Join(_userrepository.Table, (Reply r) => r.UserID, (User c) => c.ID, (Reply r, User u) => new Reply
            {

                ID = r.ID,
                reply_NickName = u.Nickname,
                Reply_Content = r.Reply_Content,
                Add_Time = r.Add_Time,
                Original_Evaluate_ID = r.Original_Evaluate_ID,
                UserID = r.UserID,
                Super_Evaluate_ID = r.Super_Evaluate_ID,
                Original_Forum_ID = r.Original_Forum_ID,
                Super_Evaluate_Name = r.Super_Evaluate_Name,
                reply_HeaderAddress = u.HeadAddress


            }).AsNoTracking().ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
                if (comment == null)
                {
                    throw new ArgumentException("comment参数为Null");
                }
               await  _repository.Table.AddAsync(comment);
               await _repository._db.SaveChangesAsync();           
        }

        public async Task AddReplyAsync(Reply reply)
        {
            if (reply == null)
            {
                throw new ArgumentException("reply参数为空");

            }
           await _replyrepository.Table.AddAsync(reply);
           await _replyrepository._db.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(string id, CommentCatetory catetory)
        {
            if (catetory == CommentCatetory.evaluate)
            {
                return await _repository.Table.Select(c => c.ID).FirstOrDefaultAsync(c => c == id) == null ?false:true;
            }
            else
            {
                return await _replyrepository.Table.Select(c => c.ID).FirstOrDefaultAsync(c => c == id) == null ? false : true;
            }
        }

        public Task<Comment> GetCommentAsync(string comment_id)
        {
            return _repository.Table.FirstOrDefaultAsync(c => c.ID == comment_id);
        }



        public Task<List<Reply>> GetItemReplyAsync(int page, int Number, string Comment_ID, SortOrder order)
        {
           
            
            int skipNumber = (page - 1) * Number;

            if (SortOrder.Ascending_order == order)
            {
                return _replyrepository.Table.Where(r=>r.Original_Evaluate_ID==Comment_ID).OrderByDescending(r => r.Add_Time).AsNoTracking().Skip(skipNumber).Take(Number).Join(_userrepository.Table, (Reply r) => r.UserID, (User c) => c.ID, (Reply r, User u) => new Reply
                {
                    ID = r.ID,
                    reply_NickName = u.Nickname,
                    Reply_Content = r.Reply_Content,
                    Add_Time = r.Add_Time,
                    Original_Evaluate_ID = r.Original_Evaluate_ID,
                    UserID = r.UserID,
                    Super_Evaluate_ID = r.Super_Evaluate_ID,
                    Original_Forum_ID = r.Original_Forum_ID,
                    Super_Evaluate_Name = r.Super_Evaluate_Name,
                    reply_HeaderAddress = u.HeadAddress


                }).ToListAsync();
            }
            else
            {
                return _replyrepository.Table.Where(r => r.Original_Evaluate_ID == Comment_ID).OrderBy(r => r.Add_Time).AsNoTracking().Skip(skipNumber).Take(Number).Join(_userrepository.Table, (Reply r) => r.UserID, (User c) => c.ID, (Reply r, User u) => new Reply
                {
                    ID = r.ID,
                    reply_NickName = u.Nickname,
                    Reply_Content = r.Reply_Content,
                    Add_Time = r.Add_Time,
                    Original_Evaluate_ID = r.Original_Evaluate_ID,
                    UserID = r.UserID,
                    Super_Evaluate_ID = r.Super_Evaluate_ID,
                    Original_Forum_ID = r.Original_Forum_ID,
                    Super_Evaluate_Name = r.Super_Evaluate_Name,
                    reply_HeaderAddress = u.HeadAddress


                }).ToListAsync();
            }
        }

        public int GetReplyCount(string Comment_ID)
        {
            return _replyrepository.Table.Count(r => r.Original_Evaluate_ID == Comment_ID);
        }

        public Task<List<Reply>> GetItemReplyAsync(int page, int Number, string Comment_ID)
        {
            return GetItemReplyAsync(page, Number,Comment_ID, SortOrder.Descending_order);
        }

        public int GetCommentCount(string forum_ID)
        {
            return _repository.Table.Where(c => c.forumID == forum_ID).AsNoTracking().Count();
        }

        public List<Comment> GetItemComment(int page, int Number, string forum_Id)
        {
            return GetItemComment(page, Number, forum_Id, SortOrder.Descending_order);
        }

        public List<Comment> GetItemComment(int page, int Number, string forum_Id, SortOrder order)
        {
            int skipnumber = (page - 1) * Number;
            if (order == SortOrder.Ascending_order)
            {
                return  _repository.Table.Where(c => c.forumID == forum_Id).Include(c => c.User).AsNoTracking().OrderByDescending(c => c.Create_Time).Skip(skipnumber).Take(Number).ToList();
            }
            else
            {
                return  _repository.Table.Where(c => c.forumID == forum_Id).Include(c => c.User).AsNoTracking().OrderBy(c => c.Create_Time).Skip(skipnumber).Take(Number).ToList();
            }
        }

     
    }
}
