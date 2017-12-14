using System;

namespace DataCare.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using DataCare.Utilities;

    public class ItemsSelector<T> : ObservableCollection<CheckedListItem<T>>
    {
        public ItemsSelector()
        {
            CheckedItems = new ObservableCollection<T>();
        }

        public ItemsSelector(IEnumerable<T> allItems, IEnumerable<T> checkedItems) : this()
        {
            foreach (var item in allItems)
            {
                Add(item, checkedItems.Contains(item));
            }
        }

        public ItemsSelector(IEnumerable<T> allItems, IEnumerable<T> checkedItems, Func<T, string> displayName) : this()
        {
            foreach (var item in allItems)
            {
                Add(item, displayName, checkedItems.Contains(item));
            }
        }

        public ObservableCollection<T> CheckedItems { get; private set; }

        public void Add(T item, Func<T, string> displayName, bool isChecked)
        {
            Add(new CheckedListItem<T>(item, displayName(item), isChecked));
        }

        public void Add(T item, bool isChecked)
        {
            Add(new CheckedListItem<T>(item, isChecked));
        }

        public bool? AreAllChecked()
        {
            if (CheckedItems.Count == Items.Count)
            {
                return true;
            }

            if (CheckedItems.Count == 0)
            {
                return false;
            }
            return null;
        }

        public void UpdateItems(IEnumerable<T> newItems, Func<T, string> displayname, bool checkStateNewItems)
        {
            var updatedItems = newItems.GroupJoin(
                Items,
                target => target,
                source => source.Item,
                (target, source) =>
                {
                    if (source.Any())
                    {
                        var item = source.First();
                        item.PropertyChanged -= HandleItemSelected;
                        item.Item = target;
                        item.DisplayName = displayname(target);
                        return item;
                    }
                    return new CheckedListItem<T>(target, displayname(target), checkStateNewItems);
                });

            Items.Clear();
            Items.AddRange(updatedItems);
            UpdateCheckedItems();
        }

        public void SelectedAll()
        {
            foreach (var item in Items)
            {
                item.PropertyChanged -= HandleItemSelected;
                item.IsChecked = true;
                item.PropertyChanged += HandleItemSelected;
            }
            UpdateCheckedItems();
        }

        public void DeSelectedAll()
        {
            foreach (var item in Items)
            {
                item.PropertyChanged -= HandleItemSelected;
                item.IsChecked = false;
                item.PropertyChanged += HandleItemSelected;
            }
            UpdateCheckedItems();
        }

        public void UpdateItems(IEnumerable<T> newItems, Func<T, string> displayname, bool checkStateNewItems, IEqualityComparer<T> comparer)
        {
            var updatedItems = newItems.GroupJoin(
                 Items,
                 target => target,
                 source => source.Item,
                 (target, source) =>
                 {
                     if (source.Any())
                     {
                         var item = source.First();
                         item.Item = target;
                         return item;
                     }
                     return new CheckedListItem<T>(target, displayname(target), checkStateNewItems);
                 },
                 comparer).ToList();

            Items.Clear();
            Items.AddRange(updatedItems);
        }

        protected override void ClearItems()
        {
            foreach (var item in Items)
            {
                item.PropertyChanged -= HandleItemSelected;
            }

            CheckedItems.Clear();
            base.ClearItems();
        }

        protected override void InsertItem(int index, CheckedListItem<T> item)
        {
            base.InsertItem(index, item);
            AddItem(item);
        }

        protected override void RemoveItem(int index)
        {
            RemoveItem(Items[index]);
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, CheckedListItem<T> item)
        {
            RemoveItem(Items[index]);
            base.SetItem(index, item);
            AddItem(item);
        }

        private void RemoveItem(CheckedListItem<T> item)
        {
            item.PropertyChanged -= HandleItemSelected;
            CheckedItems.Remove(item.Item);
        }

        private void AddItem(CheckedListItem<T> item)
        {
            item.PropertyChanged += HandleItemSelected;
            UpdateCheckedItems();
        }

        private void HandleItemSelected(object sender, PropertyChangedEventArgs args)
        {
            var checkedItem = sender as CheckedListItem<T>;
            if (checkedItem.IsChecked == false)
            {
                CheckedItems.Remove(checkedItem.Item);
            }
            else
            {
                UpdateCheckedItems();
            }
        }

        private void UpdateCheckedItems()
        {
            var newItems = Items.Where(item => item.IsChecked == true).Select(item => item.Item).ToList();
            CheckedItems.Clear();
            CheckedItems.AddRange(newItems);
        }
    }
}