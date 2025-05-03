using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public interface IInscriereRepo : IRepo<long, Inscriere>
    {
        int FindNoOfParticipants(long proba);
        int FindNoOfProbeDupaParticipanti(long participant);

        List<Participant> FindParticipantsByProba(long proba);
    }
}
