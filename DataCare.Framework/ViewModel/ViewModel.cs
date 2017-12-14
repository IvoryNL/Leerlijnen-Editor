namespace DataCare.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows.Input;

    public abstract class ViewModel : NotifyPropertyChangedBase, IViewModel, IDisposable
    {
        public event EventHandler<MessageboxEventArgs> OnShowMessagebox;

        protected bool? ShowMessagebox(MessageboxEventArgs e)
        {
            if (OnShowMessagebox != null)
            {
                OnShowMessagebox(this, e);
                return e.MessageBoxResult;
            }

            return null;
        }

        public bool IsActive { get; set; }

        internal void NavigateTo()
        {
            IsActive = true;
        }

        internal void NavigateFrom()
        {
            IsActive = false;
        }

        public virtual bool TryDeactivate()
        {
            this.ClearSubscriptions();
            return true;
        }

        public virtual void Activate()
        {
        
        }

        private Collection<IDisposable> subscriptions = new Collection<IDisposable>();

        protected Collection<IDisposable> Subscriptions
        {
            get
            {
                return subscriptions;
            }

            set
            {
                subscriptions = value;
            }
        }

        public virtual void Initialize()
        {
        }

        public virtual CommandBindingCollection ViewModelCommandBindings()
        {
            return new CommandBindingCollection();
        }

        private void ClearSubscriptions()
        {
            foreach (var subscription in Subscriptions)
            {
                subscription.Dispose();
            }

            Subscriptions.Clear();
        }

        public virtual void Dispose()
        {
            ClearSubscriptions();
        }

        private int? totaal;

        public int? Totaal
        {
            get
            {
                return totaal;
            }

            set
            {
                totaal = value;
                NotifyPropertyChanged(() => Totaal);
            }
        }

        private int? gefilterd;

        public int? Gefilterd
        {
            get
            {
                return gefilterd;
            }

            set
            {
                gefilterd = value;
                NotifyPropertyChanged(() => Gefilterd);
            }
        }

        protected void ShowSummaryCount<T1, T>(IObservable<IList<T1>> alleItems, IObservable<IList<T>> gefilterdeItems)
        {
            Subscriptions.Add(
                alleItems
                    .CombineLatest(
                        gefilterdeItems,
                        (alle, gefilterde) =>
                        {
                            var nieuwTotaal = (alle != null && alle.Any()) ? alle.Count : (int?)null;
                            var nieuwGefilterd = (nieuwTotaal != null && gefilterde != null && gefilterde.Any()) ? gefilterde.Count : (int?)null;

                            return new { nieuwTotaal, nieuwGefilterd };
                        })
                    .Subscribe(t =>
                    {
                        this.Totaal = t.nieuwTotaal;
                        this.Gefilterd = t.nieuwGefilterd;
                    }));
        }

        public virtual bool ShowsSameModelAs(ViewModel viewModel)
        {
            return object.ReferenceEquals(this, viewModel);
        }
    }
}