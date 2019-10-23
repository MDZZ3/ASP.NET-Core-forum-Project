using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Receive
{
    public class HttpReceive
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public UserReceive data { get; set; }
    }
}
