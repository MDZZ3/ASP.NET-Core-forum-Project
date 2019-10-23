using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace firstWeb.Domain.Model
{
    [Table("forum")]
    public class Forum
    {
        [Key]
        public string ID { get; set; } 
        
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public int EvaluateCount { get; set; }

        public int Reading_volume { get; set; }

        public int CategoryId { get; set; }

        public int IsElite { get; set; }

        public DateTime Create_Time { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

    }
}
