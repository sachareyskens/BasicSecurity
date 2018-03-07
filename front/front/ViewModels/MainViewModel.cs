using front.Authentication;
using front.Messages;
using front.Navigation;
using front.Utility;
using front.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace front.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        LoggedUser logged;
        public ICommand HomeCommand;
        public ICommand MessagesCommand;
        public ICommand SendMessageCommand;
        public ICommand MenuCommand { get; set; }

        public MainViewModel()
        {
            Messenger.Default.Register<LoggedUser>(this, SaveLoggedUser);
            LoadCommands();
        }

        private void SaveLoggedUser(LoggedUser obj)
        {
            this.logged = obj;
        }

        public void LoadCommands()
        {
            HomeCommand = new CustomCommand(NavigateHome, null);
            MessagesCommand = new CustomCommand(NavigateMessages, null);
            SendMessageCommand = new CustomCommand(NavigateSendMessage, null);
            MenuCommand = new RelayCommand<RadioButton>(ShowHideMenu, null);
        }
        public void ShowHideMenu(RadioButton obj)
        {
            var frame = (Frame)Window.Current.Content;
            var page = (MainPage)frame.Content;
            page.splitview().IsPaneOpen = !page.splitview().IsPaneOpen;
            obj.IsChecked = false;

        }

        public void NavigateHome(Object obj)
        {
            new NavService().NavigateTo("Home");
        }

        public void NavigateMessages(Object obj)
        {
            Messenger.Default.Send<LoggedUser>(logged);
            new NavService().NavigateTo("Messages");

        }

        public void NavigateSendMessage(Object obj)
        {
            new NavService().NavigateTo("SendMessage");
        }
    }
}
