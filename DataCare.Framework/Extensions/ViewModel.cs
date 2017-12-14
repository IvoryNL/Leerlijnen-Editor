namespace DataCare.Framework.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;

    public static class ViewModelExtension
    {
        public static bool FilterText<T>(this T obj, Func<T, string> field, string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            string value = field(obj);
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            return value.ContainsStringIgnoreCaseAndDiacritics(filter);
        }

        public static bool ContainsStringIgnoreCaseAndDiacritics(this string container, string searchText)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(container, searchText, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) >= 0;
        }
    }
}