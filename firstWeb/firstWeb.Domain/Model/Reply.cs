using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace firstWeb.Domain.Model
{
    [Table("reply")]
    public class Reply
    {
        public string ID { get; set; }

        public string UserID { get; set; }

        [NotMapped]
        public string reply_NickName { get; set; }

        [NotMapped]
        public string reply_HeaderAddress { get; set; }

        public string Reply_Content { get; set; }

        public string Original_Evaluate_ID { get; set; }

        public DateTime Add_Time { get; set; }

        public string Super_Evaluate_ID { get; set; }

        public string Super_Evaluate_Name { get; set; }

        public string Original_Forum_ID { get; set; }

    }
}
