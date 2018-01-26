using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCare.Model.Onderwijsinhoudelijk.Leerlijnen;
using System.ComponentModel.DataAnnotations;
using DataCare.Tools.LeerlijnenEditor.Commands;
using DataCare.Tools.LeerlijnenEditor.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataCare.Tools.LeerlijnenEditor.ViewModels
{
    public class DeellijnViewModel : ViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Deelgebied is verplicht!")]
        public string Deelgebied { get; set; }
        public ObservableCollection<NiveaudoelenViewModel> Niveaudoelen { get; set; }
        public RelayCommand RenameDeellijnCommand { get; set; }

        public DeellijnViewModel()
        {
            this.RenameDeellijnCommand = new RelayCommand(o => {
                var value = (DeellijnViewModel)o;
                addEditWindow deellijnWindow = new addEditWindow();
                deellijnWindow.Title = "Wijzig venster";
                deellijnWindow.txtKop.Text = "Wijzig deellijn";
                deellijnWindow.txtNaam.Text = "Deelgebied";
                deellijnWindow.button.Content = "Wijzig";
                deellijnWindow.txtWaarde.Text = value.Deelgebied;
                deellijnWindow.ShowDialog();
                if (!string.IsNullOrWhiteSpace(deellijnWindow.txtWaarde.Text)) RenameDeellijn(deellijnWindow.txtWaarde.Text);
            }, o => true);

            Niveaudoelen = new ObservableCollection<ViewModels.NiveaudoelenViewModel>();

            if (Deelgebied == null) Deelgebied = "Deelgebied";
        }

        /// <summary>
        /// Wijzigd de naam van de deellijn
        /// </summary>
        /// <param name="deellijn"></param>
        public void RenameDeellijn(string deellijn)
        {
            if (!string.IsNullOrWhiteSpace(deellijn))
            {
                Deelgebied = deellijn;
            }

            NotifyPropertyChanged("Deelgebied");
        }

        /// <summary>
        /// Voegt de niveaudoelen toe
        /// </summary>
        /// <param name="niveauMetDoel"></param>
        public void AddNiveaudoelen(DoelMetContext niveauMetDoel)
        {
           if (niveauMetDoel != null)
            {
                NiveaudoelenViewModel niveauEnDoel = new NiveaudoelenViewModel();

                niveauEnDoel.Niveau = niveauMetDoel.Niveau;
                niveauEnDoel.Doel = niveauMetDoel.Doel.Naam;
                niveauEnDoel.IsHoofddoel = niveauMetDoel.IsHoofddoel;

                Niveaudoelen.Add(niveauEnDoel);
            }
        }
    }
}
