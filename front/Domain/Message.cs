using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Message
    {
        public int id { get; set; }
        public String message { get; set; }
        public String sender { get; set; }
        public String reciever { get; set; }
        public byte[] encryptedSymm { get; set; }
        public byte[] signature { get; set; }
        public String validation { get; set; }
    }
}
