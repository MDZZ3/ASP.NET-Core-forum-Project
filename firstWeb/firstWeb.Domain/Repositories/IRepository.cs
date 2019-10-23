using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Repositories
{
    public interface IRepository<T> where T:class
    {
        DbContext _db { get; }

        DbSet<T> Table { get;}

        void Add(T entity);

        void Delete(T entity);

        void Insert(T entity);

        void SaveChanges();



    }
}
