using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Model
{
    public class forumDbContext:DbContext
    {
        public forumDbContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<User> users { get; set; }

        public DbSet<Forum> forums { get; set; }

        public DbSet<forumCategory> forumCategories { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Notice> notices { get; set; }

        public DbSet<Comment> comments { get; set; }

        public DbSet<Contern> contern { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contern>()
                 .HasKey(c => new { c.ConternAccountID, c.FansAccountID });
        }
    }
}
