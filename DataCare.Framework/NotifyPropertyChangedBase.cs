namespace DataCare.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reactive.Linq;
    using System.Windows.Input;

    public class Validator
    {
        private readonly Func<string> isValid;

        public Validator(Func<string> isValid)
        {
            this.isValid = isValid;
        }

        public string ErrorMessage { get; private set; }

        public bool HasError
        {
            get
            {
                return !string.IsNullOrEmpty(ErrorMessage);
            }
        }

        private void SetError(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ErrorMessage = message;
            }
        }

        public void Validate()
        {
            ErrorMessage = null;
            SetError(isValid());
        }
    }

    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged, IDataErrorInfo
    {
        private Dictionary<string, Validator> rules = new Dictionary<string, Validator>();

        public void AddValidationRule<T>(Expression<Func<T>> expression, Func<string> ruleDelegate)
        {
            var name = PropertyHelper.GetPropertyName(expression);
            rules.Add(name, new Validator(ruleDelegate));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isWaiting;

        public bool IsWaiting
        {
            get
            {
                return this.isWaiting;
            }

            private set
            {
                this.isWaiting = value;
                NotifyPropertyChanged(() => IsWaiting);
            }
        }

        public void NotifyPropertyChanged<Q>(Expression<Func<Q>> propertyExpression)
        {
            string propertyName = PropertyHelper.GetPropertyName(propertyExpression);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void NotifyPropertyChangedWithWaitCursor<Q>(Expression<Func<Q>> propertyExpression)
        {
            try
            {
                // ToDo: remove line after views bind Cursor to IsWaiting property
                Mouse.OverrideCursor = Cursors.Wait;

                IsWaiting = true;

                NotifyPropertyChanged(propertyExpression);
            }
            finally
            {
                // ToDo: remove line after views bind Cursor to IsWaiting property
                Mouse.OverrideCursor = null;

                IsWaiting = false;
            }
        }

        public string Error
        {
            get
            {
                var errors = from b in rules.Values where b.HasError select b.ErrorMessage;
                return string.Join("\n", errors);
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (rules.ContainsKey(columnName))
                {
                    rules[columnName].Validate();
                    NotifyPropertyChanged(() => Error);
                    NotifyPropertyChanged(() => HasErrors);
                    return rules[columnName].ErrorMessage;
                }

                return null;
            }
        }

        public bool HasErrors
        {
            get
            {
                return rules.Any(rule => rule.Value.HasError);
            }
        }

        public void Validate()
        {
            foreach (var key in rules.Keys)
            {
                rules[key].Validate();
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(key));
                }
            }

            NotifyPropertyChanged(() => HasErrors);
        }
    }
}