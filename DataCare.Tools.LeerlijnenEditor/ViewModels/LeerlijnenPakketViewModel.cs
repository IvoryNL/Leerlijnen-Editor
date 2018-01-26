using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataCare.Model.Onderwijsinhoudelijk.Leerlijnen;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DataCare.Tools.LeerlijnenEditor.Commands;

namespace DataCare.Tools.LeerlijnenEditor.ViewModels
{
    public class LeerlijnenPakketViewModel : ViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Leerlijnpakket naam is verplicht!")]
        public string Naam { get; set; }
        public ObservableCollection<LeerlijnViewModel> Leerlijnen { get; set; }

        public LeerlijnenPakketViewModel()
        {
            Leerlijnen = new ObservableCollection<ViewModels.LeerlijnViewModel>();

            if (Naam == null) Naam = "Leerlijnen pakket";
        }
        
        /// <summary>
        /// Voegt de leerlijnen toe aan het object met daarbij de vulling van de deellijnen.
        /// </summary>
        /// <param name="leerlijn"></param>
        public void AddLeerlijnen(Leerlijn leerlijn)
        {
            if (leerlijn != null)
            {
                LeerlijnViewModel Leerlijn = new ViewModels.LeerlijnViewModel();

                Leerlijn.Naam = leerlijn.Vakgebied.Naam;
                foreach (var deellijn in leerlijn.Deellijnen)
                {
                    Leerlijn.ExecuteAddDeellijn(deellijn);
                }
                Leerlijnen.Add(Leerlijn);
            }
        }
    }
}
