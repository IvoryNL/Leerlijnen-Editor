namespace DataCare.Framework
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Common.Logging;

    public class Logger
    {
        private readonly ILog logger;

        private string ModuleMessage(string modulename, string classname, string methodname, string message)
        {
            string pattern;
            if (string.IsNullOrEmpty(modulename))
            {
                pattern = "{3}";
            }
            else if (string.IsNullOrEmpty(classname))
            {
                pattern = "[{0}] {3}";
            }
            else if (string.IsNullOrEmpty(methodname))
            {
                pattern = "[{0}.{1}] {3}";
            }
            else if (string.IsNullOrEmpty(message))
            {
                pattern = "[{0}.{1}.{2}]";
            }
            else
            {
                pattern = "[{0}.{1}.{2}] {3}";
            }

            return string.Format(pattern, modulename, classname, methodname, message);
        }

        private string ModuleMessage(string modulename, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (string.IsNullOrEmpty(modulename))
                {
                    return message;
                }

                return string.Format("[{0}] {1}", modulename, message);
            }

            return string.Empty;
        }

        /// <summary>
        /// Log een bericht als Info in de log
        /// </summary>
        /// <param name="modulename">Naam van de module</param>
        /// <param name="message">de te loggen tekst</param>
        [Conditional("DEBUG")]
        public void Trace(string modulename, string message)
        {
            if (this.logger != null)
            {
                this.logger.Trace(ModuleMessage(modulename, message));
            }
        }

        /// <summary>
        /// Log een bericht als Info in de log
        /// </summary>
        /// <param name="module">module</param>
        /// <param name="message">de te loggen tekst</param>
        public void Info(IName module, string message)
        {
            Info(module.Name, message);
        }

        /// <summary>
        /// Log een bericht als Info in de log
        /// </summary>
        /// <param name="modulename">Naam van de module</param>
        /// <param name="message">de te loggen tekst</param>
        public void Info(string modulename, string message)
        {
            if (this.logger != null)
            {
                this.logger.Info(ModuleMessage(modulename, message));
            }
        }

        /// <summary>
        /// Log een bericht als Debug in de log
        /// </summary>
        /// <param name="module">module</param>
        /// <param name="message">de te loggen tekst</param>
        [Conditional("DEBUG")]
        public void Debug(IName module, string message)
        {
            if (this.logger != null)
            {
                this.logger.Debug(ModuleMessage(module.Name, message));
            }
        }

        /// <summary>
        /// Log een bericht als Debug in de log
        /// </summary>
        /// <param name="modulename">Naam van de module</param>
        /// <param name="message">de te loggen tekst</param>
        [Conditional("DEBUG")]
        public void Debug(string modulename, string message)
        {
            if (this.logger != null)
            {
                this.logger.Debug(ModuleMessage(modulename, message));
            }
        }

        /// <summary>
        /// Log a text message with debug severity with its context in a specific class and method
        /// </summary>
        /// <param name="modulename">Name of module</param>
        /// <param name="classname">Name of class, can be derived by means of reflection, e.g. this.GetType().Name</param>
        /// <param name="methodname">Name of method, can be derived by means of reflection, e.g. MethodBase.GetCurrentMethod().Name</param>
        /// <param name="message">Text message to log</param>
        [Conditional("DEBUG")]
        public void Debug(string modulename, string classname, string methodname, string message)
        {
            if (this.logger != null)
            {
                this.logger.Debug(ModuleMessage(modulename, classname, methodname, message));
            }
        }

        /// <summary>
        /// Log een bericht als warning in de log
        /// </summary>
        /// <param name="modulename">Naam van de module</param>
        /// <param name="message">de te loggen tekst</param>
        public void Warning(string modulename, string message)
        {
            if (this.logger != null)
            {
                this.logger.Warn(ModuleMessage(modulename, message));
            }
        }

        /// <summary>
        /// Log een bericht als error in de log
        /// </summary>
        /// <param name="modulename">Naam van de module</param>
        /// <param name="message">de te loggen tekst</param>
        public void Error(string modulename, string message)
        {
            if (this.logger != null)
            {
                this.logger.Error(ModuleMessage(modulename, message));
            }
        }

        /// <summary>
        /// Log exception als error. Gebruik deze methode voor het loggen van exceptions die GEEN applicatie crash veroorzaken. Ander gebruik de fatal methode.
        /// </summary>
        /// <param name="modulename">Naam van de module</param>
        /// <param name="exception">De te loggen exception</param>
        public void Error(string modulename, Exception exception)
        {
            if (this.logger != null && exception != null)
            {
                Error(modulename, string.Format("{{{0}}} {1}", exception.GetType().Name, exception.Message));
                Error(string.Format(".{0}", modulename), exception.InnerException);
            }
        }

        /// <summary>
        /// Log een bericht als fatal in de log. Gebruik dit als bij applicatie crashes
        /// </summary>
        /// <param name="modulename">Naam van de module</param>
        /// <param name="message">de te loggen tekst</param>
        public void Fatal(string modulename, string message)
        {
            if (this.logger != null)
            {
                this.logger.Fatal(ModuleMessage(modulename, message));
            }
        }

        /// <summary>
        /// Log een exception als fatal in de log. Gebruik deze methode voor exception die een applicatie crash veroorzaken
        /// </summary>
        /// <param name="modulename">Naam van de module</param>
        /// <param name="exception">De te loggen exception</param>
        public void Fatal(string modulename, Exception exception)
        {
            if (this.logger != null && exception != null)
            {
                string exceptionFormat = string.Format("{0}   - Exception: {{{1}}} {2}", Environment.NewLine, exception.GetType().Name, exception.Message); 
                string innerExceptionFormat = string.Format("{0}   - Inner Exception: {1}", Environment.NewLine, exception.InnerException == null ? "null" : exception.InnerException.ToString());
                string stacktraceFormat = string.Format("{0}   - Stack Trace:{1}{2}", Environment.NewLine, Environment.NewLine, exception.StackTrace);

                string result = ModuleMessage(modulename, string.Format("{0}{1}{2}", exceptionFormat, innerExceptionFormat, stacktraceFormat));
                this.logger.Fatal(result);
            }
        }

        /// <summary>
        /// Creeer nieuwe instantie van
        /// </summary>
        /// <param name="logger">De instantie van ILog die gebruikt moet worden voor het loggen</param>
        public Logger(ILog logger)
        {
            this.logger = logger;
        }

        public Logger()
        {
        }
    }
}