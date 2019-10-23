using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class NoticeEven:EvenBase
    {
        public string forumID { get; set; }

        public string publisherId { get; set; }

        public string SubscriberId { get; set; }

        public int category { get; set; }

    }
}
