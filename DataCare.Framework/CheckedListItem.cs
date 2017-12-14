namespace DataCare.Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public interface ICheckedItem : INotifyPropertyChanged
    {
        object Item { get; }
        bool IsChecked { get; set; }
    }

    public interface ICheckedItem<out T> : ICheckedItem
    {
        new T Item { get; }
    }

    public static class CheckedItemHelpers
    {
        public static IEnumerable<ICheckedItem> CheckedItems(this IEnumerable<ICheckedItem> items)
        {
            return items.Where(item => item.IsChecked == true);
        }

        /// <summary>
        /// Combineer twee verschillende Lijsten tot een checklist item. Wanneer een item in beide lijsten voorkomt wordt het als
        /// checked gemarkeerd.
        /// </summary>
        public static IEnumerable<CheckedListItem<T>> Match<T>(this IEnumerable<T> allItems, IEnumerable<T> selectedItems)
        {
            return allItems.GroupJoin(
            selectedItems,
            all => all,
                selected => selected,
                (all, selected) => new CheckedListItem<T>(all, selected.Any()));
        }
    }

    public class CheckedListItem : NotifyPropertyChangedBase, ICheckedItem
    {
        private bool isReadOnly;
        private bool isChecked;
        private object item;

        public CheckedListItem(object item, string displayName, bool isChecked)
            : this(item, isChecked)
        {
            this.DisplayName = displayName;
        }

        public CheckedListItem(object item, bool isChecked)
        {
            this.item = item;
            this.isChecked = isChecked;
        }

        public CheckedListItem(object item)
            : this(item, false)
        {
        }

        private string displayName;

        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                displayName = value;
                NotifyPropertyChanged(() => Item);
            }
        }

        public object Item
        {
            get
            {
                return this.item;
            }

            set
            {
                this.item = value;
                NotifyPropertyChanged(() => Item);
            }
        }

        public virtual bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.isChecked = value;
                NotifyPropertyChanged(() => IsChecked);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }

            set
            {
                this.isReadOnly = value;
                NotifyPropertyChanged(() => IsReadOnly);
            }
        }

        public void SetIsChecked(bool value)
        {
            this.IsChecked = value;
        }
    }

    public class CheckedListItem<T> : NotifyPropertyChangedBase, ICheckedItem<T>
    {
        private bool isReadOnly;
        private bool isChecked;
        private T item;

        public CheckedListItem(T item, string displayName, bool isChecked)
            : this(item, isChecked)
        {
            this.displayName = displayName;
        }

        public CheckedListItem(T item, bool isChecked)
        {
            this.item = item;
            this.isChecked = isChecked;
        }

        public CheckedListItem(T item)
            : this(item, false)
        {
        }

        private string displayName;

        public string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                displayName = value;
                NotifyPropertyChanged(() => Item);
            }
        }

        public T Item
        {
            get
            {
                return this.item;
            }

            set
            {
                this.item = value;
                NotifyPropertyChanged(() => Item);
            }
        }

        object ICheckedItem.Item
        {
            get { return item; }
        }

        public virtual bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.isChecked = value;
                NotifyPropertyChanged(() => IsChecked);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }

            set
            {
                this.isReadOnly = value;
                NotifyPropertyChanged(() => IsReadOnly);
            }
        }

        public void SetIsChecked(bool value)
        {
            this.IsChecked = value;
        }
    }
}