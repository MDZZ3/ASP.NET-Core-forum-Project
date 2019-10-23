using forum.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace forum.Domain.Services
{
   public interface IAccountServices
    {
        User GetAccount(string Username, string password);

        User GetUserInfo(string Username);

        string GetUserID(string phone);

        bool isExist(string phone);

        void RegisterUser(User user);
    }
}
