namespace DataCare.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public sealed class TemplateByTypeSelector : DataTemplateSelector
    {
        public TemplateByTypeSelector()
        {
            this.Templates = new List<TemplateForType>();
        }

        public List<TemplateForType> Templates { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is UIElement))
            {
                return base.SelectTemplate(item, container);
            }

            var itemType = item.GetType();
            var possibleTemplate = Templates.SingleOrDefault(mapping => mapping.Type.IsAssignableFrom(itemType));
            if (possibleTemplate != null)
            {
                return possibleTemplate.DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }

    public class TemplateForType
    {
        public Type Type { get; set; }
        public DataTemplate DataTemplate { get; set; }
    }
}