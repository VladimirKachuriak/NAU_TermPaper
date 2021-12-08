using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Serializable]
    public class Book
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Data { get; set; }
        public int  ID { get; set; }
        public bool Exist_status { get; set; }
        public DateTime DeadLine{ get; set; }

    }
}
