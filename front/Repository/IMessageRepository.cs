using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IMessageRepository
    {
        Task<List<Message>> getAllByReciever(String reciever);
        Task<Message> decryptMessageAsync(int id, String username);
        void addMessage(Message message);
       
    }
}
