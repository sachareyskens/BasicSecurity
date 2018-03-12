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
        public string message { get; set; }
        public string sender { get; set; }
        public string receiver { get; set; }
        public byte[] encryptedSymm { get; set; }
        public byte[] signature { get; set; }
        public string validation { get; set; }
        public DateTime date { get; set; }
    }
}
