using firstWeb.Domain.Enums;
using firstWeb.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Services.notice
{
    public interface INoticeService
    {
        /// <summary>
        /// 获取分页版的评论(或回复)通知
        /// </summary>
        /// <param name="page">当前请求页数</param>
        /// <param name="number">每页通知的数量</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        List<Notice> GetItemCommentAndReplyNotice(int page, int number, string userid);


        /// <summary>
        /// 获取分页版的评论(或回复)通知
        /// </summary>
        /// <param name="page">当前请求页数</param>
        /// <param name="number">每页通知的数量</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        List<Notice> GetItemCommentAndReplyNotice(int page, int number, string userid, SortOrder sort);

        /// <summary>
        /// 获取用户的评论(或回复)通知的数量
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        int GetCommentAndReplyNoticeCount(string userid);

        /// <summary>
        /// 获取分页版的关注通知
        /// </summary>
        /// <param name="page">当前请求页数</param>
        /// <param name="number">每页通知的数量</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        List<Notice> GetItemConcernNotice(int page, int number, string userid);
        /// <summary>
        /// 获取分页版的关注通知
        /// </summary>
        /// <param name="page">当前请求页数</param>
        /// <param name="number">每页通知的数量</param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        List<Notice> GetItemConcernNotice(int page, int number, string userid, SortOrder sort);

        /// <summary>
        /// 获取用户关注通知的数量
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        int GetConcernNoticeCount(string userid);


    }
}
