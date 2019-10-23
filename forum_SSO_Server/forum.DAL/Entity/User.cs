using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace forum.DAL.Entity
{
    [Table("Account")]
    public class User
    {
        
        public string ID { get; set; }

        public string Nickname { get; set; }

        public string PassWord { get; set; }
        [Key]
        public string Phone { get; set; }

        public string HeadAddress { get; set; }

        public DateTime Last_Login_Time { get; set; }

        public DateTime Login_Time { get; set; }

        public DateTime Register_Time { get; set; }

    }
}
