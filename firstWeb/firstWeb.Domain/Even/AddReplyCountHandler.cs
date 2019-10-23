using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class AddReplyCountHandler : IEvenHandler<ReplySumbitEven>
    {
        private readonly IRepository<Comment> _comment;
        
        public AddReplyCountHandler(IRepository<Comment> comment)
        {
            _comment = comment;
        }

        public void Run(ReplySumbitEven reply)
        {
           var comment= _comment.Table.FirstOrDefault(c => c.ID == reply.commentId);
            comment.reply_Count += 1;
        }
    }
}
