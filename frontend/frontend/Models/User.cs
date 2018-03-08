using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend.Models
{
    public class User
    {
        public String username { get; set; }
        public String password { get; set; }
        public Byte[] pubKey { get; set; }
        public Byte[] privKey { get; set; }
        public bool active { get; set; }
        public String accesToken { get; set; }
    }
}
