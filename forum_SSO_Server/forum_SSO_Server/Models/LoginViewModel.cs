using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace forum_SSO_Server.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public bool Status { get; set; }
        public string returnUri { get; set; }

    }
}
