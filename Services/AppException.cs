using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(String msg) : base(msg) { }

        public AppException(String msg, Exception ex) : base(msg, ex) { }
    }
}
