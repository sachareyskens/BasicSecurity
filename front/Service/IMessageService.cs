using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IMessageService
    {
        List<Message> All(String username);
        Message Decrypt(int id, String username);
        void AddMessage(Message message);
    }
}
