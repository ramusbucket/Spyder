using EMS.Desktop.Client.Helpers;
using EMS.Desktop.Client.Models;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMS.Desktop.Client
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        private Config config;
        private IHttpClient httpClient;
        private List<Task> listenerTasks;
        private Brush btnLoginOriginalColor;
        private Brush btnRegisterOriginalColor;

        public LoginPage()
        {
            InitializeComponent();

            this.config = LoadConfig();
            var dependenciesRegister = new DependenciesRegister();
            var injector = dependenciesRegister.RegisterDependencies(this.config);
            this.httpClient = injector.Resolve<IHttpClient>();
            //this.StartListeners(injector);
        }

        private void StartListeners(IInjector injector)
        {
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
            this.listenerTasks = new List<Task>(listeners.Count());
            foreach (var listener in listeners)
            {
                this.listenerTasks.Add(
                    Task.Run(
                        async () => await listener.Start()));
            }
        }

        private Config LoadConfig()
        {
            var configFilePath = $"{System.AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "")}\\Configs\\Config.json";
            var configJson = File.Exists(configFilePath) ?
                File.ReadAllText(configFilePath) :
                string.Empty;
            var config = JsonConvert.DeserializeObject<Config>(configJson);

            return config;
        }

        private bool IsValidCredential(string text)
        {
            return !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
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

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new RegisterPage(this.config));
        }

        private void btnRegister_MouseEnter(object sender, MouseEventArgs e)
        {
            this.btnRegisterOriginalColor = this.btnRegister.Foreground;
            this.btnRegister.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void btnRegister_MouseLeave(object sender, MouseEventArgs e)
        {
            this.btnRegister.Foreground = this.btnRegisterOriginalColor;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = this.tbUsername.Text;
            var password = this.tbPassword.SecurePassword.DecryptSecureString();

            if (this.IsValidCredential(username) &&
                this.IsValidCredential(password))
            {
                // Login

                // Save auth token
                this.httpClient.PostAsJsonAsync(this.config.UrisConfig.RegisterUserUri, new object { });
                var isLoginSuccessful = true;

                if (isLoginSuccessful)
                {
                }
            }
        }
    }
}
