namespace DataCare.Framework
{
    using System;
    using System.Linq;
    using System.Windows;

    public class OpenWindowEventArgs : EventArgs
    {
        public bool IsDialog { get; set; }
        public Size Size { get; set; }

        /// <summary>
        /// Geeft heeft dialogresult als IsDialog op true is gezet.
        /// </summary>
        public bool? DialogResult { get; set; }

        public ViewModel ViewModel { get; set; }
    }
}