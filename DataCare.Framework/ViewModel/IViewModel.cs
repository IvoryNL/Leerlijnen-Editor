namespace DataCare.Framework
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Windows.Input;

    // Note the attribute is on the untyped IViewModel interface, as open
    // generics are not supported by MEF in .Net 4.0
    [InheritedExport]
    public interface IViewModel
    {
        bool IsActive { get; set; }

        bool TryDeactivate();

        void Activate();

        CommandBindingCollection ViewModelCommandBindings();
    }
}