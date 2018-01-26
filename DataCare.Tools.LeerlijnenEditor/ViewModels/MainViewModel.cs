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
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataCare.Tools.LeerlijnenEditor.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public RelayCommand AddLeerlijnCommand { get; set; }
        public RelayCommand SaveXMLCommand { get; set; }
        public RelayCommand OpenXMLCommand { get; set; }
        public RelayCommand NewXMLCommand { get; set; }
        public RelayCommand DeleteLeerlijnCommand { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Auteur naam is verplicht!")]
        public string Auteur { get; set; }

        /// <summary>
        /// Voert ook de methode ImportFile uit.
        /// </summary>
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

        public MainViewModel()
        {
            this.AddLeerlijnCommand = new RelayCommand(o =>  {
                addEditWindow leerlijnWindow = new addEditWindow();
                leerlijnWindow.Title = "Toevoeg venster";
                leerlijnWindow.txtKop.Text = "Voeg leerlijn toe";
                leerlijnWindow.txtNaam.Text = "Naam";
                leerlijnWindow.button.Content = "Voeg toe";
                leerlijnWindow.ShowDialog();
                if (!string.IsNullOrWhiteSpace(leerlijnWindow.txtWaarde.Text)) AddLeerlijn(leerlijnWindow.txtWaarde.Text);
            }, o => true);

            this.DeleteLeerlijnCommand = new RelayCommand(o => {
                var result = o;
                ExecuteDeleteLeerlijn((LeerlijnViewModel)result);
            }, o => true);

            this.SaveXMLCommand = new RelayCommand(o =>
            { 
                SaveXML();
            }, o => true);

            this.OpenXMLCommand = new RelayCommand(o => {
                OpenXML();
            }, o => true);

            this.NewXMLCommand = new RelayCommand(o => {
                NewXML();
            }, o => true);

            if (Auteur == null) Auteur = "Naam van de auteur";
        }

        
        /// <summary>
        /// Haalt het leerlijnenpakket op en maakt een nieuw object aan.
        /// </summary>
        public void importFile()
        {
            if (FileOrPathName != null)
            {
                ImportLeerlijnenpakket leerlijnenpakket = new ImportLeerlijnenpakket();
                Leerlijnenpakket = leerlijnenpakket.GetLeerlijnenPakket(PathOfFile);

                LeerlijnenPakketVM = new LeerlijnenPakketViewModel();

                LeerlijnenPakketVM.Naam = Leerlijnenpakket.Naam;
                foreach (var leerlijnen in Leerlijnenpakket.Leerlijnen)
                {
                    LeerlijnenPakketVM.AddLeerlijnen(leerlijnen);
                }
            }

            NotifyPropertyChanged("LeerlijnenPakketVM");
        }

        /// <summary>
        /// Toont het scherm om een leerlijn toe te voegen en voegt deze toe aan het object.
        /// </summary>
        public void AddLeerlijn(string leerlijnNaam)
        {
            if (!string.IsNullOrWhiteSpace(leerlijnNaam))
            {
                if (LeerlijnenPakketVM != null)
                {
                    LeerlijnViewModel leerlijn = new LeerlijnViewModel();
                    leerlijn.Naam = leerlijnNaam;
                    LeerlijnenPakketVM.Leerlijnen.Add(leerlijn);
                }
            }
        }
        
       /// <summary>
       /// Verwijderd een leerlijn uit de collectie.
       /// </summary>
       /// <param name="leerlijn"></param>
        public void ExecuteDeleteLeerlijn(LeerlijnViewModel leerlijn)
        {
            if (leerlijn != null) LeerlijnenPakketVM.Leerlijnen.Remove(leerlijn);
        }

        /// <summary>
        /// Maakt een nieuwe XML aan.
        /// </summary>
        public void NewXML()
        {
            LeerlijnenPakketVM = new LeerlijnenPakketViewModel();
            LeerlijnViewModel leerlijn = new LeerlijnViewModel();
            leerlijn.Naam = "Nieuwe leerlijn";
            LeerlijnenPakketVM.Leerlijnen.Add(leerlijn);
            NotifyPropertyChanged("LeerlijnenPakketVM");
        }

        /// <summary>
        /// Opent een bestaande XML bestand
        /// </summary>
        public void OpenXML()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FileOrPathName = openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Genereert een nieuwe XML bestand
        /// </summary>
        public void SaveXML()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML files(.xml)|*.xml|all Files(*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                XDocument xDoc = new XDocument(
                new XElement("Leerlijnpackage",
                new XElement("MetaData", new XAttribute("GemaaktOp", DateTime.Now), new XAttribute("GemaaktDoorPersoon", Auteur)),
                new XElement("Leerlijn", new XAttribute("Naam", LeerlijnenPakketVM.Naam),
                fillLeerlijnXML()
                        )
                    )
                );
                xDoc.Save(fileName);
            }
        }

        /// <summary>
        /// Vult de leerlijnen in de XML
        /// </summary>
        /// <returns></returns>
        public XElement fillLeerlijnXML()
        {
            XElement vakGebieden = new XElement("Vakgebieden");

            foreach (var leerlijn in LeerlijnenPakketVM.Leerlijnen)
            {
                vakGebieden.Add(new XElement("Vakgebied", new XAttribute("Naam", leerlijn.Naam),
                    fillDeellijnXML(leerlijn)));
            }

            return vakGebieden;
        }

        /// <summary>
        /// Vult de deellijnen in de XML
        /// </summary>
        /// <param name="leerlijn"></param>
        /// <returns></returns>
        public XElement fillDeellijnXML(LeerlijnViewModel leerlijn)
        {
           if (leerlijn != null)
            {
                XElement items = new XElement("Items");
                foreach (var deellijn in leerlijn.Deellijnen)
                {
                    items.Add(new XElement("Item", new XAttribute("Naam", deellijn.Deelgebied),
                        fillNiveaDoelXML(deellijn)));
                }
                return items;
            }
            else return null;
        }

        /// <summary>
        /// Vult de niveaudoelen in de XML
        /// </summary>
        /// <param name="deellijn"></param>
        /// <returns></returns>
        public XElement fillNiveaDoelXML(DeellijnViewModel deellijn)
        {
            if (deellijn != null)
            {
                XElement subItems = new XElement("Subitems");

                foreach (var niveauDoel in deellijn.Niveaudoelen)
                {
                    subItems.Add(new XElement("Subitem", new XAttribute("Naam", niveauDoel.Doel),
                        new XAttribute("Niveau", niveauDoel.Niveau),
                        new XAttribute("IsHoofdDoelstelling", niveauDoel.IsHoofddoel)));
                }
                return subItems;
            }
            else return null;
        }
    }
}
