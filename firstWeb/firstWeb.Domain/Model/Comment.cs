using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace firstWeb.Domain.Model
{
    [Table("Comment")]
    public class Comment
    {
        public string ID { get; set; }

        public string UserID { get; set; }

        public string forumID { get; set; }
       
        public string Content { get; set; }

        public DateTime Create_Time { get; set; }

        public int reply_Count { get; set; }

        public int LocationPage { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual User User { get; set; }

        
        

    }
}
