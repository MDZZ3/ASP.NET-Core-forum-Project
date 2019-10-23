using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class Concern_NoticeHandler : IEvenHandler<ConcernSumbitEven>
    {
        private readonly IRepository<Notice> _noticerepository;

        public Concern_NoticeHandler(IRepository<Notice> notice)
        {
            _noticerepository = notice;
        }

        public void Run(ConcernSumbitEven value)
        {
            Notice notice = new Notice()
            {
                category = value.category,
                forum_title = null,
                forum_link = null,
                PublisherID = value.publisherId,
                SubscriberID = value.SubscriberId,
                state = 0,
                Time = DateTime.Now
            };
            _noticerepository.Table.Add(notice);
            _noticerepository._db.SaveChanges();
        }

    }
}
