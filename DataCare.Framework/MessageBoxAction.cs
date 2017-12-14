namespace DataCare.Framework
{
    using System;
    using System.Linq;
    using System.Windows;

    public static class MessageBoxAction
    {
        public static bool? ShowMessageBox(MessageboxType type, string caption, string message)
        {
            MessageBoxResult result;

            switch (type)
            {
                default:
                    result = MessageBox.Show(message, caption);
                    break;

                case MessageboxType.Confirmation:
                    result = MessageBox.Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                    break;

                case MessageboxType.Error:
                    result = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Stop);
                    break;

                case MessageboxType.Question:
                    result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    break;

                case MessageboxType.QuestionWithCancel:
                    result = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    break;

                case MessageboxType.Warning:
                    result = MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    break;
            }

            switch (result)
            {
                case MessageBoxResult.Cancel:
                case MessageBoxResult.None:
                    return null;

                case MessageBoxResult.No:
                    return false;

                case MessageBoxResult.OK:
                case MessageBoxResult.Yes:
                    return true;
            }

            return null;
        }
    }
}