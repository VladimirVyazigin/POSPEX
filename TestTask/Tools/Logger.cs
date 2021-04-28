using System;
using log4net;
using log4net.Config;
using log4net.Core;

namespace TestTask.Tools
{
    public class Logger
    {
        private ILog _log;

        static Logger()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        ///     Get logger for specified class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Logger Get<T>()
        {
            return new Logger {_log = LogManager.GetLogger(typeof(T))};
        }

        public static Logger Get(Type type)
        {
            return new Logger {_log = LogManager.GetLogger(type)};
        }

        /// <summary>
        ///     Log DEBUG message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Debug(object message, Exception exception = null)
        {
            if (exception == null)
                _log.Debug(message);
            else
                _log.Debug(message, exception);
        }

        /// <summary>
        ///     Log ERROR message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Error(object message, Exception exception = null)
        {
            _log.Logger.Repository.Threshold = Level.All;

            if (exception == null)
                _log.Error(message);
            else
                _log.Error(message, exception);
        }

        /// <summary>
        ///     Log FATAL message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Fatal(object message, Exception exception = null)
        {
            _log.Logger.Repository.Threshold = Level.All;
            if (exception == null)
                _log.Fatal(message);
            else
                _log.Fatal(message, exception);
        }

        /// <summary>
        ///     Log INFO message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Info(object message, Exception exception = null)
        {
            if (exception == null)
                _log.Info(message);
            else
                _log.Info(message, exception);
        }

        /// <summary>
        ///     Log WARN message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Warn(object message, Exception exception = null)
        {
            if (exception == null)
                _log.Warn(message);
            else
                _log.Warn(message, exception);
        }
    }
}