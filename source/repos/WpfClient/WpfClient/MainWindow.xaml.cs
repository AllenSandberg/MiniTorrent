using ServiceInterface;
using System;
using System.Collections.Generic;
using System.IO;
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
using WpfClient.Views;
using static WpfClient.ClientConfigurations;
using Newtonsoft.Json;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Peer peer;

        public MainWindow()
        {
            InitializeComponent();
            peer = new Peer();           
        }

        
        public bool CheckConfigurationsFile()
        {
            if (CheckConfigFile() != ConfigStatus.OK)
            {
                LabelConfigError.Content = "Configuration file error: " + CheckConfigFile().ToString();
                // TODO RED TEXT
                LabelConfigError.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                if (peer.CheckIPPortConflict())
                {
                    LabelConfigError.Content = "IP / PORT conflict";
                    LabelConfigError.Visibility = Visibility.Visible;
                    return false;
                }
                return true;
            }
        }

        public bool ConnectToServer()
        {
            LabelConfigError.Content = "Logging in...";
            // TODO BLACK TEXT
            LabelConfigError.Visibility = Visibility.Visible;

            // Login
            peer.SignIn();

            // Check login
            if (peer.UserId != -1)
            {
                LabelConfigError.Content = "Logged in with ID=" + peer.UserId;
                return true;
            }
            else
            {
                // TODO: RED TEXT
                LabelConfigError.Content = "Login failed";
                return false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void MyFrame0_Click(object sender, RoutedEventArgs e)
        {
            //WindowConfigurationDialog windowConfigurationDialog = new WindowConfigurationDialog();
            ////windowConfigurationDialog.ShowDialog();
            //windowConfigurationDialog.ShowDialog();
            //CheckConfigurationsFile();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckConfigurationsFile())
                if (ConnectToServer())
                {
                    // logged in
                    TorrentWindow torrentWindow = new TorrentWindow(peer);
                    torrentWindow.Show();

                    this.Close();
                }
                else
                {
                    // login failed
                }
                
        }

        private void ButtonPickDownloadsPath_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                string selectedPath = dialog.SelectedPath;
                if (selectedPath != null && Directory.Exists(selectedPath))
                {
                    Properties.Settings.Default.DownloadsPath = selectedPath;
                    
                }
            }
        }
    }
}
