using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class AddCommentCountHandler : IEvenHandler<CommentSubmitEven>
    {
        private readonly IRepository<Forum> _forum;

        public AddCommentCountHandler(IRepository<Forum> repository)
        {
            _forum = repository;
        }

        public void Run(CommentSubmitEven comment)
        {
            var forum = _forum.Table.FirstOrDefault(c => c.ID == comment.forumID);
            forum.EvaluateCount += 1;
        }
    }
}
