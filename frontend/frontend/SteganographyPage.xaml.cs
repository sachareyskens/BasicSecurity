using System.Windows.Forms;
using frontend.Utils;
using System.Windows;
using System.Windows.Controls;
using System;

namespace frontend
{
    public partial class SteganographyPage : Page
    {
        private string basePath, implantPath, outputPathEmb, inputPath, outputPathExt;
        HomeWindow scherm;

        public SteganographyPage()
        {
            InitializeComponent();
            scherm = (HomeWindow)System.Windows.Application.Current.MainWindow;

        }

        private void selectImageToEmbedButton_Click(object sender, RoutedEventArgs e)
        {
            //Select Path
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Bitmap (*.bmp)|*.bmp";
            if (openFileDialog.ShowDialog() == true)
                basePath = openFileDialog.FileName;

            // UI
            //imageEmbed.Source = new BitmapImage(new Uri(basePath));
            imageEmbedUrlTextBlock.Text = basePath;

        }

        private void fileToEmbedButton_Click(object sender, RoutedEventArgs e)
        {
            //Select Path
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                implantPath = openFileDialog.FileName;

            // UI
            fileEmbedUrlTextBlock.Text = implantPath;
        }

        private void outputPathEmbedButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputPathEmb = dialog.SelectedPath;
            }


            outputPathEmbedUrlTextBlock.Text = outputPathEmb;
        }

        private void embedButton_Click(object sender, RoutedEventArgs e)
        {

            if (basePath != null && implantPath != null && outputPathEmb != null)
                SteganographyHelper.ImplantFile(basePath, implantPath, outputPathEmb + "\\Steganografied.bmp");


            System.Windows.Forms.MessageBox.Show("Saved at:" + outputPathEmb + "\\Steganografied.bmp");

        }

        private void selectImageToExtractbutton_Click(object sender, RoutedEventArgs e)
        {
            //Select Path
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Bitmap (*.bmp)|*.bmp";
            if (openFileDialog.ShowDialog() == true)
                inputPath = openFileDialog.FileName;

            // UI
            //imageEmbed.Source = new BitmapImage(new Uri(basePath));
            imageExtractUrlTextBlock.Text = inputPath;

        }

        private void outputPathButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                outputPathExt = dialog.SelectedPath;
            }


            outputPathExtractUrlTextBlock.Text = outputPathExt;
        }

        private void extractButton_Click(object sender, RoutedEventArgs e)
        {
            SteganographyHelper.ExtractFile(inputPath, outputPathExt + "\\secret" + extentionTextBox.Text);
            System.Windows.Forms.MessageBox.Show("Extracted to:" + outputPathExt + "\\secret" + extentionTextBox.Text);
        }

        private void MenuBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = menuBox.SelectedIndex;

            switch (index)
            {
                case 0:
                    scherm.displayFrame.Source = new Uri("HomePage.xaml", UriKind.Relative);
                    break;
                case 1:
                    scherm.displayFrame.Source = new Uri("MessagePage.xaml", UriKind.Relative);
                    break;
                case 2:
                    scherm.displayFrame.Source = new Uri("RecievedPage.xaml", UriKind.Relative);
                    break;
                case 3:
                    scherm.displayFrame.Source = new Uri("ChatboxPage.xaml", UriKind.Relative);
                    break;
                case 4:
                    scherm.displayFrame.Source = new Uri("StegenographyWindow.xaml", UriKind.Relative);
                    break;
                
                case 5:
                    scherm.displayFrame.Source = new Uri("LogoutPage.xaml", UriKind.Relative);
                    break;

            }
        }
    }
}
