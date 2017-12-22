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
    public class NiveaudoelenViewModel : PropertyValidateModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Niveau is verplicht en moet tussen de 1 en 20 zijn!")]
        [Range(1, 20)]
        public int Niveau { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Doel is verplicht!")]
        public string Doel { get; set; }
        public bool IsHoofddoel { get; set; }
    }

    public class NiveautredeValidationRule : ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value,
            System.Globalization.CultureInfo cultureInfo)
        {
            NiveaudoelenViewModel doel = (value as BindingGroup).Items[0] as NiveaudoelenViewModel;
            if (doel.Niveau <=0 || doel.Niveau > 20)
            {
                return new System.Windows.Controls.ValidationResult(false,
                    "Niveau moet tussen de 1 en 20 zijn!");
            }
            else
            {
                return System.Windows.Controls.ValidationResult.ValidResult;
            }
        }
    }
}
