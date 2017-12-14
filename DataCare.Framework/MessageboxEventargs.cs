namespace DataCare.Framework
{
    using System;
    using System.Linq;

    public enum MessageboxType
    {
        Default,
        Question,
        QuestionWithCancel,
        Warning,
        Confirmation,
        Error,
    }

    public class MessageboxEventArgs : EventArgs
    {
        public MessageboxEventArgs(string message)
        {
            this.MessageBoxType = MessageboxType.Default;
            this.Message = message;
            this.Caption = string.Empty;
        }

        public MessageboxEventArgs(MessageboxType messageBoxType, string message, string caption)
        {
            this.MessageBoxType = messageBoxType;
            this.Message = message;
            this.Caption = caption;
        }

        public string Message { get; set; }
        public string Caption { get; set; }

        public MessageboxType MessageBoxType { get; private set; }

        public bool? MessageBoxResult { get; set; }
    }
}