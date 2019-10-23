using forum.DAL.Entity;
using forum.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace forum.Domain.Services
{
   public class AccountServices: IAccountServices
    {
        private readonly IRepository<User> _user;

        public AccountServices(IRepository<User> repository)
        {
            _user = repository;
        }

        public User GetAccount(string Username,string password)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("参数为空或为null");
            }
            var user = _user.Table;
            return user.FirstOrDefault(u => u.Phone == Username && u.PassWord == password);
        }

        public User GetUserInfo(string Username)
        {
            if (string.IsNullOrEmpty(Username))
            {
                throw new ArgumentException("参数为空或为null");
            }
            return _user.Table.FirstOrDefault(u => u.Phone == Username);
        }

        /// <summary>
        /// 通过电话号码来获取用户的唯一id
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <returns></returns>
        public string GetUserID(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentException("参数为空或为null");
            }
            User user= _user.Table.FirstOrDefault(u => u.Phone == phone);
            if (user == null)
            {
                throw new Exception("找不到用户");
            }
            return user.ID;

        }

        public bool isExist(string phone)
        {
            var account = _user.Table.FirstOrDefault(u => u.Phone == phone);

            return account == null ? false : true;
        }

        public void RegisterUser(User user)
        {
            _user.Table.Add(user);
            _user._context.SaveChanges();
        }
    }
}
