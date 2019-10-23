using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace forum.DAL.Repository
{
   public interface IRepository<T>  where T:class
    {
        DbContext _context { get; }

        DbSet<T> Table { get; }
        void Insert(T entity);
        void Updata(T entity);
        void Delete(T entity);
        void Find(T entity);
    }
}
