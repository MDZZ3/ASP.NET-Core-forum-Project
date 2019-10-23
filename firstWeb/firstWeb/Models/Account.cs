using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace firstWeb.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public string UserID { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public string PassWord { get; set; }

        public string Telephone { get; set; }

        public string Sex { get; set; }

        public double Credit { get; set; } = 100;

        public int RoleID { get; set; }
        
        public int IsRealname { get; set; }

       // //个人简历
       // public virtual CustomerResume CustomerResume { get; set; }

       // //实名制
       // public virtual Realname Realname { get; set; }

       // //个人收藏
       //public virtual ICollection<CustomerCollection> CustomerCollections { get; set; }

       // //对应的工作
       // public virtual ICollection<Job> Jobs { get; set; }

    }
}