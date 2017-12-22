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
using System.Windows.Shapes;

namespace DataCare.Tools.LeerlijnenEditor.Views
{
    /// <summary>
    /// Interaction logic for addWindow.xaml
    /// </summary>
    public partial class addLeerlijnWindow : Window
    {
        private string nieuweLeerlijn;
        public addLeerlijnWindow()
        {
            InitializeComponent();
        }

        private void btnVoegToe_Click(object sender, RoutedEventArgs e)
        {
            nieuweLeerlijn = this.leerlijnNaam.Text;
            this.Close();
        }

        public string parseNieuwLeerlijn()
        {
            return nieuweLeerlijn;
        }
    }
}
