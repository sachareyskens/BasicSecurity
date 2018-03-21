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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace frontend
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        
        HttpClient client = new HttpClient();
        HomeWindow scherm;
        public HomePage()
        {
            InitializeComponent();
            scherm = (HomeWindow)Application.Current.MainWindow;
            menuBox.SelectionChanged += MenuBox_SelectionChanged;
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;         

            client.BaseAddress = new Uri("https://localhost:8443");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "basic_security", "2018"))));

            loadData();

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
                    scherm.displayFrame.Source = new Uri("SettingsPage.xaml", UriKind.Relative);
                    break;
                case 5:
                    scherm.displayFrame.Source = new Uri("LogoutPage.xaml", UriKind.Relative);
                    break;

            }
        }

        public async void loadData()
        {
            var userUrl = "/api/messages/countAll";
            HttpResponseMessage response = await client.GetAsync(userUrl);

            if (response.IsSuccessStatusCode)
            {
                StatsSource s = await response.Content.ReadAsAsync<StatsSource>();
                welcomeBlock.Text = "Welcome, " + scherm.GetUser().username + "!";
                messBlock.Text = s.totalMessages;
                usersBlock.Text = s.totalUsers;
            }

        }
    }
}
