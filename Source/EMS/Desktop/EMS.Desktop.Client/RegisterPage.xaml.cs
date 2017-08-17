using Easy.Common;
using Easy.Common.Interfaces;
using EMS.Desktop.Client.Helpers;
using EMS.Desktop.Client.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EMS.Desktop.Client
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private IRestClient restClient;
        private Brush btnRegisterOriginalColor;
        private Config config;

        public RegisterPage(Config config, IRestClient restClient)
        {
            InitializeComponent();
            this.config = config;
            this.restClient = restClient;
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

        private bool IsValidCredential(string text)
        {
            return !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var username = this.tbUsername.Text;
            var password = this.tbPassword.SecurePassword.DecryptSecureString();
            var confirmPassword = this.tbConfirmPassword.SecurePassword.DecryptSecureString();
            var fullName = this.tbFullName.Text;
            var email = this.tbEmail.Text;

            if (this.IsValidCredential(username) &&
                this.IsValidCredential(password) &&
                this.IsValidCredential(confirmPassword) &&
                this.IsValidCredential(fullName) &&
                this.IsValidCredential(email))
            {
                var requestData = new
                {
                    Email = email,
                    Username = username,
                    Password = password,
                    ConfirmPassword = confirmPassword
                };

                var requestUri = new Uri($"{this.config.UrisConfig.BaseServiceUri}{this.config.UrisConfig.RegisterUserUri}");
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
                requestMessage.Content = new JSONContent(JsonConvert.SerializeObject(requestData),Encoding.UTF8);

                var response = await this.restClient.SendAsync(requestMessage);

                MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
