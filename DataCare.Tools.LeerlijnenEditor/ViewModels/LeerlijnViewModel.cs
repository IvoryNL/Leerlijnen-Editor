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
    public class LeerlijnViewModel : ViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Leerlijn naam is verplicht!")]
        public string Naam { get; set; }
        public ObservableCollection<DeellijnViewModel> Deellijnen { get; set; }
        public RelayCommand AddDeellijnCommand { get; set; }
        public RelayCommand DeleteDeellijnCommand { get; set; }
        public RelayCommand RenameLeerlijnCommand { get; set; }

        public LeerlijnViewModel()
        {
            this.AddDeellijnCommand = new RelayCommand(o =>
            {
                addEditWindow deellijnWindow = new addEditWindow();
                deellijnWindow.Title = "Toevoeg venster";
                deellijnWindow.txtKop.Text = "Voeg deellijn toe";
                deellijnWindow.txtNaam.Text = "Deelgebied";
                deellijnWindow.button.Content = "Voeg toe";
                deellijnWindow.ShowDialog();
                if (!string.IsNullOrWhiteSpace(deellijnWindow.txtWaarde.Text)) AddDeellijn(deellijnWindow.txtWaarde.Text); ;
            }, o => true);

            this.DeleteDeellijnCommand = new RelayCommand(o => {
                ExecuteDeleteDeellijn((DeellijnViewModel)o);                
            }, o => true);

            this.RenameLeerlijnCommand = new RelayCommand(o => {
                var value = (LeerlijnViewModel)o;
                addEditWindow leerlijnWindow = new addEditWindow();
                leerlijnWindow.Title = "Wijzig venster";
                leerlijnWindow.txtKop.Text = "Wijzig leerlijn";
                leerlijnWindow.txtNaam.Text = "Naam";
                leerlijnWindow.button.Content = "Wijzig";
                if (value != null)
                {
                    leerlijnWindow.txtWaarde.Text = value.Naam;
                    leerlijnWindow.ShowDialog();
                    if (!string.IsNullOrWhiteSpace(leerlijnWindow.txtWaarde.Text)) RenameLeerlijn(leerlijnWindow.txtWaarde.Text);
                }
            });

            Deellijnen = new ObservableCollection<ViewModels.DeellijnViewModel>();
        }

        /// <summary>
        /// Voegt de deellijnen toe aan de verzameling met daarbij de vulling van de niveaudoelen.
        /// </summary>
        /// <param name="deellijn"></param>
        public void ExecuteAddDeellijn(Deellijn deellijn)
        {
            if (deellijn != null)
            {
                DeellijnViewModel Deellijn = new DeellijnViewModel();

                Deellijn.Deelgebied = deellijn.Deelgebied;
                foreach (var niveaudoel in deellijn.AlleNiveauDoelen)
                {
                    Deellijn.AddNiveaudoelen(niveaudoel);
                }
                Deellijnen.Add(Deellijn);
            }
        }

        /// <summary>
        /// Voegt de deellijn to aan de verzameling als deze wordt ingevoerd in het toevoeg scherm.
        /// </summary>
        public void AddDeellijn(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                AddDeellijnToCollection(value);
            }
        }

        /// <summary>
        /// Verwijderd een deellijn uit de collectie
        /// </summary>
        /// <param name="deellijn"></param>
        public void ExecuteDeleteDeellijn(DeellijnViewModel deellijn)
        {
            if (deellijn != null) Deellijnen.Remove(deellijn);
        }

        /// <summary>
        /// Voegt een deellijn toe aan de collectie
        /// </summary>
        /// <param name="deellijn"></param>
        public void AddDeellijnToCollection(string deellijn)
        {
            DeellijnViewModel deellijnViewModel = new DeellijnViewModel();
            deellijnViewModel.Deelgebied = deellijn;
            Deellijnen.Add(deellijnViewModel);
            NotifyPropertyChanged("Deellijnen");
        }

        /// <summary>
        /// Wijzigd de naam van de leerlijn
        /// </summary>
        /// <param name="leerlijnNaam"></param>
        public void RenameLeerlijn(string leerlijnNaam)
        {
            if (!string.IsNullOrWhiteSpace(leerlijnNaam))
            {
                Naam = leerlijnNaam;
            }
            NotifyPropertyChanged("Naam");
        }
    }
}
