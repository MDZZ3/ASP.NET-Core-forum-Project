using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Even
{
    /// <summary>
    /// 评价帖子的时候，回复发帖子的人
    /// </summary>
    public class Comment_NoticeHandler : IEvenHandler<CommentSubmitEven>
    {
        private readonly IRepository<Forum> _forumrepository;

        private readonly IRepository<Comment> _commentRepository;

        private readonly IRepository<Notice> _noticeRepository;

        public Comment_NoticeHandler(IRepository<Forum> forum,IRepository<Comment> comment,IRepository<Notice> notice)
        {
            _forumrepository = forum;
            _commentRepository = comment;
            _noticeRepository = notice;
        }

        public void Run(CommentSubmitEven value)
        {
            var forum = _forumrepository.Table.FirstOrDefault(f => f.ID == value.forumID);
            var comment = _commentRepository.Table.FirstOrDefault(c => c.ID == value.Comment_Id);
            Notice notice = new Notice()
            {
                category = value.category,
                forum_title = forum.Title,
                Time = DateTime.Now,
                PublisherID = value.publisherId,
                SubscriberID = value.SubscriberId,
                forum_link = $"/forum/{value.forumID}?p={comment.LocationPage}#comment_list",
                state = 0
            };

            _noticeRepository._db.Add(notice);
            _noticeRepository._db.SaveChanges();

        }
    }
}
