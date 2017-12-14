namespace DataCare.Framework
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.FSharp.Core;

    public sealed class FSharpChoiceTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Choice1Template { get; set; }

        public DataTemplate Choice2Template { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is UIElement))
            {
                return base.SelectTemplate(item, container);
            }

            if (item.IsInstanceOfGenericType(typeof(FSharpChoice<,>)))
            {
                // we know typeof(item) is typeof(FSharpChoice<T,R>) for some T and R
                // but not which T and R, so we need reflection to find IsChoice1Of2
                if (item != null)
                {
                    PropertyInfo isChoice1Of2 = item.GetType().GetProperty("IsChoice1Of2");
                    var is1 = (bool)isChoice1Of2.GetValue(item, new object[] { });
                    if (is1)
                    {
                        return Choice1Template;
                    }

                    return Choice2Template;
                }
            }
            
            return base.SelectTemplate(item, container);
        }
    }

    public sealed class FSharpOptionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SomeTemplate { get; set; }

        public DataTemplate NoneTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is UIElement))
            {
                return base.SelectTemplate(item, container);
            }

            if (item.IsInstanceOfGenericType(typeof(FSharpOption<>)))
            {
                // we know typeof(item) is typeof(FSharpOption<T>) for some T
                // but not which T, so we need reflection to find get_IsSome
                if (item != null)
                {
                    MethodInfo getIsSome = item.GetType().GetMethod("get_IsSome");
                    var isSome = (bool)getIsSome.Invoke(item, new[] { item });
                    if (isSome)
                    {
                        return SomeTemplate;
                    }

                    return NoneTemplate;
                }
            }

            if (item == null)
            {
                return NoneTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }

    internal static class ObjectHelper
    {
        public static bool IsInstanceOfGenericType(this object item, Type genericType)
        {
            if (item != null)
            {
                Type type = item.GetType();
                while (type != null)
                {
                    if (type.IsGenericType &&
                        type.GetGenericTypeDefinition() == genericType)
                    {
                        return true;
                    }

                    type = type.BaseType;
                }
            }

            return false;
        }
    }
}