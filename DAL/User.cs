using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   
    public class User
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Academicgroup { get; set; }
        public int Id { get; set; }
        public List<int> Shelf{ get; set; }

    }
}
