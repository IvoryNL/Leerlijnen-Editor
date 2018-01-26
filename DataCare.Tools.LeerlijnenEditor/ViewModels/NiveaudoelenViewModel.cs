using DataCare.Tools.LeerlijnenEditor.ValidationRules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataCare.Tools.LeerlijnenEditor.ViewModels
{
    public class NiveaudoelenViewModel : ViewModel
    {
        public int Niveau { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Doel is verplicht!")]
        public string Doel { get; set; }
        public bool IsHoofddoel { get; set; }

        public NiveaudoelenViewModel()
        {
            if (Doel == null) Doel = "Doel";
        }
    }
}
