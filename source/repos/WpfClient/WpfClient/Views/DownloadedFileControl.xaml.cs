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

namespace WpfClient.Views
{
    /// <summary>
    /// Interaction logic for DownloadedFileControl.xaml
    /// </summary>
    public partial class DownloadedFileControl : UserControl
    {
        public DownloadedFileControl(string fileName, string fileSize)
        {
            InitializeComponent();
            LabelFileName.Content = fileName;
            LabelFileSize.Content = fileSize;
        }
    }
}
