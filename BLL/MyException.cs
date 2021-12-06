using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MyException : Exception
    {
        public MyException(string e):base(e) {}
    }
}
