using Domain;
using front.Authentication;
using front.Messages;
using front.Navigation;
using front.Utility;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace front.ViewModels
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        private ObservableCollection<Message> messages;
        private IMessageService messageService;
        public ICommand DecryptCommand;
        public ICommand LoadCommand;
        LoggedUser logged;

        public MessageViewModel(IMessageService service)
        {
            Messenger.Default.Register<LoggedUser>(this, SaveLoggedUser);
            messageService = service;
            LoadCommands();
        }

        private void SaveLoggedUser(LoggedUser obj)
        {
            this.logged = obj;
        }

        private void LoadCommands()
        {
            
            DecryptCommand = new CustomCommand(DecryptMessage, null);
            LoadCommand = new CustomCommand(LoadData, null);
           
        }

        private void LoadData(Object obj)
        {
            var messageList = messageService.All(logged.username);
            Messages = new ObservableCollection<Message>(messageList);
        }

        public void DecryptMessage(Object o)
        {
            if (SelectedMessage!=null)
            {
                Messenger.Default.Send(messageService.Decrypt(SelectedMessage.id, logged.username));
                new NavService().NavigateTo("DecryptedMessage");
            }
        }

        public ObservableCollection<Message> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        private Message selectedMessage;

        public Message SelectedMessage
        {
            get
            {
                return selectedMessage;
            }
            set
            {
                selectedMessage = value;
                RaisePropertyChanged("SelectedMessage");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
