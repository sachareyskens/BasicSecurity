using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Repository;

namespace Service
{
    public class MessageService : IMessageService
    {
        private MessageRepository repo;

        public MessageService(String username, String password)
        {
            this.repo = new MessageRepository(username, password);
        }
        public void AddMessage(Message message)
        {
            repo.addMessage(message);
        }

        public List<Message> All(string username)
        {
            return repo.getAllByReciever(username).Result.ToList();
        }

        public Message Decrypt(int id, string username)
        {
            return repo.decryptMessageAsync(id, username).Result;
        }
    }
}
