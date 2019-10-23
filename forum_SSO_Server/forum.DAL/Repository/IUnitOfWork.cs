using System;
using System.Collections.Generic;
using System.Text;

namespace forum.DAL.Repository
{
   public interface IUnitOfWork
    {
        void Commit();
        void Rollback();
    }
}
