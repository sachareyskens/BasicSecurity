using System;
using System.Collections.Generic;


using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private string prefix;

        private HttpClient client;
        public UserRepository(String username, String password)
        {

            client = new HttpClient();
            prefix = Global.IP_ADRESS;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue(
                "Basic",
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password))));

        }
        public void AddUser(User user)
        {
            Uri url = new Uri(prefix + "/users/add");
            var jsonString = JsonConvert.SerializeObject(user);
            HttpResponseMessage response = client.PostAsync(url, new HttpStringContent(jsonString, UnicodeEncoding.Utf8, "application/json")).GetResults();
        }

        public async Task<List<string>> AllNamesAsync()
        {
            Uri url = new Uri(prefix + "/users/names");
            HttpResponseMessage response = client.GetAsync(url).GetResults();
            string jsonString = "";


            jsonString = await response.Content.ReadAsStringAsync();


            var StringResponse = JsonConvert.DeserializeObject<List<String>>(jsonString);
            return StringResponse;
        }

        public async Task<User> Login(string username, string password)
        {
            HttpResponseMessage response = null;
            try
            {
                Uri url = new Uri(prefix + "/users/login?username=" + username + "&password=" + password);
                response = client.GetAsync(url).GetResults();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                
            }
            string jsonString = "";

            try
            {
                jsonString = await response.Content.ReadAsStringAsync();
            } catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            var user = JsonConvert.DeserializeObject<User>(jsonString);

            return user;
        }

        public async Task<Boolean> LogoutAsync(String username)
        {
            Uri url = new Uri(prefix + "/users/logout?username=" + username);
            HttpResponseMessage response = client.GetAsync(url).GetResults();
            string jsonString = "";

            
            jsonString = await response.Content.ReadAsStringAsync();
            

            var boolResponse = JsonConvert.DeserializeObject<Boolean>(jsonString);
            return boolResponse;
        }

        public async Task<Boolean> ValidateToken(string token)
        {
            Uri url = new Uri(prefix + "/users/validatetoken?token=" + token);
            HttpResponseMessage response = client.GetAsync(url).GetResults();
            string jsonString = "";


            jsonString = await response.Content.ReadAsStringAsync();


            var boolResponse = JsonConvert.DeserializeObject<Boolean>(jsonString);
            return boolResponse;
        }
    }
}
