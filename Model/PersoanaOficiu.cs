using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class PersoanaOficiu : Entity<long>
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public PersoanaOficiu(long id, String username, String password):base(id)
        {
            Username = username;
            Password = password;
        }

        public override String ToString()
            => $"PersoanaOficiu {{user= {Username}, pass={Password} }}";
    }
}
