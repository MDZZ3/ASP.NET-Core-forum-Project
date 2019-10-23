using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class AddForumCountHandler:IEvenHandler<CreateForumSumbitEven>
    {
        private readonly IRepository<User> _user;

        public AddForumCountHandler(IRepository<User> user)
        {
            _user = user;
        }

        public void Run(CreateForumSumbitEven generic)
        {
            var user = _user.Table.FirstOrDefault(f => f.ID == generic.userid);
            user.Forum_Count += 1;
        }
    }
}
