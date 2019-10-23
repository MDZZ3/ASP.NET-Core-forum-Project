using System;
using System.Collections.Generic;
using System.Text;

namespace firstWeb.Domain.Even
{
    public interface IEvenHandler<T> where T:EvenBase
    {
         void Run(T value);
    }
}
