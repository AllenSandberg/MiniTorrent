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

namespace WpfClient
{
    /// <summary>
    /// Interaction logic for DownloadControl.xaml
    /// </summary>
    public partial class DownloadControl : UserControl
    {
        string id;

        public DownloadControl(string id, FileDetails fileDetails, UserDetails userDetails, bool isDownload)
        {
            InitializeComponent();
            this.id = id;
            this.Name = id;
            LabelFilename.Content = fileDetails.Filename;
            if (fileDetails.FileSize >= 1000000)
                LabelFileSize.Content = (fileDetails.FileSize / 1000000.0).ToString("0.00") + " MB";
            else if (fileDetails.FileSize < 1000000 && fileDetails.FileSize > 10000)
                LabelFileSize.Content = (fileDetails.FileSize / 1000.0).ToString("0.00") + " KB";
            else
                LabelFileSize.Content = fileDetails.FileSize + " B";
            
            LabelUploadOrDownload.Content = (isDownload)? "Download" : "Upload";
        }
    }
}
