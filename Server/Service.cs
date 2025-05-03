using Model;
using Persistance;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Server
{
    public class Service : IServices
    {
        private readonly IPersoanaOficiuRepo _persoanaOficiuRepo;
        private readonly IParticipantRepo _participantRepo;
        private readonly IProbaRepo _probaRepo;
        private readonly IInscriereRepo _inscriereRepo;
        private readonly Dictionary<string, IObserver> _loggedClients;

        public Service(IPersoanaOficiuRepo persoanaOficiuRepo, IParticipantRepo participantRepo, IProbaRepo probaRepo,
            IInscriereRepo inscriereRepo)
        {
            _persoanaOficiuRepo = persoanaOficiuRepo;
            _participantRepo = participantRepo;
            _probaRepo = probaRepo;
            _inscriereRepo = inscriereRepo;
            _loggedClients = new Dictionary<string, IObserver>();
        }

        public void login(string username, string password, IObserver client)
        {
            PersoanaOficiu? of = _persoanaOficiuRepo.FindByCredentials(username);
            if (of == null)
            {
                throw new ServiceException("Nu exista oficiu cu acest nume");
            }

            if (of.Password != password)
            {
                throw new ServiceException("Parola gresita");
            }

            if (_loggedClients.ContainsKey(username))
            {
                throw new ServiceException("Oficiul este deja logat");
            }

            _loggedClients[username] = client;


        }

        public void logout(string username, IObserver client)
        {
            if (!_loggedClients.ContainsKey(username))
            {
                throw new ServiceException("Oficiul nu este logat");
            }

            _loggedClients.Remove(username);

        }

        public void inscriereParticipant(Participant participant, long[] probaIds)
        {
            if (probaIds.Length == 0)
            {
                throw new ServiceException("Participantul nu a fost inscris la nicio proba!");
            }

            if (_participantRepo.Save(participant) != null)
            {
                long id = _participantRepo.GetMaxIdParticipant();
                participant.Id = id;
            }
            else
            {
                throw new ServiceException("Participantul este deja inscris!");
            }

            foreach (var probaId in probaIds)
            {
                Proba? proba = _probaRepo.FindOne(probaId);
                if (proba == null)
                {
                    throw new ServiceException($"Proba cu id-ul {probaId} nu exista!");
                }

                _inscriereRepo.Save(new Inscriere( 0,proba,participant));
            }

            NotifyAllClients(participant, probaIds);
        }

        private void NotifyAllClients(Participant participant, long[] probaIds)
        {
            foreach (var client in _loggedClients.Values)
            {
                try
                {
                    Task.Run(() => client.Update(participant, probaIds));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }



        public Dictionary<Participant, int> getParticipantsByProba(int distance, String style)
        {
            var proba = _probaRepo.FindProbaByStyleDistance(style, distance)
                        ?? throw new ServiceException("Proba incorecta!");

            var participanti = _inscriereRepo.FindParticipantsByProba(proba.Id);

            Dictionary<Participant, int> result = new Dictionary<Participant, int>();
            foreach (var participant in participanti)
            {
                if (!result.ContainsKey(participant))
                {
                    result[participant] = 0;
                }

                result[participant]++;
            }

            return result;
        }


        public int GetNrProbeParticipant(long idParticipant)
        {
            return _inscriereRepo.FindNoOfProbeDupaParticipanti(idParticipant);
        }

        public List<Proba> GetAllProbe()
        {
            return _probaRepo.FindAll();
        }


        public List<Participant> GetParticipantsByProba(string style, int distance)
        {
            var proba = _probaRepo.FindProbaByStyleDistance(style, distance)
                        ?? throw new ServiceException("Proba incorecta!");

            var participanti = _inscriereRepo.FindParticipantsByProba(proba.Id);

            return participanti;
        }


        public Dictionary<Proba, Int32> getAllProbe()
        {
            List<Proba> probe = _probaRepo.FindAll();
            Dictionary<Proba, Int32> result = new Dictionary<Proba, Int32>();
            foreach (var proba in probe)
            {
                result[proba] = _inscriereRepo.FindNoOfParticipants(proba.Id);
            }

            return result;
        }
    }
}
