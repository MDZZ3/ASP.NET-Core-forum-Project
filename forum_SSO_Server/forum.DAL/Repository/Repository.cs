using forum.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace forum.DAL.Repository
{
   public class Repository<T>:IRepository<T> where T:class
    {
        public  DbContext _context { get; }

        public Repository(EFContext context)
        {
            _context = context;
        }

        private DbSet<T> Entity
        {
            get
            {
                return _context.Set<T>();
            }
        }

        public virtual DbSet<T> Table
        {
            get
            {
                return this.Entity;
            }
        }

       public void Insert(T entity)
       {
            if (entity != null)
            {
                this.Entity.Add(entity);
            }
        }

        public void Delete(T entity)
        {
            if (entity != null)
            {
                this.Entity.Remove(entity);
            }
        }

        public void Updata(T entity)
        {
            if (entity != null)
            {
                this.Entity.Update(entity);
            }
        }

        public void Find(T entity)
        {
            if (entity != null)
            {
                this.Entity.Find();
            }
        }
    }
}
