using frontend.Models;
using System;
using System.Collections;
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
    /// Interaction logic for RecievedPage.xaml
    /// </summary>
    public partial class RecievedPage : Page
    {
        HttpClient client = new HttpClient();
        HomeWindow scherm;
        IEnumerable<Message> messages;
        Message decryptedMessage;
        List<Message> decryptedList = new List<Message>();
        public RecievedPage()
        {
            InitializeComponent();
            scherm = (HomeWindow)Application.Current.MainWindow;
            menuBox.SelectionChanged += MenuBox_SelectionChanged;
            
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

            client.BaseAddress = new Uri(Adress.url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "basic_security", "2018"))));


            getAllUsers();

        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = "";

            getAllUsers();
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

        public async void getAllUsers()
        {
            var userUrl = "/basicsec/api/messages/showall?username=" + scherm.GetUser().username;
            HttpResponseMessage response = await client.GetAsync(userUrl);
            IEnumerable<Message> t = null;
            if (response.IsSuccessStatusCode)
            {
                t = await response.Content.ReadAsAsync<List<Message>>();
                articlesDataGrid.ItemsSource = t;
                messages = t;
            }

        }



        

        private void SearchTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            getAllUsers();
            IEnumerable<Message> allMessages = messages;

            List<Message> selectedMessages = new List<Message>();

            foreach (Message m in allMessages)
            {
                if (m.sender == searchTextBox.Text)
                {
                    selectedMessages.Add(m);
                }
            }
            if (selectedMessages.Count !=0)
            {
                articlesDataGrid.ItemsSource = selectedMessages;
            }

        }

        public async void decryptMessage(Message message)
        {
            var userUrl = "/basicsec/api/messages/decrypt/" + message.id + "?loggedIn=" + scherm.GetUser().username;
            HttpResponseMessage response = await client.GetAsync(userUrl);
            
            if (response.IsSuccessStatusCode)
            {
                decryptedMessage = await response.Content.ReadAsAsync<Message>();
                decryptedList.Add(decryptedMessage);
                decryptedDataGrid.ItemsSource = decryptedList;
                decryptedDataGrid.Items.Refresh();
            }

            

            
        }

        public async void deleteMessage(Message message)
        {
            var userUrl = "/basicsec/api/messages/delete/" + message.id;
            HttpResponseMessage response = await client.DeleteAsync(userUrl);
            Message t = null;
            if (response.IsSuccessStatusCode)
            {
                t = await response.Content.ReadAsAsync<Message>();
                getAllUsers();
            }
        }

        private void decryptMessageButton_Click(object sender, RoutedEventArgs e)
        {
            decryptMessage((Message)articlesDataGrid.SelectedItem);
            
        }

        private void deleteMessageButton_Click(object sender, RoutedEventArgs e)
        {
            deleteMessage((Message)articlesDataGrid.SelectedItem);
            
        }
    }
}
