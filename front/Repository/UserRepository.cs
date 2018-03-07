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
    public class UserRepository : IUserRepository
    {
        private HttpClient client;
        public UserRepository(String username, String password)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(Global.IP_ADRESS);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password))));
        }
        public void AddUser(User user)
        {
            var url = "/users/add";
            var jsonString = JsonConvert.SerializeObject(user);
            HttpResponseMessage response = client.PostAsync(url, new StringContent(jsonString, Encoding.UTF8, "application/json")).Result;
        }

        public async Task<List<string>> AllNamesAsync()
        {
            var url = "/users/names";
            HttpResponseMessage response = client.GetAsync(url).Result;
            string jsonString = "";


            jsonString = await response.Content.ReadAsStringAsync();


            var StringResponse = JsonConvert.DeserializeObject<List<String>>(jsonString);
            return StringResponse;
        }

        public async Task<String> Login(string username, string password)
        {
            var url = "/users/login?username=" + username + "&password=" + password;
            HttpResponseMessage response = client.GetAsync(url).Result;
            string jsonString = "";


            jsonString = await response.Content.ReadAsStringAsync();


            var StringResponse = JsonConvert.DeserializeObject<String>(jsonString);
            return StringResponse;
        }

        public async Task<Boolean> LogoutAsync(String username)
        {
            var url = "/users/logout?username=" + username;
            HttpResponseMessage response = client.GetAsync(url).Result;
            string jsonString = "";

            
            jsonString = await response.Content.ReadAsStringAsync();
            

            var boolResponse = JsonConvert.DeserializeObject<Boolean>(jsonString);
            return boolResponse;
        }

        public async Task<bool> ValidateToken(string token)
        {
            var url = "/users/validatetoken?token=" + token;
            HttpResponseMessage response = client.GetAsync(url).Result;
            string jsonString = "";


            jsonString = await response.Content.ReadAsStringAsync();


            var boolResponse = JsonConvert.DeserializeObject<Boolean>(jsonString);
            return boolResponse;
        }
    }
}
