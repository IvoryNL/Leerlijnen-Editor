namespace DataCare.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class CheckedTreeNode : NotifyPropertyChangedBase
    {
        private bool? isChecked;

        private Func<CheckedTreeNode, IEnumerable<CheckedTreeNode>> getChildren;
        private CheckedTreeNode parent;

        public CheckedTreeNode(object item, string displayName)
        {
            isChecked = false;
            this.DisplayName = displayName;
            this.Item = item;
        }

        public CheckedTreeNode(object item, string displayName, Func<CheckedTreeNode, IEnumerable<CheckedTreeNode>> getChildren)
            : this(item, displayName)
        {
            this.getChildren = getChildren;
        }

        public CheckedTreeNode(object item, string displayName, Func<CheckedTreeNode, IEnumerable<CheckedTreeNode>> getChildren, CheckedTreeNode parent)
            : this(item, displayName, getChildren)
        {
            this.parent = parent;
        }

        public CheckedTreeNode(object item, string displayName, CheckedTreeNode parent)
            : this(item, displayName)
        {
            this.parent = parent;
        }

        public string DisplayName { get; protected set; }

        public bool HasChildren
        {
            get
            {
                return Children != null && Children.Any();
            }
        }

        private ObservableCollection<CheckedTreeNode> children = null;

        public ObservableCollection<CheckedTreeNode> Children
        {
            get
            {
                if (children == null && getChildren != null)
                {
                    children = new ObservableCollection<CheckedTreeNode>(getChildren(this));
                    NotifyPropertyChanged(() => Children);
                }

                return children;
            }
        }

        public void SetChildren(IEnumerable<CheckedTreeNode> children)
        {
            this.children = new ObservableCollection<CheckedTreeNode>(children);
        }

        private object item;

        public object Item
        {
            get
            {
                return item;
            }

            private set
            {
                this.item = value;
                NotifyPropertyChanged(() => Item);
            }
        }

        public List<CheckedTreeNode> GetNodesOfType<T>()
        {
            return GetNodesOfType(typeof(T), this);
        }

        private List<CheckedTreeNode> GetNodesOfType(Type type, CheckedTreeNode node)
        {
            var result = new List<CheckedTreeNode>();

            if (node.Children != null && node.Children.Any())
            {
                foreach (var child in node.Children)
                {
                    result.AddRange(GetNodesOfType(type, child));
                }
            }
            else
            {
                if (node.Item.GetType() == type)
                {
                    result.Add(node);
                }
            }

            return result;
        }

        public bool? IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                SetIsChecked(value.Value);
            }
        }

        public void UpdateFromChild()
        {
            var newState = GetCheckedState();

            if (newState != isChecked)
            {
                this.isChecked = newState;
                NotifyPropertyChanged(() => IsChecked);
                if (parent != null)
                {
                    parent.UpdateFromChild();
                }
            }
        }

        private bool? GetCheckedState()
        {
            if (this.Children.All(child => child.IsChecked == true))
            {
                return true;
            }

            if (this.Children.All(child => child.IsChecked == false))
            {
                return false;
            }

            return null;
        }

        private void SetIsChecked(bool value)
        {
            if (value != isChecked)
            {
                this.isChecked = value;
                if (Children != null)
                {
                    foreach (var child in children)
                    {
                        if (child.IsChecked != value)
                        {
                            child.IsChecked = value;
                        }
                    }
                }

                if (parent != null)
                {
                    parent.UpdateFromChild();
                }

                NotifyPropertyChanged(() => IsChecked);
            }
        }
    }
}