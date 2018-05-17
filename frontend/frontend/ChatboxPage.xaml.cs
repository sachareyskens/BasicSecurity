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
using System.Windows.Threading;

namespace frontend
{
    /// <summary>
    /// Interaction logic for ChatboxPage.xaml
    /// </summary>
    public partial class ChatboxPage : Page
    {
        HttpClient client = new HttpClient();
        HomeWindow scherm;
        DispatcherTimer fetchTimer;
        int counter;
        Boolean fetching = false;
        IEnumerable<Message> checker = new List<Message>();
        Message checkerMessage = new Message();
        
        public ChatboxPage()
        {

            InitializeComponent();
            scherm = (HomeWindow)Application.Current.MainWindow;
            menuBox.SelectionChanged += MenuBox_SelectionChanged;
            ServicePointManager.ServerCertificateValidationCallback += (send, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            sendMessages.KeyDown += sendMessages_KeyDown;
            fetchTimer = new DispatcherTimer();
            fetchTimer.Interval = TimeSpan.FromSeconds(1);
            fetchTimer.Tick += fetchTimer_Tick;
            counter = 8;
            client.BaseAddress = new Uri("https://localhost:8443");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "basic_security", "2018"))));
            GetAllNames();
            chatterList.SelectionChanged += ComboBox_SelectionChangedAsync;
        }

        private void fetchTimer_Tick(object sender, EventArgs e)
        {
            if (fetching != false)
            {
                counter--;
                if (counter == 0)
                {
                    fetchTimer.Stop();
                    fetchAllMessages();
                    counter = 10;
                    fetchTimer.Start();
                }
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
                    scherm.displayFrame.Source = new Uri("SettingsPage.xaml", UriKind.Relative);
                    break;
                case 6:
                    scherm.displayFrame.Source = new Uri("LogoutPage.xaml", UriKind.Relative);
                    break;

            }
        }
        private void sendMessages_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                Sendbutton.Focus();
            }
            else if (e.Key == Key.Enter)
            {
                Send();
            }
        }

        private void Sendbutton_ClickAsync(object sender, RoutedEventArgs e)
        {
            Send();
        }

        public async void GetAllNames()
        {
            var userUrl = "/api/users/names";
            HttpResponseMessage response = await client.GetAsync(userUrl);
            List<String> t = null;
            if (response.IsSuccessStatusCode)
            {
                t = await response.Content.ReadAsAsync<List<String>>();
                t.Remove(scherm.GetUser().username);
                if (t.Count != 0)
                {
                    chatterList.ItemsSource = t;
                } else
                {
                    chatterList.Visibility = Visibility.Collapsed;
                }
            }

        }

        private async void Send()
        {
            if (sendMessages.Text != "" && chatterList.SelectedValue != null)
            {
                try
                {
                    Sendbutton.IsEnabled = false;
                    Message message = new Message();
                    message = new Message();
                    message.receiver = chatterList.SelectedValue.ToString();
                    message.message = sendMessages.Text;
                    message.sender = scherm.GetUser().username;
                    message.validation = "";
                    message.date = DateTime.Now.ToString();
                    message.signature = new Byte[1];
                    message.encryptedSymm = new Byte[1];


                    var userUrl = "/api/messages/add";
                    HttpResponseMessage response = await client.PostAsJsonAsync(userUrl, message);

                    
                    Sendbutton.IsEnabled = true;
                    txtMessages.AppendText(message.date + " - " + message.sender + ": " + message.message + "\n");
                    txtMessages.ScrollToEnd();
                    sendMessages.Clear();
                    

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


        private void ComboBox_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (fetching == false)
            {
                fetching = true;
                fetchTimer.Start();
            }
            fetchAllMessages();
            
        }

        private async void fetchAllMessages()
        {
            try
            {
                var userUrl = "/api/messages/decrypt/all?sender=" + chatterList.SelectedValue.ToString() + "&reciever=" + scherm.GetUser().username;
                HttpResponseMessage response = await client.GetAsync(userUrl);
                List<Message> t = null;
                int index = 0;
                if (response.IsSuccessStatusCode)
                {
                    
                    t = await response.Content.ReadAsAsync<List<Message>>();

                    
                   
                    if (t.Last().id == checkerMessage.id) {
                        t = null;
                    } else
                    {
                        index = t.FindIndex(x => x.id == checkerMessage.id);
                        if (index != -1)
                        {
                            index--;
                            for (int i = 0; i < index; i++)
                            {
                                t.RemoveAt(0);
                                
                            }
                        }
                        
                    }
                    if (t != null)
                    {
                        foreach (Message m in t)
                        {
                            txtMessages.AppendText(m.date + " - " + m.sender + ": " + m.message + "\n");
                        }
                        checkerMessage = t.Last();
                        txtMessages.ScrollToEnd();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                fetchTimer.Stop();
                MessageBox.Show("Sorry, failed to fetch any data. You might not have messages from this person. Reason : " + ex.Message );
            }
        }
    }

}
