using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Even
{
    /// <summary>
    /// 回复评价的时候，通知被回复的人
    /// </summary>
    public class Reply_NoticeHandler : IEvenHandler<ReplySumbitEven>
    {
        private readonly IRepository<Comment> _commentRepository;

        private readonly IRepository<Forum> _forumRepository;

        private readonly IRepository<Notice> _noticeRepository;

        public Reply_NoticeHandler(IRepository<Comment> comment,IRepository<Forum> forum,IRepository<Notice> notice)
        {
            _commentRepository = comment;
            _forumRepository = forum;
            _noticeRepository = notice;
        }

        public void Run(ReplySumbitEven value)
        {
            
            var forum = _forumRepository.Table.Select(f => new { forum_title = f.Title, id = f.ID }).FirstOrDefault(f => f.id == value.forumID);

            var comment = _commentRepository.Table.Select(f => new { comment_id = f.ID, user_id = f.UserID,locationpage=f.LocationPage }).FirstOrDefault(c => c.comment_id == value.commentId);

            Notice notice = new Notice()
            {
                PublisherID = value.publisherId,
                SubscriberID = comment.user_id,
                Time = DateTime.Now,
                category = value.category,
                forum_title = forum.forum_title,
                forum_link = $"/forum/{forum.id}?p={comment.locationpage}#comment_list",
                state = 0,
            };

            _noticeRepository.Table.Add(notice);

            _noticeRepository._db.SaveChanges();

        }
    }
}
