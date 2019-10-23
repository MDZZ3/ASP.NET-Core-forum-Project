using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace firstWeb.Domain.Model
{
   [Table("account")]
   public class User
   {
        [Key]
         public string ID { get; set; }

        public string Nickname { get; set; }

        public string Phone { get; set; }

        public string PassWord { get; set; }

        public string HeadAddress { get; set; }

        public DateTime Last_Login_Time { get; set; }

        public DateTime Login_Time { get; set; }

        public DateTime Register_Time { get; set; }

        public int Forum_Count { get; set; }

        public int Comment_Count { get; set; }

        public int Reply_Count { get; set; }

        public int Concern { get; set; }

        public int Fans { get; set; }

        public virtual ICollection<Forum> forums { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

   }
}
