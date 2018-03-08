using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserRepository
    {
        Task<String> Login(String username, String password);
        Task<Boolean> LogoutAsync(String username);
        Task<List<String>> AllNamesAsync();
        Task<Boolean> ValidateToken(String token);
        void AddUser(User user);
    }
}
