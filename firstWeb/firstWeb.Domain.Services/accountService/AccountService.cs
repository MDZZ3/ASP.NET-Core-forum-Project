using firstWeb.Domain.Model;
using firstWeb.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace firstWeb.Domain.Services.accountService
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<User> _user;

        public AccountService(IRepository<User> user)
        {
            _user = user;
        }

        public User GetAcccount(string id)
        {
            return _user.Table.FirstOrDefault(u => u.ID == id);
        }

        public string GetNickName(string userid)
        {
            return _user.Table.Select(u => new { Nickname = u.Nickname, ID = u.ID }).FirstOrDefault(u => u.ID == userid).Nickname;
        }

        public string GetUserID(string phone)
        {          
            return _user.Table.Select(u => new { phone = u.Phone, ID = u.ID }).FirstOrDefault(u => u.phone == phone).ID;
        }

        public bool IsExist(string id)
        {
            return _user.Table.Select(u => u.ID).FirstOrDefault(u => u == id) == null ? false : true;
        }
    }
}
