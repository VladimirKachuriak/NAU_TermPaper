using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDataContext<T>
    {
        string ConnectionString { get; set; }
        IDataProvider<T> DataProvider { get; set; }
        T GetData();
        void SetData(T data);
    }
}
