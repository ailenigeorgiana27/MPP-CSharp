using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public interface IParticipantRepo : IRepo<long, Participant>
    {
        
        public long GetMaxIdParticipant();
    }
}
