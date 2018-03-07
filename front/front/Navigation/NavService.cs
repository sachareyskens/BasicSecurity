using front.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace front.Navigation
{
    public class NavService
    {
        public void NavigateTo(string key)
        {
            var frame = (Frame)Window.Current.Content;

            if (key == "Main")
            {
                frame.Navigate(typeof(MainPage));
            }
            else
            {
                var page = (MainPage)frame.Content;

                switch (key)
                {
                    case "Messages":
                        page.getFrame().Navigate(typeof(MessagePage));
                        break;
                    case "DecryptedMessage":
                        page.getFrame().Navigate(typeof(DecryptedMessagePage));
                        break;
                    case "SendMessage":
                        page.getFrame().Navigate(typeof(SendMessagePage));
                        break;
                    case "Main":
                        page.getFrame().Navigate(typeof(MainPage));
                        break;
                    default:
                        page.getFrame().Navigate(typeof(HomePage));
                        break;
                }
            }
        }
    }
}
