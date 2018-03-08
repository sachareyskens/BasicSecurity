using frontend.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace frontend
{
    /// <summary>
    /// Interaction logic for MessagePage.xaml
    /// </summary>
    public partial class MessagePage : Page
    {
        HttpClient client = new HttpClient();
        HomeWindow scherm;
        List<string> attachments = new List<string>();
        User Sender;
        List<string> l;
        public MessagePage()
        {
            InitializeComponent();
            
            menuBox.SelectionChanged += MenuBox_SelectionChanged;
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            scherm =  (HomeWindow)Application.Current.MainWindow;
            client.BaseAddress = new Uri("https://localhost:8443");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "basic_security", "2018"))));
            GetAllNames();
            Sender = scherm.GetUser();
            senderTextBox.Text = Sender.username;
            recieverBox.ItemsSource = l;
            
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
                    scherm.displayFrame.Source = new Uri("HomePage.xaml", UriKind.Relative);
                    break;
                case 3:
                    scherm.displayFrame.Source = new Uri("HomePage.xaml", UriKind.Relative);
                    break;
                case 4:
                    scherm.displayFrame.Source = new Uri("Statspage.xaml", UriKind.Relative);
                    break;
                case 5:
                    scherm.displayFrame.Source = new Uri("LogoutPage.xaml", UriKind.Relative);
                    break;

            }
        }
        public async void GetAllNames()
        {
            var userUrl = "/api/users/names";
            HttpResponseMessage response = await client.GetAsync(userUrl);
            List<String> t = null;
            if (response.IsSuccessStatusCode)
            {
                t = await response.Content.ReadAsAsync<List<String>>();
                

            }
            l = t;
        }

        
        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ben je zeker dat je deze mail wilt annuleren? Alle wijzigingen zullen verloren gaan", "Sluit venster",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                scherm.displayFrame.Source = new Uri("HomePage.xaml", UriKind.Relative);

            }
        }


        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {

            if (messageText.Text != "")
            {
                try
                {
                    Message message = new Message();
                    message.reciever = recieverBox.SelectedValue.ToString();
                    message.message = messageText.Text;
                    message.sender = Sender.username;
                    message.validation = "";
                    message.signature = new Byte[1];
                    message.encryptedSymm = new Byte[1];

                    var userUrl = "/api/messages/add";
                    HttpResponseMessage response = await client.PostAsJsonAsync(userUrl, message);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Succesfully sent");
                    }

                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Verbinding met de server verbroken. Probeer later opnieuw.",
                        "Serverfout", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
    }
}
