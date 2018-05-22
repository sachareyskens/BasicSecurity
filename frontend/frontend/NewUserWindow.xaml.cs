using frontend.Models;
using System;
using System.Collections.Generic;
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

namespace frontend
{
    /// <summary>
    /// Interaction logic for NewUserWindow.xaml
    /// </summary>
    public partial class NewUserWindow : Window
    {
        HttpClient client = new HttpClient();

        public NewUserWindow()
        {

            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            InitializeComponent();

            emailBox.GotFocus += EmailBox_GotFocus;
            emailBox.LostFocus += EmailBox_LostFocus;

            passwordTextBox.GotFocus += PasswordTextBox_GotFocus;
            passwordBox.LostFocus += PasswordBox_LostFocus;
            

            

            client.BaseAddress = new Uri(Adress.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "basic_security", "2018"))));
        }


        private async void addButton_Click(object sender, RoutedEventArgs e)
        {

            if (emailBox.Text != "" && passwordBox.Password != "")
            {
                try
                {
                    User user = new User();
                    user.username = emailBox.Text;
                    user.password = passwordBox.Password;
                    user.active = true;


                    var userUrl = "/basicsec/api/users/add";
                    HttpResponseMessage response = await client.PostAsJsonAsync(userUrl, user);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Succesfully Registered");
                        LoginWindow login = new LoginWindow();
                        login.Show();
                        Close();
                    }

                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Verbinding met de server verbroken. Probeer later opnieuw.",
                        "Serverfout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter message/select reciever.");
            }

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
    }
}
