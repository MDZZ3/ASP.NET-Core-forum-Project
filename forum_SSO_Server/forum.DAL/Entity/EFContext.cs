using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace forum.DAL.Entity
{
   public class EFContext:DbContext
    {
        public EFContext(DbContextOptions<EFContext> ef) : base(ef)
        {

        }    
        public DbSet<User> Users { get; set; }
    }
}
