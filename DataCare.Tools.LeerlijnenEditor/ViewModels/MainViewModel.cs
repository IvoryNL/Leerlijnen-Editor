using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCare.FileFormat.Leerlijnenpakket;
using DataCare.Model.Onderwijsinhoudelijk.Leerlijnen;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using DataCare.Tools.LeerlijnenEditor.Views;
using DataCare.Tools.LeerlijnenEditor.Commands;

namespace DataCare.Tools.LeerlijnenEditor.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand AddLeerlijn { get; set; }
        public RelayCommand AddDeellijn { get; set; }

        public string PathOfFile;
        public string FileOrPathName
        {
            get
            {
                return PathOfFile;
            }
            set
            {
                PathOfFile = value;
                importFile();
            }
        }
        public Leerlijnenpakket Leerlijnenpakket { get; set; }

        public LeerlijnenPakketViewModel LeerlijnenPakketVM { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            this.AddLeerlijn = new RelayCommand(o =>  {
                addLeerlijn();
            }, o => true);
            this.AddDeellijn = new RelayCommand(o => {
                var result = o.ToString();
                addDeellijn(result);
            }, o => true);
        }

        

        public void importFile()
        {
            ImportLeerlijnenpakket leerlijnenpakket = new ImportLeerlijnenpakket();
            Leerlijnenpakket = leerlijnenpakket.GetLeerlijnenPakket(PathOfFile);

            LeerlijnenPakketVM = new LeerlijnenPakketViewModel();

            LeerlijnenPakketVM.Naam = Leerlijnenpakket.Naam;
            foreach (var leerlijnen in Leerlijnenpakket.Leerlijnen)
            {
                LeerlijnenPakketVM.addLeerlijnen(leerlijnen);
            }

            NotifyPropertyChanged("LeerlijnenPakketVM");
        }

        public void addLeerlijn()
        {
            addLeerlijnWindow leerlijnWindow = new addLeerlijnWindow();
            leerlijnWindow.ShowDialog();
            if (leerlijnWindow.leerlijnNaam.Text != "" && LeerlijnenPakketVM != null)
            {
                LeerlijnViewModel leerlijn = new LeerlijnViewModel();
                leerlijn.Naam = leerlijnWindow.leerlijnNaam.Text;
                LeerlijnenPakketVM.Leerlijnen.Add(leerlijn);
            }
        }

        public void addDeellijn(string leerlijnNaam)
        {
            addDeellijnWindow deellijnWindow = new addDeellijnWindow();
            deellijnWindow.ShowDialog();
            DeellijnViewModel deellijn = new DeellijnViewModel();
            deellijn.Deelgebied = deellijnWindow.deelgebiedNaam.Text;
            var leerlijn = LeerlijnenPakketVM.Leerlijnen.FirstOrDefault(l => l.Naam == leerlijnNaam);
            leerlijn.Deellijnen.Add(deellijn);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
