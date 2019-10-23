using firstWeb.Domain.Enums;
using firstWeb.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace firstWeb.Domain.Services.comment
{
    public interface ICommentService
    {
        /// <summary>
        /// 根据某ID查找相关评论
        /// </summary>
        /// <param name="id">id值</param>
        /// <param name="dType">枚举值，里面有两个值</param>
        /// <returns>与ID相关的评论</returns>
        Task<List<Comment>> GetCommentsAsync(string id, IDType dType);

        Task<List<Comment>> GetCommentsAsync(string id, IDType dType, SortOrder order);

        List<Comment> GetComments(string id, IDType dType, SortOrder order);

        List<Comment> GetComments(string id, IDType dType);

        /// <summary>
        /// 通过评论id获取回复
        /// </summary>
        /// <param name="Comment_ID">评论id</param>
        /// <returns></returns>
        Task<List<Reply>> GetReplysAsync(string Comment_ID);


        Task<List<Reply>> GetItemReplyAsync(int page, int Number, string Comment_ID);

        /// <summary>
        /// 获取分页版的回复
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="Number">当前页的数量</param>
        /// <param name="Comment_ID">评价ID</param>
        /// <returns></returns>
        Task<List<Reply>> GetItemReplyAsync(int page, int Number, string Comment_ID,SortOrder order);



        List<Comment> GetItemComment(int page, int Number, string forum_Id);
        /// <summary>
        /// 获取分页版的评论
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="Number">当前页的数量</param>
        /// <param name="forum_Id">帖子ID</param>
        /// <param name="order">排序方式</param>
        /// <returns></returns>
        List<Comment> GetItemComment(int page, int Number, string forum_Id, SortOrder order);

        


        Task<List<Reply>> GetReplysAsync(string Comment_ID,SortOrder order);

        /// <summary>
        /// 获取评论下的回复数量
        /// </summary>
        /// <param name="Comment_ID">评价ID</param>
        /// <returns></returns>
        int GetReplyCount(string Comment_ID);

        /// <summary>
        /// 获取帖子下的评论数量
        /// </summary>
        /// <param name="forum_ID">帖子ID</param>
        /// <returns></returns>
        int GetCommentCount(string forum_ID);

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="comment">评论</param>
        Task AddCommentAsync(Comment comment);

        /// <summary>
        /// 添加回复
        /// </summary>
        /// <param name="reply">回复</param>
        Task AddReplyAsync(Reply reply);

        /// <summary>
        /// 根据ID查看评论(或者回复)是否存在
        /// </summary>
        /// <param name="comment_id"></param>
        /// <param name="catetory"></param>
        /// <returns></returns>
        Task<bool> IsExistAsync(string comment_id,CommentCatetory catetory);


        Task<Comment> GetCommentAsync(string comment_id);


    }
}
