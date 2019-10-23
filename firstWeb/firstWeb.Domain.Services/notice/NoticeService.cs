using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using firstWeb.Domain.Enums;
using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace firstWeb.Domain.Services.notice
{
    public class NoticeService : INoticeService
    {
        private readonly IRepository<Notice> _noticeRepository;

        public NoticeService(IRepository<Notice> notice)
        {
            _noticeRepository = notice;
        }

        public int GetCommentAndReplyNoticeCount(string userid)
        {
           return _noticeRepository.Table.Where(n => n.SubscriberID == userid&&(n.category==1||n.category==2)).AsNoTracking().Count();
        }

        public int GetConcernNoticeCount(string userid)
        {
            return _noticeRepository.Table.Where(n => n.SubscriberID == userid && n.category == 3).AsNoTracking().Count();
        }

        public List<Notice> GetItemCommentAndReplyNotice(int page,int number,string userid)
        {
            return GetItemCommentAndReplyNotice(page, number, userid, SortOrder.Descending_order);
        }

        public List<Notice> GetItemCommentAndReplyNotice(int page,int number,string userid,SortOrder sort)
        {
            int skipNumber = (page - 1) * number;

            if (sort == SortOrder.Ascending_order)
            {
               return _noticeRepository.Table.Where(n => n.SubscriberID == userid && (n.category == 1 || n.category == 2)).Skip(skipNumber).Take(number).AsNoTracking().OrderBy(n=>n.Time).ToList();
            }
            else
            {
                return _noticeRepository.Table.Where(n => n.SubscriberID == userid && (n.category == 1 || n.category == 2)).Skip(skipNumber).Take(number).AsNoTracking().OrderByDescending(n => n.Time).ToList();
            }
        }

        public List<Notice> GetItemConcernNotice(int page, int number, string userid)
        {
            return GetItemConcernNotice(page, number, userid, SortOrder.Descending_order);
        }

        public List<Notice> GetItemConcernNotice(int page, int number, string userid, SortOrder sort)
        {
            int skipNumber = (page - 1) * number;

            if (sort == SortOrder.Ascending_order)
            {
                return _noticeRepository.Table.Where(n => n.SubscriberID == userid &&n.category==3).Skip(skipNumber).Take(number).AsNoTracking().OrderBy(n => n.Time).ToList();
            }
            else
            {
                return _noticeRepository.Table.Where(n => n.SubscriberID == userid && n.category == 3).Skip(skipNumber).Take(number).AsNoTracking().OrderByDescending(n => n.Time).ToList();
            }
        }
    }
}
