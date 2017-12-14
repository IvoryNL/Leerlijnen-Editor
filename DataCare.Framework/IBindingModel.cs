namespace DataCare.Framework
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public interface IBindingModel<Model> : IChangeTracking
    {
        Model ToModel();

        void BeginChanges();
    }

    // Marker interface: alles dat dit gebruikt heeft review/refactoring nodig.
    public interface IMustBeReviewedOrRefactored : IChangeTracking { }
}