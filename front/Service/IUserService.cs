using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserService
    {
        List<String> allNames();
        void addUser(User user);
        User Login(String username, String password);
        Boolean Logout(String username);
        Boolean ValidateToken(String token);

    }
}
