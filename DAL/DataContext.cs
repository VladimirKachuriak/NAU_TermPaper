using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DataContext<T> : IDataContext<T>
    {
        public string ConnectionString { get; set; }

        public IDataProvider<T> DataProvider { get; set; }

        public DataContext(string connection)
        {
            ConnectionString = connection;
        }

        public T GetData()
        {
           
            return DataProvider.read(ConnectionString);   
           
        }

        public void SetData(T data)
        {
            DataProvider.write(data, ConnectionString);
        }
    }
}
