using Domain;
using front.Authentication;
using front.Messages;
using front.Navigation;
using front.Utility;
using Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace front.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        

        private LoggedUser currentUser;
        private IUserService service;

        public ICommand LoginCommand { get; set; }
        public ICommand SSOCommand { get; set; }

        public LoginViewModel(IUserService service)
        {
            this.service = service;
            LoadCommands();
            currentUser = new LoggedUser();

        }

        public void LoadCommands()
        {
            LoginCommand = new CustomCommand(Login, null);
            SSOCommand = new CustomCommand(null, null);
        }

        public void Login(Object obj)
        {
            currentUser = CurrentUser;
            String checker = service.Login(currentUser.username, currentUser.password);
            if (!checker.Equals("false"))
            {
                
                currentUser.accesstoken = checker;
                Messenger.Default.Send<LoggedUser>(currentUser);
                new NavService().NavigateTo("Main");
            }
        }

        public LoggedUser CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
