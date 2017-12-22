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
    /// Interaction logic for addDeellijnWindow.xaml
    /// </summary>
    public partial class addDeellijnWindow : Window
    {
        private string deelgebied;
        public addDeellijnWindow()
        {
            InitializeComponent();
        }

        private void btnVoegToe_Click(object sender, RoutedEventArgs e)
        {
            deelgebied = deelgebiedNaam.Text;
            this.Close();
        }

        public string parseDeelgebied()
        {
            return deelgebied;
        }
    }
}
