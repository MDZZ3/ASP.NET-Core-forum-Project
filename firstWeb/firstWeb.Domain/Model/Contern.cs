using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace firstWeb.Domain.Model
{
    [Table("contern")]
    public class Contern
    {
        public string ID { get; set; }

        
        public string FansAccountID { get; set; }

        
        public string ConternAccountID { get; set; }
    }
}
