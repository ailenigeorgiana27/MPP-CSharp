using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IServices
    {
        void login(string username, string password, IObserver client);
    
        void logout(string username, IObserver client);
    
        void inscriereParticipant(Participant participant, Int64[] probaIds);

        Dictionary<Participant, Int32> getParticipantsByProba(Int32 distance, String style);

        Dictionary<Proba, Int32> getAllProbe();
    }
}
