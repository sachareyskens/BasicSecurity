using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public String username { get; set; }
        public String password { get; set; }
        public byte[] pubKey { get; set; }
        public byte[] privKey { get; set; }
        public Boolean active {get;set;}
        public String accesstoken { get; set; }
    }
}
