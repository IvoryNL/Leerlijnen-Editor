// -----------------------------------------------------------------------
// <copyright file="SingleInstanceApplicationWrapper.cs" company="DataCare BV">
// </copyright>
// -----------------------------------------------------------------------

namespace DataCare.Infrastructure.WpfHelpers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading;
    using System.Windows;

    /// <summary>
    /// Single Instance Application wrapper for WPF
    /// </summary>
    public class SingleInstanceApplicationWrapper<T> where T : Application, IDisposable
    {
        private readonly ApplicationInstanceMonitor<bool> instanceMonitor;

        public SingleInstanceApplicationWrapper(Action<T> starter)
        {
            this.starter = starter;
            this.instanceMonitor = new ApplicationInstanceMonitor<bool>(typeof(T).FullName);
        }

        private T app;
        private readonly Action<T> starter;

        protected bool OnStartup()
        {
            this.app = Activator.CreateInstance<T>();
            this.starter(this.app);
            return false;
        }

        protected void OnStartupNextInstance(object sender, NewInstanceStartedEventArgs<bool> eventArgs)
        {
            this.app.Dispatcher.Invoke((Action)(() => this.app.MainWindow.Activate()));
        }

        public void Run(string[] args)
        {
            Contract.Assume(this.instanceMonitor != null);
            if (this.instanceMonitor.Start())
            {
                // this is the first instance
                this.instanceMonitor.NewInstanceStarted += OnStartupNextInstance;
                OnStartup();
            }
            else
            {
                // this is a later instance, defer to first instance
                this.instanceMonitor.NotifyNewInstance(true);
            }
        }

        public void Dispose()
        {
            if (instanceMonitor != null)
            {
                instanceMonitor.Dispose();
            }
        }
    }

    [ServiceContract]
    public interface IApplicationInstanceMonitor<T>
    {
        [OperationContract(IsOneWay = true)]
        void NotifyNewInstance(T message);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public sealed class ApplicationInstanceMonitor<T> : IApplicationInstanceMonitor<T>, IDisposable
    {
        public event EventHandler<NewInstanceStartedEventArgs<T>> NewInstanceStarted;

        private readonly string mutexName;
        private readonly Uri ipcUri;
        private readonly NetNamedPipeBinding binding;
        private Mutex processLock;
        private ServiceHost ipcServer;
        private ChannelFactory<IApplicationInstanceMonitor<T>> channelFactory;
        private IApplicationInstanceMonitor<T> ipcChannel;

        public ApplicationInstanceMonitor() :
            this(typeof(ApplicationInstanceMonitor<>).Assembly.FullName)
        {
        }

        public ApplicationInstanceMonitor(string mutexName)
            : this(mutexName, mutexName)
        {
        }

        public ApplicationInstanceMonitor(string mutexName, string ipcUriPath)
        {
            this.mutexName = mutexName;
            var builder = new UriBuilder();
            builder.Scheme = Uri.UriSchemeNetPipe;
            builder.Host = "localhost";
            builder.Path = ipcUriPath;

            this.ipcUri = builder.Uri;

            this.binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
        }

        public void Dispose()
        {
            if (this.processLock != null)
            {
                this.processLock.Close();
            }

            if (this.ipcServer != null)
            {
                this.ipcServer.Close();
            }

            if (this.channelFactory != null)
            {
                this.channelFactory.Close();
            }

            GC.SuppressFinalize(this);
        }

        public bool Start()
        {
            if (this.processLock != null)
            {
                throw new InvalidOperationException("Monitor already started");
            }

            bool first;
            this.processLock = new Mutex(true, this.mutexName, out first);

            if (first)
            {
                StartIpcServer();
            }
            else
            {
                ConnectToIpcServer();
            }

            return first;
        }

        private void StartIpcServer()
        {
            this.ipcServer = new ServiceHost(this, this.ipcUri);
            this.ipcServer.AddServiceEndpoint(typeof(IApplicationInstanceMonitor<T>), this.binding, this.ipcUri);

            this.ipcServer.Open();

            this.ipcChannel = this;
        }

        private void ConnectToIpcServer()
        {
            this.channelFactory = new ChannelFactory<IApplicationInstanceMonitor<T>>(this.binding, new EndpointAddress(this.ipcUri));
            this.ipcChannel = this.channelFactory.CreateChannel();
        }

        public void NotifyNewInstance(T message)
        {
            if (this.ipcChannel == null)
            {
                throw new InvalidOperationException("Another instance seems to be running, but could not be connected");
            }

            this.ipcChannel.NotifyNewInstance(message);
        }

        void IApplicationInstanceMonitor<T>.NotifyNewInstance(T message)
        {
            // This is the server side code
            if (NewInstanceStarted != null)
            {
                NewInstanceStarted(this, new NewInstanceStartedEventArgs<T>(message));
            }
        }
    }

    public class NewInstanceStartedEventArgs<T> : EventArgs
    {
        public NewInstanceStartedEventArgs(T message)
        {
            Message = message;
        }

        public T Message { get; private set; }
    }
}