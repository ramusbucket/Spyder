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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private HttpClient httpClient = new HttpClient();
        private Brush btnRegisterOriginalColor;
        private Config config;
         
        public Register()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private bool IsValidCredential(string text)
        {
            return string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
        }

        private void btnLogin_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void btnLogin_MouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var username = this.tbUsername.Text;
            var password = this.tbPassword.SecurePassword.DecryptSecureString();

            if (this.IsValidCredential(username) &&
                this.IsValidCredential(password))
            {
                this.WindowStyle = WindowStyle.None;
            }
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
    }
}
