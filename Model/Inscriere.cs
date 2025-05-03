using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Inscriere : Entity<long>
    {
        public Participant Participant { get; set; }
        public Proba Proba { get; set; }

        public Inscriere(long id,Proba proba, Participant participant):base(id)
        {
            Participant = participant;
            Proba = proba;
        }

        public override string ToString()
            => $"Inscriere {{ participant: {Participant}; proba: {Proba} }}";

    }
}
