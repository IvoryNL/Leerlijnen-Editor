// -----------------------------------------------------------------------
// <copyright file="Attached.CommandBindings.cs" company="DataCare BV">
// </copyright>
// -----------------------------------------------------------------------

namespace DataCare.Framework
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Class to contain Attached properties
    /// </summary>
    public static class Attached
    {
        /// <summary>
        /// Allows databinding the CommandBindings of a view
        /// </summary>
        public static readonly DependencyProperty CommandBindingsProperty = DependencyProperty.RegisterAttached(
            "CommandBindings",
            typeof(CommandBindingCollection),
            typeof(Attached),
            new PropertyMetadata(OnCommandBindingsChanged));

        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static CommandBindingCollection GetCommandBindings(UIElement target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            return (CommandBindingCollection)target.GetValue(CommandBindingsProperty);
        }

        public static void SetCommandBindings(UIElement target, CommandBindingCollection value)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            target.SetValue(CommandBindingsProperty, value);
        }

        private static void OnCommandBindingsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var element = sender as UIElement;
            var newBindings = args.NewValue as CommandBindingCollection;
            if (newBindings != null && element != null)
            {
                element.CommandBindings.Clear();
                element.CommandBindings.AddRange(newBindings);
            }
            else
            {
                throw new InvalidOperationException("The CommandBindings attached property only supports types derived from UIElement");
            }
        }
    }
}