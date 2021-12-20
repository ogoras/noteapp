using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    public interface IUpdateable<T>
    {
        public void updateValues(T t);
    }
}
