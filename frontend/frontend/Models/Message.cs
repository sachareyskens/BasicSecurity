using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend.Models
{
    public class Message
    {
        public int id { get; set; }
        public String message { get; set; }
        public String sender { get; set; }
        public String reciever { get; set; }
        public Byte[] encryptedSymm { get; set; }
        public Byte[] signature { get; set; }
        public String validation { get; set; }
    }
}
