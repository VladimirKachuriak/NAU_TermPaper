using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL
{
    public class XMLProvider<T> : IDataProvider<T>
    {
        public T read(string connection)
        {
            T data = default;
            using (FileStream fs = new FileStream(connection, FileMode.OpenOrCreate))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                try
                {
                    data = (T)formatter.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                return data;
            }
            
        }

        public void write(T data, string connection)
        {
            using (FileStream fs = new FileStream(connection, FileMode.Create))
            {
                XmlSerializer formatter = new XmlSerializer(data.GetType());
                try
                {
                    formatter.Serialize(fs, data);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
