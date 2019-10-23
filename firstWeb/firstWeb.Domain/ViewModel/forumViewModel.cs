using firstWeb.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstWeb.Domain.ViewModel
{
    public class forumViewModel
    {
        public string forum_ID { get; set; }

        public string forum_Title { get; set; }

        public string forum_Content { get; set; }

        public int forum_Category { get; set; }

        public string forum_author_Id { get; set; }

        public string forum_author_Name { get; set; }

        public string forum_author_HeaderAddress { get; set; }

        public DateTime forum_CreateTime { get; set;}

        public int Reading_volume { get; set; }

    }
}
