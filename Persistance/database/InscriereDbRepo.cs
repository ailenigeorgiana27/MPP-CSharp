using Model;
using Persistance;
using Persistance.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using log4net;

namespace Persistence.database
{
    public class InscriereDbRepo : IInscriereRepo
    {
        private static readonly ILog Log = LogManager.GetLogger("InscriereDbRepo");
        private readonly IParticipantRepo _participantRepo;
        private readonly IProbaRepo _probaRepo;
        private readonly IDictionary<string, string> _dbConnection;

        public InscriereDbRepo(IDictionary<string, string> dbConnection, IParticipantRepo participantRepo, IProbaRepo probaRepo)
        {
            _dbConnection = dbConnection;
            _participantRepo = participantRepo;
            _probaRepo = probaRepo;
        }
        public int FindNoOfParticipants(long proba)
        {
            Log.InfoFormat("Entering FindNoOfParticipanti with idProba {0}", proba);
            IDbConnection con = DBUtils.getConnection(_dbConnection);
            int number = 0;

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT COUNT(*) FROM Inscriere WHERE proba = @proba";
                IDbDataParameter param = comm.CreateParameter();
                param.ParameterName = "@proba";
                param.Value = proba;
                comm.Parameters.Add(param);

                number = Convert.ToInt32(comm.ExecuteScalar());
            }

            Log.InfoFormat("Exiting FindNoOfParticipanti with result {0}", number);
            return number;
        }
        
        public int FindNoOfProbeDupaParticipanti(long participant)
        {
            Log.InfoFormat("Entering FindNoOfProbeDupaParticipanti with idParticipant {0}", participant);
            IDbConnection con = DBUtils.getConnection(_dbConnection);
            int number = 0;

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT COUNT(*) FROM Inscriere WHERE participant = @participant";
                IDbDataParameter param = comm.CreateParameter();
                param.ParameterName = "@participant";
                param.Value = participant;
                comm.Parameters.Add(param);

                number = Convert.ToInt32(comm.ExecuteScalar());
            }

            Log.InfoFormat("Exiting FindNoOfProbeDupaParticipanti with result {0}", number);
            return number;
        }
        public Inscriere? FindOne(long id)
        {
            return null;
        }
        
        public List<Inscriere> FindAll()
    {
        Log.Info("Entering FindAll");
        List<Inscriere> entities = new List<Inscriere>();
        IDbConnection con = DBUtils.getConnection(_dbConnection);

        using (var comm = con.CreateCommand())
        {
            comm.CommandText = "SELECT * FROM Inscriere";
            using (var dataR = comm.ExecuteReader())
            {
                while (dataR.Read())
                {
                    long id = dataR.GetInt64(0);
                    int idParticipant = dataR.GetInt32(2);
                    int idProba = dataR.GetInt32(1);

                    Participant? participant = _participantRepo.FindOne(idParticipant);
                    Proba? proba = _probaRepo.FindOne(idProba);

                    if (participant != null && proba != null)
                    {
                        entities.Add(new Inscriere(id,proba,participant));
                    }
                }
            }
        }
        Log.InfoFormat("Exiting FindAll with {0} results", entities.Count);
        return entities;
    }

    public Inscriere? Save(Inscriere entity)
    {
        Log.InfoFormat("Entering Save with value {0}", entity);
        IDbConnection con = DBUtils.getConnection(_dbConnection);

        using (var comm = con.CreateCommand())
        {
            comm.CommandText = "INSERT INTO Inscriere (proba, participant) VALUES (@proba,@participant)";
            IDbDataParameter paramParticipant = comm.CreateParameter();
            paramParticipant.ParameterName = "@participant";
            paramParticipant.Value = entity.Participant.Id;
            comm.Parameters.Add(paramParticipant);

            IDbDataParameter paramProba = comm.CreateParameter();
            paramProba.ParameterName = "@proba";
            paramProba.Value = entity.Proba.Id;
            comm.Parameters.Add(paramProba);

            int result = comm.ExecuteNonQuery();
            if (result > 0)
            {
                Log.InfoFormat("Exiting Save with value {0}", entity);
                return entity;
            }
        }
        Log.Info("Exiting Save with null");
        return null;
    }

    public List<Participant> FindParticipantsByProba(long proba)
    {
        Log.InfoFormat("Entering FindParticipantsByProba with idProba {0}", proba);
        List<Participant> participants = new List<Participant>();
        IDbConnection con = DBUtils.getConnection(_dbConnection);
        using (var comm = con.CreateCommand())
        {
            comm.CommandText = "SELECT participant FROM Inscriere WHERE proba = @proba";
            IDbDataParameter param = comm.CreateParameter();
            param.ParameterName = "@proba";
            param.Value = proba;
            comm.Parameters.Add(param);
            using (var dataR = comm.ExecuteReader())
            {
                while (dataR.Read())
                {
                    long idParticipant = dataR.GetInt64(0);
                    Participant? participant = _participantRepo.FindOne(idParticipant);
                    if (participant != null)
                    {
                        participants.Add(participant);
                    }
                }
            }
        }
        Log.InfoFormat("Exiting FindParticipantsByProba with {0} results", participants.Count);
        return participants;
    }

    public Inscriere? Delete(long id)
    {
        return null;
    }

    public Inscriere Update(Inscriere entity)
    {
        return null;
    }
    }
}
