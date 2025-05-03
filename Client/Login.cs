using Client.GUI;
using Services;
using log4net;
namespace Client;

 public partial class Login : Form
    {
        private readonly IServices service;
        private MainWindow mainWindow;
        private string currentUsername;
        private static readonly ILog logger = LogManager.GetLogger(typeof(Login));

        public Login(IServices service)
        {
            this.service = service;
            InitializeComponent();
            logger.Debug("Login form initialized");
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Optional: any load-time logic
        }

        private void ClickEvent(object sender, EventArgs e)
        {
            Console.WriteLine("Login button pressed");
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            try
            {
                mainWindow = new MainWindow(service);
                service.login(username, password, mainWindow);

                currentUsername = username;
                OpenMainWindow();
            }
            catch (Exception ex)
            {
                logger.Error("Login failed", ex);
                MessageBox.Show("Login failed: " + ex.Message, "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (mainWindow != null)
                {
                    mainWindow.Dispose();
                    mainWindow = null;
                }
            }
        }

        private void OpenMainWindow()
        {
            try
            {
                mainWindow.Text = $"Main Window for {currentUsername}";

                mainWindow.FormClosing += (sender, e) =>
                {
                    LogoutUser();
                    txtUsername.Clear();
                    txtPassword.Clear();
                    this.Show();
                };

                mainWindow.Show();
                mainWindow.SetOficiuUsername(currentUsername);
                mainWindow.LoadData();
                this.Hide();
            }
            catch (Exception ex)
            {
                logger.Error($"Error opening main window: {ex.Message}", ex);
                MessageBox.Show($"Error opening main window: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogoutUser()
        {
            try
            {
                if (!string.IsNullOrEmpty(currentUsername) && mainWindow != null)
                {
                    service.logout(currentUsername, mainWindow);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Error during logout: {ex.Message}", ex);
            }
        }
    }