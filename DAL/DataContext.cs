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

        public DataContext()
        {
        }

        public T GetData()
        {
            if (DataProvider == null) throw new InvalidOperationException("Data provider not set"); 
            if (ConnectionString == null) throw new InvalidOperationException("connection not set"); 
            return DataProvider.read(ConnectionString);   
           
        }

        public void SetData(T data)
        {
            if (DataProvider == null) throw new InvalidOperationException("Data provider not set");
            if (ConnectionString == null) throw new InvalidOperationException("connection not set");
            DataProvider.write(data, ConnectionString);
        }
    }
}
