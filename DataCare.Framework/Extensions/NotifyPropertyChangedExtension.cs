namespace DataCare.Framework.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public static class NotifyPropertyChangedExtension
    {
        private static IObservable<EventPattern<PropertyChangedEventArgs>> AsObservablePropertyChanged(this INotifyPropertyChanged propertyHolder)
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                handler => handler.Invoke,
                h => propertyHolder.PropertyChanged += h,
                h => propertyHolder.PropertyChanged -= h);
        }

        public static IObservable<T> AsObservable<T>(this INotifyPropertyChanged propertyHolder, Expression<Func<T>> propertyExpression)
        {
            var property = propertyExpression.Compile();
            IConnectableObservable<T> observable = propertyHolder.AsObservablePropertyChanged()
                .Where(p => p.EventArgs.PropertyName == PropertyHelper.GetPropertyName(propertyExpression))
                .Select(x => property())
                .Publish(property());

            observable.Connect();
            return observable;
        }
    }
}