
using Model;
using Persistance.utils;
using Persistance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using log4net;

namespace Persistence.database
{
    public class ParticipantDbRepo(IDictionary<string, string> dbConnection) : IParticipantRepo
    {
        private static readonly ILog Log = LogManager.GetLogger("ParticipantDbRepo");

       

        public Participant? Save(Participant entity)
        {
            Log.InfoFormat("Entering Save with value {0}", entity);
            IDbConnection con = DBUtils.getConnection(dbConnection);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "insert into Participant (name, age) values (@name, @age)";
                IDbDataParameter paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = entity.Name;
                comm.Parameters.Add(paramName);

                IDbDataParameter paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@age";
                paramPassword.Value = entity.Age;
                comm.Parameters.Add(paramPassword);

                var result = comm.ExecuteNonQuery();
                if (result > 0)
                {
                    Log.InfoFormat("Exiting Save with value {0}", entity);
                    return entity;
                }
            }
            Log.InfoFormat("Exiting Save with value {0}", null);
            return null;
        }

        public long GetMaxIdParticipant()
        {
            Log.Info("Entering GetMaxIdParticipant");
            IDbConnection con = DBUtils.getConnection(dbConnection);
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select max(id) from Participant";
                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        long id = dataR.GetInt64(0);
                        Log.InfoFormat("Exiting GetMaxIdParticipant with value {0}", id);
                        return id;
                    }
                }
            }
            Log.InfoFormat("Exiting GetMaxIdParticipant with value {0}", 0);
            return 0;
        }
        public Participant? Delete(long id)
        {
            return null;
        }

        public Participant? Update(Participant entity)
        {
            return null;
        }

        public Participant? FindOne(long id)
        {
            Log.InfoFormat("Entering FindOne with value {0}", id);
            IDbConnection con = DBUtils.getConnection(dbConnection);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from Participant where id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        long participantId = dataR.GetInt64(0);
                        string name = dataR.GetString(1);
                        int age = dataR.GetInt32(2);
                        Participant participant = new Participant( participantId,name, age);
                        Log.InfoFormat("Exiting FindOne with value {0}", participant);
                        return participant;
                    }
                }
            }
            Log.InfoFormat("Exiting FindOne with value {0}", null);
            return null;
        }

        public List<Participant> FindAll()
        {
            return null;
        }

        public IEnumerable<Participant> FilterByName(string name)
        {
            Log.Info($"Filtering Participants by name {name}");

            var participants = new List<Participant>();

            using var connection = DBUtils.getConnection(dbConnection);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM \"Participant\" WHERE \"name\" = @name";

            var nameParam = command.CreateParameter();
            nameParam.ParameterName = "@name";
            nameParam.Value = name;
            command.Parameters.Add(nameParam);

            try
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(reader.GetOrdinal("id"));
                    var participantName = reader.GetString(reader.GetOrdinal("name"));
                    var age = reader.GetInt32(reader.GetOrdinal("age"));
                    var participant = new Participant(id,participantName, age) { Id = id };
                    participants.Add(participant);
                }
            }
            catch (Exception ex)
            {
                Log.Warn("Error filtering Participants by name");
            }

            return participants;
        }

        public IEnumerable<Participant> FilterByAge(int age)
        {
            Log.Info($"Filtering Participants by age {age}");

            var participants = new List<Participant>();

            using var connection = DBUtils.getConnection(dbConnection);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM \"Participant\" WHERE \"age\" = @age";

            var ageParam = command.CreateParameter();
            ageParam.ParameterName = "@age";
            ageParam.Value = age;
            command.Parameters.Add(ageParam);

            try
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(reader.GetOrdinal("id"));
                    var name = reader.GetString(reader.GetOrdinal("name"));
                    var participantAge = reader.GetInt32(reader.GetOrdinal("age"));
                    var participant = new Participant(id,name, participantAge) { Id = id };
                    participants.Add(participant);
                }
            }
            catch (Exception ex)
            {
                Log.Warn("Error filtering Participants by age");
            }

            return participants;
        }
    }
}
