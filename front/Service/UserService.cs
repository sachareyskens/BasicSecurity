using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Repository;

namespace Service
{
    public class UserService : IUserService
    {
        private UserRepository repo;
        public UserService(String username, String password)
        {
            this.repo = new UserRepository(username, password);
        }
        public void addUser(User user)
        {
            repo.AddUser(user);
        }

        public List<string> allNames()
        {
            return repo.AllNamesAsync().Result.ToList(); ;
        }

        public string Login(string username, string password)
        {
            return repo.Login(username, password).Result;
        }

        public bool Logout(string username)
        {
            return repo.LogoutAsync(username).Result;
        }

        public bool ValidateToken(string token)
        {
            return repo.ValidateToken(token).Result;
        }
    }
}
