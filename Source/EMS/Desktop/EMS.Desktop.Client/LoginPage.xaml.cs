using Easy.Common.Interfaces;
using EMS.Desktop.Client.Helpers;
using EMS.Desktop.Client.Models;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
        private OAuthTokenDetails authDetails;
        private Config config;
        private IRestClient restClient;
        private List<Task> listenerTasks;
        private Brush btnLoginOriginalColor;
        private Brush btnRegisterOriginalColor;

        public LoginPage()
        {
            InitializeComponent();

            this.config = LoadConfig();
            var dependenciesRegister = new DependenciesRegister();
            var injector = dependenciesRegister.RegisterDependencies(this.config);
            this.restClient = injector.Resolve<IRestClient>();
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
            this.NavigationService.Navigate(new RegisterPage(this.config, this.restClient));
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

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = this.tbUsername.Text;
            var password = this.tbPassword.SecurePassword.DecryptSecureString();

            if (this.IsValidCredential(username) &&
                this.IsValidCredential(password))
            {
                // Login
                var requestUri = new Uri($"{this.config.UrisConfig.BaseServiceUri}{this.config.UrisConfig.LoginUserUri}");
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                request.Content = new FormUrlEncodedContent(
                    new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("grant_type","password"),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("username", username)
                    });

                var rawResponse = await this.restClient.SendAsync(request);
                var rawResponseContent = await rawResponse.Content.ReadAsStringAsync();

                if (rawResponse.IsSuccessStatusCode)
                {
                    rawResponse.EnsureSuccessStatusCode();
                    var response = JsonConvert.DeserializeObject<OAuthTokenDetails>(rawResponseContent);

                    // Save auth token
                    var isLoginSuccessful = !string.IsNullOrEmpty(response.AccessToken);
                    if (isLoginSuccessful)
                    {
                        // Save credentials
                        this.authDetails = response;

                        // Notify user for successful login
                        MessageBox.Show("Loggin successful", "Login successful", MessageBoxButton.OK);

                        // Hide UI
                        (this.Parent as MainNavigationWindow).Hide();

                        // Start listening
                    }
                }
                else
                {
                    var responseContent = JsonConvert.DeserializeObject<LoginResponse>(rawResponseContent);
                    MessageBox.Show(responseContent.ErrorDescription);
                }
            }
        }
    }
}
