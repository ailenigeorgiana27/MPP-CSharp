using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public interface IProbaRepo : IRepo<long, Proba>
    {
        public Proba? FindProbaByStyleDistance(string style, int distance);
    }
}
