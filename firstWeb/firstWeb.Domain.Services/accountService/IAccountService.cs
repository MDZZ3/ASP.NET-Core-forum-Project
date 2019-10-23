using firstWeb.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Services.accountService
{
    public interface IAccountService
    {
        string GetNickName(string userid);

        string GetUserID(string phone);

        User GetAcccount(string id);

        bool IsExist(string id);
    }
}
