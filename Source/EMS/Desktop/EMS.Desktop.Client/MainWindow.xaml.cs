using EMS.Desktop.Client.Helpers;
using EMS.Desktop.Client.Models;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EMS.Desktop.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient httpClient = new HttpClient();
        private Brush btnLoginOriginalColor;
        private Config config;
         
        public MainWindow()
        {
            InitializeComponent();
            var configFilePath = "Configs/Config.json";
            var config = File.Exists(configFilePath) ? File.ReadAllText("Configs/Config.json") : DefaultConfig;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = this.tbUsername.Text;
            var password = this.tbPassword.SecurePassword.DecryptSecureString();

            if (this.IsValidCredential(username) && 
                this.IsValidCredential(password))
            {
                this.WindowStyle = WindowStyle.None;
            }
        }

        private bool IsValidCredential(string text)
        {
            return string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
        }

        private void btnLogin_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnLoginOriginalColor = this.btnLogin.Foreground;
            this.btnLogin.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void btnLogin_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnLogin.Foreground = this.btnLoginOriginalColor;
        }
    }
}
