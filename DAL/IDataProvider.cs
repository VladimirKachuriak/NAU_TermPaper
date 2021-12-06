using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDataProvider<T>
    {
        void write(T data, string connection);
        T read(string connection);
    }
}
