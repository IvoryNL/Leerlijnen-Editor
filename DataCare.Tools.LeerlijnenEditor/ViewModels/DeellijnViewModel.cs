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
    public class DeellijnViewModel : PropertyValidateModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Deelgebied is verplicht!")]
        public string Deelgebied { get; set; }
        public ObservableCollection<NiveaudoelenViewModel> Niveaudoelen { get; set; }

        public DeellijnViewModel()
        {
            Niveaudoelen = new ObservableCollection<ViewModels.NiveaudoelenViewModel>();
        }

        public void addNiveaudoelen(DoelMetContext niveauMetDoel)
        {
            NiveaudoelenViewModel niveauEnDoel = new NiveaudoelenViewModel();

            niveauEnDoel.Niveau = niveauMetDoel.Niveau;
            niveauEnDoel.Doel = niveauMetDoel.Doel.Naam;
            niveauEnDoel.IsHoofddoel = niveauMetDoel.IsHoofddoel;

            Niveaudoelen.Add(niveauEnDoel);
        }
    }
}
