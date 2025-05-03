
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Proba : Entity<long>
    {
        public int Distanta { get; set; }
        public String Stil { get; set; }

        

        public Proba(long id, int distanta, String stil):base(id)
        {
            Distanta = distanta;
            Stil = stil;
        }

        public override string ToString()
            => $"Proba: {{dist = {Distanta}, stil = {Stil}}}";
    }
}
