using EMS.Desktop.Client.Helpers;
using EMS.Desktop.Client.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
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
            var configFilePath = $"{System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\","")}\\Configs\\Config.json";
            var configJson = File.Exists(configFilePath) ? 
                File.ReadAllText(configFilePath) : 
                string.Empty;

            var dependenciesRegister = new DependenciesRegister();
            var injector = dependenciesRegister.RegisterDependencies(configJson);

            var type = typeof(IListener);
            var listenerTypes = Assembly.Load("EMS.Desktop.Client")
                .GetTypes()
                .Where(
                    x =>
                        !x.IsAbstract &&
                        !x.IsInterface &&
                        x.IsClass &&
                        type.IsAssignableFrom(x));

            var listeners = listenerTypes.Select(x => injector.Resolve<IListener>(x.Name));
            var listenerTasks = new List<Task>(listeners.Count());
            foreach (var listener in listeners)
            {
                listenerTasks.Add(
                    Task.Run(
                        async () => await listener.Start()));
            }

            Task.WaitAll(listenerTasks.ToArray());
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
