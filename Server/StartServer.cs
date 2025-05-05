using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Persistance;
using Persistance.database;
using Services;
using Model;
using Persistence.database;
using System.Configuration;
using System.Data.SQLite;
using System.Reflection;
using log4net;
using log4net.Config;
using Networking;
using Networking.JsonProtocol;


namespace Server
{
    public class StartServer
    {
        private static int DEFAULT_PORT=55556;
        private static String DEFAULT_IP="127.0.0.1";
        private static readonly ILog log = LogManager.GetLogger(typeof(StartServer));
        public static void Main(string[] args)
        {
            
          
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
			
            log.Info("Starting  server");
            log.Info("Reading properties from App.config ...");
            int port = DEFAULT_PORT;
            String ip = DEFAULT_IP;
            String portS= ConfigurationManager.AppSettings["port"];
            if (portS == null)
            {
                log.Debug("Port property not set. Using default value "+DEFAULT_PORT);
            }
            else
            {
                bool result = Int32.TryParse(portS, out port);
                if (!result)
                {
                    log.Debug("Port property not a number. Using default value "+DEFAULT_PORT);
                    port = DEFAULT_PORT;
                    log.Debug("Portul "+port);
                }
            }
            Console.WriteLine("Port property set to "+port);
            String ipS=ConfigurationManager.AppSettings["ip"];
           
            if (ipS == null)
            {
                log.Info("Port property not set. Using default value "+DEFAULT_IP);
            }
           
            else
            {
                ip = ipS;
                log.Debug("IP property set to "+ip);
            }
           
            Console.WriteLine("IP property set to "+ip);
      
            Console.WriteLine("Configuration Settings for concursDB {0}", GetConnectionStringByName("InotDb"));
            IDictionary<String, string> props = new SortedList<String, String>();
            props.Add("ConnectionString", GetConnectionStringByName("InotDb"));
            IPersoanaOficiuRepo employeeRepository = new PersoanaOficiuDbRepo(props);
            IParticipantRepo participantRepo = new ParticipantDbRepo(props);
            IProbaRepo probaRepo = new ProbaDbRepo(props);
            IInscriereRepo inscriereRepo = new InscriereDbRepo(props, participantRepo, probaRepo);

            IServices service =
                new Service(employeeRepository, participantRepo, probaRepo, inscriereRepo);

            log.DebugFormat("Starting server on IP {0} and port {1}", ip, port);
            JsonServer server = new JsonServer(ip,port, service);
            Console.WriteLine("Server created");
            server.Start();
            log.Debug("Server started ...");
            Console.ReadLine();
        }

        static string GetConnectionStringByName(string name)
        {
            string returnValue = null;
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            if (connectionStringSettings != null)
            {
                returnValue = connectionStringSettings.ConnectionString;
            }
            return returnValue;
        }

    }
    public class JsonServer: ConcurrentServer 
    {
        private IServices server;
        private ClientWorker worker;
        private static readonly ILog log = LogManager.GetLogger(typeof(JsonServer));
        public JsonServer(string host, int port, IServices server) : base(host, port)
        {
            this.server = server;
            log.Debug("Creating JsonServer...");
        }
        protected override Thread createWorker(TcpClient client)
        {
            if (client == null)
            {
                Console.WriteLine("TcpClient is null.");
                throw new ArgumentNullException(nameof(client), "TcpClient cannot be null.");
            }

            worker = new ClientWorker(server, client);
            return new Thread(worker.run);
        }
    }
}


