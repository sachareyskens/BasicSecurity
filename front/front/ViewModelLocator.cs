using front.ViewModels;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace front
{
    public class ViewModelLocator
    {
        private static String username = "basic_security";
        private static String password = "2018";

        #region Services
        private static IMessageService messageService = new MessageService(username, password);
        private static IUserService userService = new UserService(username, password);
        #endregion

        #region Views
        private MessageViewModel messageViewModel = new MessageViewModel(messageService);
        private MainViewModel mainViewModel = new MainViewModel();
        private DecryptedMessageViewModel decryptedMessageViewModel = new DecryptedMessageViewModel();
        private LoginViewModel loginViewModel = new LoginViewModel(userService);
        #endregion

        #region Properties
        public MessageViewModel MessageViewModel
        {
            get
            {
                return messageViewModel;
            }
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return mainViewModel;
            }
        }

        public DecryptedMessageViewModel DecryptedMessageViewModel
        {
            get
            {
                return decryptedMessageViewModel;
            }
        }

        public LoginViewModel LoginViewModel
        {
            get
            {
                return loginViewModel;
            }
        }
        #endregion

    }
}
