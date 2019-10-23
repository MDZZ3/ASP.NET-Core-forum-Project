using forum.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace forum.DAL.Repository
{
    public class UnitofWork : IDisposable,IUnitOfWork
    {
        private readonly EFContext _context;

        public UnitofWork(EFContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool dispose)
        {  
                if (dispose)
                {
                   _context.Dispose();
                }        
        }

        public void Rollback()
        {
            
        }
    }
}
