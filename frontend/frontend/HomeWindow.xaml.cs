using frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace frontend
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public User user;
        public HomeWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            displayFrame.Navigated += DisplayFrame_Navigated;
            

            ChangeFrame();
        }

        private void DisplayFrame_Navigated(object sender, NavigationEventArgs e)
        {
            displayFrame.NavigationService.RemoveBackEntry();
        }

        // Laden van de eerste pagina in displayFrame
        // Navigatiebalk van de frame verbergen
        private void ChangeFrame()
        {
            displayFrame.Source = new Uri("HomePage.xaml", UriKind.Relative);
            displayFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }

        public User GetUser()
        {
            return this.user;
        }
    }
}
