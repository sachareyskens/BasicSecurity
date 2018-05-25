using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using frontend.Models;

namespace frontend
{
    /// <summary>
    /// Interaction logic for LogoutPage.xaml
    /// </summary>
    public partial class LogoutPage : Page
    {
        HomeWindow scherm = (HomeWindow)Application.Current.MainWindow;
        HttpClient client = new HttpClient();
        public LogoutPage()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

            client.BaseAddress = new Uri(Adress.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "basic_security", "2018"))));
            menuBox.SelectionChanged += MenuBox_SelectionChanged;

            if (MessageBox.Show("Bent u zeker dat u wilt afmelden?", "Afmelden",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Logout();
                LoginWindow window = new LoginWindow();
                window.Show();
                scherm.Close();
            }
            else
            {
                scherm.displayFrame.Source = new Uri("HomePage.xaml", UriKind.Relative);
            }
        }


        private void MenuBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = menuBox.SelectedIndex;

            switch (index)
            {
                case 0:
                    scherm.displayFrame.Source = new Uri("HomePage.xaml", UriKind.Relative);
                    break;
                case 1:
                    scherm.displayFrame.Source = new Uri("MessagePage.xaml", UriKind.Relative);
                    break;
                case 2:
                    scherm.displayFrame.Source = new Uri("RecievedPage.xaml", UriKind.Relative);
                    break;
                case 3:
                    scherm.displayFrame.Source = new Uri("ChatboxPage.xaml", UriKind.Relative);
                    break;
                case 4:
                    scherm.displayFrame.Source = new Uri("StegenographyWindow.xaml", UriKind.Relative);
                    break;
                
                case 5:
                    scherm.displayFrame.Source = new Uri("LogoutPage.xaml", UriKind.Relative);
                    break;

            }
        }

        private async void Logout()
        {
            var userUrl = "/api/users/logout?username=" + scherm.GetUser().username;
            HttpResponseMessage response = await client.GetAsync(userUrl);
        
        }
    }
}

