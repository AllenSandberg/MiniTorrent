using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
    /// Interaction logic for DLLWindow.xaml
    /// </summary>
    public partial class DLLWindow : Window
    {
        private string dllPath;
        public DLLWindow(string dllPath)
        {
            InitializeComponent();
            this.dllPath = dllPath;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textboxOutput.Text = CallDLL(textboxInput.Text.Trim());
        }

        public string CallDLL(string str)
        {
            var a = Assembly.LoadFrom(this.dllPath);
            foreach (Type type in a.GetExportedTypes())
            {
                dynamic t = Activator.CreateInstance(type);
                if (type.Name.Equals("Class1"))
                    return t.Reverse(str);
            }
            return "";
        }
    }
}
