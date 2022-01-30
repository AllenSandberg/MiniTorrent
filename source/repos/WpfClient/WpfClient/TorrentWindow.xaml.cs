using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for TorrentWindow.xaml
    /// </summary>
    public partial class TorrentWindow : Window
    {
        Peer peer;
        FileRequestListener fileListener;
        Thread downloadsDirectoryThread;
        Thread fileListenerThread;

        public TorrentWindow(Peer peer)
        {
            InitializeComponent();

            ConsoleAllocator.ShowConsoleWindow();

            this.peer = peer;
            Title = "MiniTorrent - " + peer.UserName;
            downloadsDirectoryThread = new Thread(new ThreadStart(UpdateDownloadsLoop));
            downloadsDirectoryThread.Start();


            fileListenerThread = new Thread(() =>
            {
                int portIn = Int32.Parse(Properties.Settings.Default.ClientPortIn);
                fileListener = new FileRequestListener(portIn);
                fileListener.StartListening();
            });
            fileListenerThread.Start();

            PublishFiles(false);
        }

        void UpdateDownloadsLoop()
        {
            while (true)
            {
                UpdateDownloadFolderGrid();
                Thread.Sleep(30000);
            }
        }

        private void UpdateDownloadFolderGrid()
        {
            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.DownloadsPath);
            FileInfo[] files = d.GetFiles();


            this.Dispatcher.Invoke(() =>
            {
                // Create the Grid
                DownloadsDirectoryGrid.Children.Clear();
                DownloadsDirectoryGrid.RowDefinitions.Clear();
                DownloadsDirectoryGrid.ColumnDefinitions.Clear();

                DownloadsDirectoryGrid.HorizontalAlignment = HorizontalAlignment.Left;
                DownloadsDirectoryGrid.VerticalAlignment = VerticalAlignment.Top;
                DownloadsDirectoryGrid.ShowGridLines = true;
                DownloadsDirectoryGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                //DownloadsDirectoryGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);

                // Create Columns
                ColumnDefinition gridCol1 = new ColumnDefinition { Width = GridLength.Auto };
                ColumnDefinition gridCol2 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                ColumnDefinition gridCol3 = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
                DownloadsDirectoryGrid.ColumnDefinitions.Add(gridCol1);
                DownloadsDirectoryGrid.ColumnDefinitions.Add(gridCol2);
                DownloadsDirectoryGrid.ColumnDefinitions.Add(gridCol3);

                DownloadsDirectoryGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });

                // Add first column header
                TextBlock txtBlock1 = new TextBlock();
                txtBlock1.Text = "File name";
                txtBlock1.FontSize = 14;
                txtBlock1.FontWeight = FontWeights.Bold;
                //txtBlock1.Foreground = new SolidColorBrush(Colors.Green);
                txtBlock1.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock1, 0);
                Grid.SetColumn(txtBlock1, 0);

                // Add second column header
                TextBlock txtBlock2 = new TextBlock();
                txtBlock2.Text = "File size";
                txtBlock2.FontSize = 14;
                txtBlock2.FontWeight = FontWeights.Bold;
                //txtBlock2.Foreground = new SolidColorBrush(Colors.Green);
                txtBlock2.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock2, 0);
                Grid.SetColumn(txtBlock2, 1);

                // Add third column header
                TextBlock txtBlock3 = new TextBlock();
                txtBlock3.Text = "Creation date";
                txtBlock3.FontSize = 14;
                txtBlock3.FontWeight = FontWeights.Bold;
                //txtBlock3.Foreground = new SolidColorBrush(Colors.Green);
                txtBlock3.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock3, 0);
                Grid.SetColumn(txtBlock3, 2);

                //// Add column headers to the Grid
                DownloadsDirectoryGrid.Children.Add(txtBlock1);
                DownloadsDirectoryGrid.Children.Add(txtBlock2);
                DownloadsDirectoryGrid.Children.Add(txtBlock3);

                int row = 1;
                foreach (FileInfo fileInfo in files)
                {
                    // Create Row
                    DownloadsDirectoryGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });

                    if (fileInfo.Name.CompareTo("ClassLibrary1.dll") != 0)
                    {
                        TextBlock filenameText = new TextBlock();
                        filenameText.Text = fileInfo.Name;
                        filenameText.FontSize = 12;
                        filenameText.FontWeight = FontWeights.Bold;
                        Grid.SetRow(filenameText, row);
                        Grid.SetColumn(filenameText, 0);
                        DownloadsDirectoryGrid.Children.Add(filenameText);
                    }
                    else
                    {
                        Button filenameText = new Button();
                        filenameText.Content = fileInfo.Name;
                        filenameText.FontSize = 12;
                        filenameText.FontWeight = FontWeights.Bold;
                        Grid.SetRow(filenameText, row);
                        Grid.SetColumn(filenameText, 0);
                        filenameText.Click += ShowDLL;
                        DownloadsDirectoryGrid.Children.Add(filenameText);
                    }
                    TextBlock fileSizeText = new TextBlock();
                    fileSizeText.Text = (fileInfo.Length / 100).ToString();
                    fileSizeText.FontSize = 12;
                    fileSizeText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(fileSizeText, row);
                    Grid.SetColumn(fileSizeText, 1);

                    TextBlock fileCreationDateText = new TextBlock();
                    fileCreationDateText.Text = fileInfo.CreationTime.ToString();
                    fileCreationDateText.FontSize = 12;
                    fileCreationDateText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(fileCreationDateText, row);
                    Grid.SetColumn(fileCreationDateText, 2);

                    // Add first row to Grid

                    DownloadsDirectoryGrid.Children.Add(fileSizeText);
                    DownloadsDirectoryGrid.Children.Add(fileCreationDateText);

                    row++;
                }
            });
        }

        private void ShowDLL(object sender, RoutedEventArgs e)
        {
            string filepath = System.IO.Path.Combine(Properties.Settings.Default.DownloadsPath, "ClassLibrary1.dll");
            DLLWindow dLLWindow = new DLLWindow(filepath);
            dLLWindow.ShowDialog();
        }

        private void UpdateTorrentFilesGrid(List<FileDetails> fileDetails)

        {

            this.Dispatcher.Invoke(() =>
            {
                // Create the Grid
                FileSearchResultsGrid.Children.Clear();
                FileSearchResultsGrid.RowDefinitions.Clear();
                FileSearchResultsGrid.ColumnDefinitions.Clear();


                FileSearchResultsGrid.HorizontalAlignment = HorizontalAlignment.Left;
                FileSearchResultsGrid.VerticalAlignment = VerticalAlignment.Top;
                FileSearchResultsGrid.ShowGridLines = true;
                FileSearchResultsGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                //FileSearchResultsGrid.Background = new SolidColorBrush(Colors.Black);

                // Create Columns
                FileSearchResultsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                FileSearchResultsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                FileSearchResultsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                FileSearchResultsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                FileSearchResultsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(35) });

                // Add first column header
                TextBlock txtBlock1 = new TextBlock();
                txtBlock1.Text = "File name";
                txtBlock1.FontSize = 14;
                txtBlock1.FontWeight = FontWeights.Bold;
                txtBlock1.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock1, 0);
                Grid.SetColumn(txtBlock1, 0);

                // Add second column header
                TextBlock txtBlock2 = new TextBlock();
                txtBlock2.Text = "File size";
                txtBlock2.FontSize = 14;
                txtBlock2.FontWeight = FontWeights.Bold;
                txtBlock2.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock2, 0);
                Grid.SetColumn(txtBlock2, 1);

                // Add third column header
                TextBlock txtBlock3 = new TextBlock();
                txtBlock3.Text = "Creation date";
                txtBlock3.FontSize = 14;
                txtBlock3.FontWeight = FontWeights.Bold;
                txtBlock3.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock3, 0);
                Grid.SetColumn(txtBlock3, 2);

                // Add fourth column header
                TextBlock txtBlock4 = new TextBlock();
                txtBlock4.Text = "";
                txtBlock4.FontSize = 14;
                txtBlock4.FontWeight = FontWeights.Bold;
                txtBlock4.VerticalAlignment = VerticalAlignment.Top;
                Grid.SetRow(txtBlock4, 0);
                Grid.SetColumn(txtBlock4, 3);

                //// Add column headers to the Grid
                FileSearchResultsGrid.Children.Add(txtBlock1);
                FileSearchResultsGrid.Children.Add(txtBlock2);
                FileSearchResultsGrid.Children.Add(txtBlock3);
                FileSearchResultsGrid.Children.Add(txtBlock4);

                int row = 1;
                foreach (FileDetails file in fileDetails)
                {
                    // Create Row
                    FileSearchResultsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });

                    TextBlock filenameText = new TextBlock();
                    if (file.Filename != null)
                        filenameText.Text = file.Filename;
                    else
                        filenameText.Text = "---";
                    filenameText.FontSize = 12;
                    filenameText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(filenameText, row);
                    Grid.SetColumn(filenameText, 0);

                    TextBlock fileSizeText = new TextBlock();
                    fileSizeText.Text = (file.FileSize).ToString() + "byte";
                    fileSizeText.FontSize = 12;
                    fileSizeText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(fileSizeText, row);
                    Grid.SetColumn(fileSizeText, 1);

                    TextBlock fileCreationDateText = new TextBlock();
                    if (file.PublishDate != null)
                        fileCreationDateText.Text = file.PublishDate.ToString();
                    else
                        fileCreationDateText.Text = "---";
                    fileCreationDateText.FontSize = 12;
                    fileCreationDateText.FontWeight = FontWeights.Bold;
                    Grid.SetRow(fileCreationDateText, row);
                    Grid.SetColumn(fileCreationDateText, 2);

                    DownloadButton downloadButton = new DownloadButton(peer, file, this);
                    downloadButton.Content = "Download";
                    downloadButton.Width = 50;
                    Grid.SetRow(downloadButton, row);
                    Grid.SetColumn(downloadButton, 3);
                    // Add first row to Grid
                    FileSearchResultsGrid.Children.Add(filenameText);
                    FileSearchResultsGrid.Children.Add(fileSizeText);
                    FileSearchResultsGrid.Children.Add(fileCreationDateText);
                    FileSearchResultsGrid.Children.Add(downloadButton);

                    row++;
                }
                LabelSearchResultCount.Content = fileDetails.Count + " files found";
            });
        }

        private void ButtonRefreshDirectory_Click(object sender, RoutedEventArgs e)
        {
            UpdateDownloadFolderGrid();
        }

        private void ButtonPublishFiles_Click(object sender, RoutedEventArgs e)
        {
            PublishFiles(true);
        }

        private void PublishFiles(bool showMessageBox)
        {
            // Clear files
            peer.ClearPublishedFiles();

            DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.DownloadsPath);
            FileInfo[] files = d.GetFiles();

            // Publish files one by one
            foreach (FileInfo fileInfo in files)
            {
                peer.PublishFileInformation(fileInfo);
            }

            if (showMessageBox)
                MessageBox.Show(this, "Published " + files.Count() + " files");
        }

        private void ButtonSubmitFileSearch_Click(object sender, RoutedEventArgs e)
        {
            string filenameToSearch = TextBoxFileSearch.Text.Trim();

            List<FileDetails> fileDetails = peer.FileRequest(filenameToSearch);
            List<FileDetails> fileDetailsToRemove = new List<FileDetails>();
            foreach (FileDetails file in fileDetails)
            {
                // only show files that the user doesnt have in his downloads table
                if (peer.UserFileExists(file.FileId))
                    fileDetailsToRemove.Add(file);
            }
            foreach (FileDetails fileToRemove in fileDetailsToRemove)
                fileDetails.Remove(fileToRemove);
            UpdateTorrentFilesGrid(fileDetails);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            peer.SignOut();
            Environment.Exit(Environment.ExitCode);
            System.Windows.Application.Current.Shutdown();
        }

        public void OnFileTransferProgressChangeEvent(object sender, FileTransferProgressEventArgs args)
        {
            Console.WriteLine(args.ID + " " + args.Progress);

            DownloadsAndUploadsPanel.Dispatcher.Invoke(() =>
            {
                //                DownloadControl downloadControl = (DownloadControl)DownloadsAndUploadsPanel.FindName(args.ID);
                DownloadControl downloadControl = (DownloadControl) LogicalTreeHelper.FindLogicalNode(DownloadsAndUploadsPanel, args.ID);
                if (downloadControl != null)
                    downloadControl.ProgressBar.Value = args.Progress * 100;
            });

            if (args.Progress == 1)
                new Thread(() =>
                {
                    Thread.Sleep(500);
                    PublishFiles(false);
                    this.Dispatcher.Invoke(() =>
                    {
                        ButtonSubmitFileSearch.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    });
                }).Start();
        }

        public class DownloadButton : Button
        {
            FileDetails fileDetails;
            Peer peer;
            TorrentWindow torrentWindow;
            public DownloadButton(Peer peer, FileDetails fileDetails, TorrentWindow torrentWindow)
            {
                this.torrentWindow = torrentWindow;
                this.fileDetails = fileDetails;
                this.peer = peer;
                Click += MyCustomClick;
            }

            protected void MyCustomClick(object sender, RoutedEventArgs e)
            {
                UserDetails userDetails = peer.GetFileOwnerDetails(fileDetails.FileId);
                if (userDetails != null)
                {
                    Content = userDetails.ToString();

                    new Thread(() =>
                    {
                        int RemotePort = userDetails.PortIn;
                        string RemoteIP = userDetails.IP;
                        string Filename = fileDetails.Filename;

                        string id = ("ID" + RemotePort + DateTime.Now.Ticks.ToString()).Trim();
                        ManualResetEvent m = new ManualResetEvent(false);
                        this.Dispatcher.Invoke(() =>
                        {
                            torrentWindow.DownloadsAndUploadsPanel.Children.Add(new DownloadControl(id, fileDetails, userDetails, true));
                            m.Set();
                        });
                        m.WaitOne();
                        FileRequest ac = new FileRequest(id, RemoteIP, RemotePort, Filename, fileDetails.FileSize);
                        ac.FileTransferProgressChangeEvent += torrentWindow.OnFileTransferProgressChangeEvent;
                        ac.StartFileDownload();

                    }).Start();
                }
                else
                {
                    // todo error
                }
            }
        }
    }


}
