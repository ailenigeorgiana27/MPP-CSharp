using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public interface IPersoanaOficiuRepo : IRepo<long, PersoanaOficiu>
    {
        PersoanaOficiu? FindByCredentials(string username);
    }
}
