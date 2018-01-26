using DataCare.Tools.LeerlijnenEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataCare.Tools.LeerlijnenEditor.ValidationRules
{
    public class NiveautredeValidationRule : ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int myInt = 0;

            try
            {
                var stringValue = value as string;

                if (!string.IsNullOrWhiteSpace(stringValue))
                { 
                    myInt = int.Parse(stringValue);
                
                    if (myInt > 0 && myInt <= 20)
                    {
                        return new ValidationResult(true, null);

                    }
                }
                return new ValidationResult(false,
                      "Niveau moet tussen de 1 en 20 zijn!");
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Niveau moet tussen de 1 en 20 zijn!");
            }
        }
    }
}
