using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Even
{
    public class EvenBase
    {
        public EvenBase()
        {
            Create_Time = DateTime.Now;
        }

        protected DateTime Create_Time { get; set; }
    }
}
