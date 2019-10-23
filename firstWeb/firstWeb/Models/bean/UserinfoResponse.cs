using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstWeb.Models.bean
{
    public class UserinfoResponse
    {
        public string state { get; set; }

        public string Message { get; set; }

        public Userinfo data { get;set; }
    }
}
