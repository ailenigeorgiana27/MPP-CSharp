
using Model;
using Persistance.utils;
using log4net;
using log4net.Config;
using System.Data;


namespace Persistance.database
{
    public class PersoanaOficiuDbRepo(IDictionary<string, string> dbConnection) : IPersoanaOficiuRepo
    {
        private static readonly ILog Log = LogManager.GetLogger("PersoanaOficiuDbRepo");



        public PersoanaOficiu? FindByCredentials(string username)
        { 
            Log.InfoFormat("Entering findByUsername with value {0}", username);
            IDbConnection con = DBUtils.getConnection(dbConnection);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from PersoanaOficiu where username=@username ";
                IDbDataParameter paramUsername = comm.CreateParameter();
                paramUsername.ParameterName = "@username";
                paramUsername.Value = username;
                comm.Parameters.Add(paramUsername);
                

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        long id = dataR.GetInt64(0);
                        string user = dataR.GetString(1);
                        string pass = dataR.GetString(2);
                        PersoanaOficiu oficiu = new PersoanaOficiu( id, user, pass);
                        
                        Log.InfoFormat("Exiting findByUsername with value {0}", oficiu);
                        return oficiu;
                    }
                }
            }
            Log.InfoFormat("Exiting findByUsername with value {0}", null);
            return null;
        }

        public PersoanaOficiu? Save(PersoanaOficiu entity)
        {
            return null;
        }

        public PersoanaOficiu? Delete(long id)
        {
            return null;
        }

        public PersoanaOficiu? Update(PersoanaOficiu entity)
        {
            return null;
        }


        
        


        public PersoanaOficiu? FindOne(long id)
        {
            Log.InfoFormat("Entering FindOne with value {0}", id);
            IDbConnection con = DBUtils.getConnection(dbConnection);

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from PersoanaOficiu where id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        long oficiuId = dataR.GetInt64(0);
                        string user = dataR.GetString(1);
                        string password = dataR.GetString(2);
                        PersoanaOficiu oficiu = new PersoanaOficiu( oficiuId, user, password);
                        Log.InfoFormat("Exiting FindOne with value {0}", oficiu);
                        return oficiu;
                    }
                }
            }
            Log.InfoFormat("Exiting FindOne with value {0}", null);
            return null;
        }


        public List<PersoanaOficiu> FindAll()
        {
            Log.Info("Entering FindAll");
            IDbConnection con = DBUtils.getConnection(dbConnection);
            List<PersoanaOficiu> oficii = new List<PersoanaOficiu>();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "select * from PersoanaOficiu";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        long id = dataR.GetInt64(0);
                        string user = dataR.GetString(1);
                        string password = dataR.GetString(2);
                        PersoanaOficiu oficiu = new PersoanaOficiu(id, user, password);
                        oficii.Add(oficiu);
                    }
                }
            }
            Log.InfoFormat("Exiting FindAll with {0} results", oficii.Count);
            return oficii;
        }
        

    }
}
