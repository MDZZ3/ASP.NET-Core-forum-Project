using System;
using System.Collections.Generic;
using System.Text;
using firstWeb.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace firstWeb.Domain.Repositories
{
    public class forumRepository<T> : IRepository<T> where T:class
    {
        
        public DbContext _db { get;}
        
        public forumRepository(forumDbContext db)
        {
            _db = db;
        }

        public DbSet<T> Table
        {
            get
            {
                return this.Entity;
            }
        }
        private DbSet<T> Entity
        {
            get
            {
                return _db.Set<T>();
            }
        }


        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            try
            {
                _db.SaveChanges();
            }catch(Exception ex)
            {
                throw ex;
            }
           
        }
        
    }
}
