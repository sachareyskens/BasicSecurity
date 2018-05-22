using frontend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace frontend
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        
        HttpClient client = new HttpClient();
        HomeWindow homeWindow;
        DispatcherTimer loginTimer;
        private int attempts = 0;
        private int counter = 10;

        public LoginWindow()
        {
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            InitializeComponent();

            emailBox.GotFocus += EmailBox_GotFocus;
            emailBox.LostFocus += EmailBox_LostFocus;
            emailBox.KeyDown += EmailBox_KeyDown;

            passwordTextBox.GotFocus += PasswordTextBox_GotFocus;
            passwordBox.LostFocus += PasswordBox_LostFocus;
            passwordBox.KeyDown += PasswordBox_KeyDown;

            loginTimer = new DispatcherTimer();
            loginTimer.Interval = TimeSpan.FromSeconds(1);
            loginTimer.Tick += LoginTimer_Tick;

            client.BaseAddress = new Uri(Adress.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}","basic_security", "2018"))));

        }
        private void EmailBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (emailBox.Text == "")
            {
                emailBox.Text = "E-mailadres";
                emailBox.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void EmailBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (loginButton.IsEnabled == true)
            {
                errorBox.Content = "";
            }

            if (emailBox.Text == "E-mailadres")
            {
                emailBox.Text = "";
                emailBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void EmailBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                passwordTextBox.Focus();
            }
            else if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password.Length == 0)
            {
                passwordTextBox.Visibility = Visibility.Visible;
            }
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (loginButton.IsEnabled == true)
            {
                errorBox.Content = "";
            }

            passwordTextBox.Visibility = Visibility.Hidden;
            passwordBox.Focus();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                loginButton.Focus();
            }
            else if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            loginButton.IsEnabled = false;
            Login();
        }

        private void LoginTimer_Tick(object sender, EventArgs e)
        {
            counter--;
            if (counter == 0)
            {
                loginTimer.Stop();
                errorBox.Content = "";
                loginButton.IsEnabled = true;
                counter = attempts * 5;
            }
            else
            {
                errorBox.Content = "Te veel pogingen. \nProbeer het opnieuw over " + counter + " seconden";
            }
        }
        public async Task<User> CheckUserAsync(string username)
        {
            User user = null;

            try
            {
                var userUrl = "/basicsec/api/users/get/" + username;
                HttpResponseMessage response = await client.GetAsync(userUrl);
                user = await response.Content.ReadAsAsync<User>();
                
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Verbinding met de server verbroken. Probeer later opnieuw.",
                    "Serverfout", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.Write(e.InnerException);

            }

            return user;
        }

        private async void Login()
        {
            loginButton.IsEnabled = false;
            messageLabel.Content = "Velden controleren...";
           
            User user = await CheckUserAsync(emailBox.Text);
            

            if (user!=null)
            {
                
                try
                {
                    var userUrl = "/basicsec/api/users/login?username=" + emailBox.Text + "&password=" + passwordBox.Password;
                    HttpResponseMessage response = await client.GetAsync(userUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        User userr = await response.Content.ReadAsAsync<User>();
                        user = userr;
                        
                    }
                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Verbinding met de server verbroken. Probeer later opnieuw.",
                        "Serverfout", MessageBoxButton.OK, MessageBoxImage.Error);
                }



                if (user.accesToken!=null)
                {
                    messageLabel.Content = "Bezig met aanmelden...";
                    homeWindow = new HomeWindow(user);
                    Application.Current.MainWindow = homeWindow;
                    homeWindow.Owner = Owner;
                    homeWindow.Show();
                    Close();
                }
                else
                {
                    errorBox.Content = "Verkeerd paswoord of geen toegang!";
                    passwordBox.Password = "";
                    messageLabel.Content = "";
                    loginButton.IsEnabled = true;
                    attempts++;
                }
            }
            else
            {
                errorBox.Content = "Email bestaat niet";
                messageLabel.Content = "";
                loginButton.IsEnabled = true;
                attempts++;
            }

            if (!(attempts < (counter / 5 + 1)))
            {
                loginButton.IsEnabled = false;
                counter = attempts * 5;
                loginTimer.Start();
            }
            else
            {
                loginButton.IsEnabled = true;
            }
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            NewUserWindow window = new NewUserWindow();
            window.Show();
            Close();
        }

       
    }
}
