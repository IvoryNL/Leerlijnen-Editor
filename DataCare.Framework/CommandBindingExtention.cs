namespace DataCare.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    public static class CommandBindingFactory
    {
        public static CommandBinding Create(ICommand command, Action execute, Func<bool> canExecute)
        {
            return new CommandBinding(
                command,
                (sender, args) => execute(),
                (sender, args) => args.CanExecute = canExecute());
        }

        public static CommandBinding Create(ICommand command, Action execute)
        {
            return new CommandBinding(command, (sender, args) => execute());
        }

        public static CommandBinding Create(ICommand command, ExecutedRoutedEventHandler execute, Func<bool> canExecute)
        {
            return new CommandBinding(command, execute, (sender, args) => args.CanExecute = canExecute());
        }
    }
}