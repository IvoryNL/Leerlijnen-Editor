using Microsoft.Win32;
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
using System.Xml.Linq;
using DataCare.Tools.LeerlijnenEditor.Views;
using DataCare.Tools.LeerlijnenEditor.ViewModels;
using System.Windows.Controls.Ribbon;

namespace DataCare.Tools.LeerlijnenEditor.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = "Leerlijnen editor";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
