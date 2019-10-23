using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace firstWeb.Domain.Model
{
    [Table("Notice")]
    public class Notice
    {
        public int ID { get; set; }

        public string PublisherID { get; set; }

        public string SubscriberID { get; set; }

        public DateTime Time { get; set; }

        public string forum_title { get; set; }

        public string forum_link { get; set; }

        public int state { get; set; }

        public int category { get; set; }


    }
}
