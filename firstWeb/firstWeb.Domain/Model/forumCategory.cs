using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace firstWeb.Domain.Model
{
    [Table("forumCategory")]
    public class forumCategory
    {
        public int ID { get; set; }

        public string Category { get; set; }
    }
}
