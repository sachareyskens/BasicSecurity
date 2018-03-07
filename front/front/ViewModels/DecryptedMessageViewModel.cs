using Domain;
using front.Messages;
using front.Navigation;
using front.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace front.ViewModels
{
    public class DecryptedMessageViewModel : INotifyPropertyChanged
    {
        private Message currentMessage;

        public ICommand returnCommand;

        public DecryptedMessageViewModel()
        {
            Messenger.Default.Register<Message>(this, LoadMessage);
            LoadCommands();
        }

        private void LoadMessage(Message m)
        {
            CurrentMessage = m;
        }

        private void LoadCommands()
        {
            returnCommand = new CustomCommand(Return, null);

        }
        public Message CurrentMessage
        {
            get
            {
                return currentMessage;
            }
            set
            {
                currentMessage = value;
                RaisePropertyChanged("CurrentMessage");
            }
        }

        public void Return(Object obj)
        {
            new NavService().NavigateTo("Messages");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string v)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
    }
}
