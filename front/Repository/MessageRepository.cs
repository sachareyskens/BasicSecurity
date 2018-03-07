using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;

namespace Repository
{
    public class MessageRepository : IMessageRepository
    {
        private HttpClient client;

        public MessageRepository(String username, String password)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(Global.IP_ADRESS);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password))));

        }

        public void addMessage(Message message)
        {
            var url = "/messages/add";
            var jsonString = JsonConvert.SerializeObject(message);
            HttpResponseMessage response = client.PostAsync(url, new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;
        }

        public async Task<Message> decryptMessageAsync(int id, string username)
        {
            var url = "/messages/decrypt/" + id + "?loggedIn=" + username;
            HttpResponseMessage response = client.GetAsync(url).Result;
            string jsonString = "";

            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
            }

            var decryptedMessage = JsonConvert.DeserializeObject<Message>(jsonString);
            return decryptedMessage;
        }


        public async Task<List<Message>> getAllByReciever(string reciever)
        {
            var url = "/messages/showall?username=" + reciever;
            HttpResponseMessage response = client.GetAsync(url).Result;
            string jsonString = "";

            if (response.IsSuccessStatusCode)
            {
                jsonString = await response.Content.ReadAsStringAsync();
            }
            var result = JsonConvert.DeserializeObject<List<Message>>(jsonString);
            return result;

        }
    }
}
