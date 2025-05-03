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
    public class ProbaDbRepo(IDictionary<string, string> dbConnection) : IProbaRepo
    {
        private static readonly ILog Log = LogManager.GetLogger("ProbaDbRepo");
        

        public Proba? Save(Proba entity)
        {
            return null;
        }

        public Proba? Delete(long id)
        {
            return null;
        }

        public Proba? Update(Proba entity)
        {
            return null;
        }

        public List<Proba> FindAll()
        {
            Log.Info("Entering FindAll");
            List<Proba> entities = new List<Proba>();
            IDbConnection con = DBUtils.getConnection(dbConnection);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM Proba";
                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long probaId = dataR.GetInt64(0);
                        int distanta = dataR.GetInt32(1);
                        string stil = dataR.GetString(2);
                        entities.Add(new Proba( probaId,distanta, stil));
                    }
                }
            }
            Log.InfoFormat("Exiting FindAll with {0} results", entities.Count);
            return entities;
        }
        

        public Proba? FindOne(long id)
        {
            Log.InfoFormat("Entering FindOne with value {0}", id);
            IDbConnection con = DBUtils.getConnection(dbConnection);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM Proba WHERE id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        long probaId = dataR.GetInt64(0);
                        int distanta = dataR.GetInt32(1);
                        string stil = dataR.GetString(2);
                        Proba proba = new Proba( probaId,distanta, stil);
                        Log.InfoFormat("Exiting FindOne with value {0}", proba);
                        return proba;
                    }
                }
            }
            Log.InfoFormat("Exiting FindOne with value {0}", null);
            return null;
        }

       

        public Proba FindProbaByStyleDistance(string stil, int distance)
        {
            Log.Info("Entering FindProbaByStyleDistance");
            IDbConnection con = DBUtils.getConnection(dbConnection);
            Proba entity = null;
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT * FROM Proba WHERE stil = @stil AND distance = @distance";
                IDbDataParameter paramStyle = comm.CreateParameter();
                paramStyle.ParameterName = "@stil";
                paramStyle.Value = stil;
                comm.Parameters.Add(paramStyle);
                IDbDataParameter paramDistance = comm.CreateParameter();
                paramDistance.ParameterName = "@distance";
                paramDistance.Value = distance;
                comm.Parameters.Add(paramDistance);
                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        long probaId = dataR.GetInt64(0);
                        int distanta = dataR.GetInt32(1);
                        string style = dataR.GetString(2);
                        entity = new Proba(probaId,distanta, stil);
                    }
                }
            }

            return entity;
        }
    }
}
