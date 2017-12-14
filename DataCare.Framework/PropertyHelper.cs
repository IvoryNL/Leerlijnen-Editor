namespace DataCare.Framework
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class PropertyHelper
    {
        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            return GetPropertyMember(propertyExpression).Member.Name;
        }

        public static MemberExpression GetPropertyMember<T>(Expression<Func<T>> propertyExpression)
        {
            MemberExpression propertyMember;

            if (propertyExpression == null)
            {
                throw new ArgumentNullException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Property expression can not be null"),
                    @"propertyExpression");
            }

            if (propertyExpression.Body is UnaryExpression)
            {
                propertyMember = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
            }
            else
            {
                propertyMember = propertyExpression.Body as MemberExpression;
            }

            if (propertyMember == null || !(propertyMember.Member is PropertyInfo))
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Property expression needs to be of the form '() => Property' or '() => this.Property, expression is: {0}",
                        propertyExpression),
                    "propertyExpression");
            }

            return propertyMember;
        }
    }
}