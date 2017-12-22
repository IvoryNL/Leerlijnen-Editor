using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCare.Model.Onderwijsinhoudelijk.Leerlijnen;
using System.ComponentModel.DataAnnotations;

namespace DataCare.Tools.LeerlijnenEditor.ViewModels
{
    public class LeerlijnViewModel : PropertyValidateModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Leerlijn naam is verplicht!")]
        public string Naam { get; set; }
        public ObservableCollection<DeellijnViewModel> Deellijnen { get; set; }

        public LeerlijnViewModel()
        {
            Deellijnen = new ObservableCollection<ViewModels.DeellijnViewModel>();
        }

        public void addDeellijnen(Deellijn deellijn)
        {
            DeellijnViewModel Deellijn = new DeellijnViewModel();

            Deellijn.Deelgebied = deellijn.Deelgebied;
            foreach (var niveaudoel in deellijn.AlleNiveauDoelen)
            {
                Deellijn.addNiveaudoelen(niveaudoel);
            }
            Deellijnen.Add(Deellijn);
        }
    }
}
