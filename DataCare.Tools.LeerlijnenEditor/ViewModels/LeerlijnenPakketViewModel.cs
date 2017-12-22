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

namespace DataCare.Tools.LeerlijnenEditor.ViewModels
{
    class LeerlijnenPakketViewModel : PropertyValidateModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Leerlijnpakket naam is verplicht!")]
        public string Naam { get; set; }
        public ObservableCollection<LeerlijnViewModel> Leerlijnen { get; set; }

        public LeerlijnenPakketViewModel()
        {
            Leerlijnen = new ObservableCollection<ViewModels.LeerlijnViewModel>();
        }

        

        public void addLeerlijnen(Leerlijn leerlijn)
        {
            LeerlijnViewModel Leerlijn = new ViewModels.LeerlijnViewModel();

            Leerlijn.Naam = leerlijn.Vakgebied.Naam;
            foreach (var deellijn in leerlijn.Deellijnen)
            {
                Leerlijn.addDeellijnen(deellijn);
            }
            Leerlijnen.Add(Leerlijn);
        }

  
    }

    public abstract class PropertyValidateModel : IDataErrorInfo, INotifyPropertyChanged
    {
        // check for general model error
        public string Error { get { return null; } }

        // check for property errors
        public string this[string columnName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();

                if (Validator.TryValidateProperty(
                        GetType().GetProperty(columnName).GetValue(this)
                        , new ValidationContext(this)
                        {
                            MemberName = columnName
                        }
                        , validationResults))
                    return null;

                return validationResults.First().ErrorMessage;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
