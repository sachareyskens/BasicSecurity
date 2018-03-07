using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace front.Authentication
{
    public class LoggedUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string accesstoken { get; set; }
    }
}
