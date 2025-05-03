using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Services;
using Networking.JsonProtocol;
namespace Client;

internal class StartClient
    {
        private static readonly int DEFAULT_PORT = 55556;
        private static readonly string DEFAULT_IP = "127.0.0.1";
        private static readonly ILog log = LogManager.GetLogger(typeof(StartClient));

        [STAThread]
        public static void Main(string[] args)
        {
            
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Debug("Reading properties from app.config...");
            int port = DEFAULT_PORT;
            string ip = DEFAULT_IP;

            string portS = ConfigurationManager.AppSettings["port"];
            if (portS == null)
            {
                log.DebugFormat("Port property not set. Using default value {0}", DEFAULT_PORT);
            }
            else if (!int.TryParse(portS, out port))
            {
                log.DebugFormat("Port property not a number. Using default value {0}", DEFAULT_PORT);
                port = DEFAULT_PORT;
            }

            // Read IP from app.config
            string ipS = ConfigurationManager.AppSettings["ip"];
            if (ipS != null)
            {
                ip = ipS;
            }
            else
            {
                log.DebugFormat("IP property not set. Using default value {0}", DEFAULT_IP);
            }

            log.InfoFormat("Using server on IP {0} and port {1}", ip, port);

            // Initialize the service proxy
            IServices server = new Proxy(ip, port);

            // Start the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Login loginWindow = new Login(server);
                Application.Run(loginWindow);
            }
            catch (Exception ex)
            {
                log.Error("Application failed to start", ex);
                MessageBox.Show($"Application failed to start: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }