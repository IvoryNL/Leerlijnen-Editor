// -----------------------------------------------------------------------
// <copyright file="ImageExtensions.cs" company="DataCare BV">
// </copyright>
// -----------------------------------------------------------------------

namespace DataCare.Framework
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Helpers to create imagesources for icons
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Converts an Icon to an ImageSource for WPF
        /// </summary>
        /// <param name="icon">The icon</param>
        /// <returns>ImageSource for that icon</returns>
        public static ImageSource ToImageSource(this Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        /// <summary>
        /// Get an Image file from resource.
        /// </summary>
        /// <param name="namespacename">Project name / namespace</param>
        /// <param name="filepath">It searches in the /Images/ map, usage: 'folder'/'filename.png' or 'filename.png' </param>
        /// <returns>Object / Image</returns>
        public static object GetImageFromFileName(string namespacename, string filepath)
        {
            var dirBase = string.Format("pack://application:,,,/{0};component/Images/", namespacename);
            var fileUri = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}{1}", dirBase, filepath));

            return new System.Windows.Media.Imaging.BitmapImage(fileUri);
        }
    }
}