using System;
using System.Linq;
using Hyl.Core.Configuration;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;

namespace Hyl.Core.Logs
{
    public class DefaultLog<T> : ILogs<T>
    {
        private ILog log;
        public DefaultLog(HylWebConfig config)
        {
            //DefaultLogHelper.InitDbInfo(config);
            log = LogManager.GetLogger(typeof(T));
        }

        public bool IsDebugEnabled => log.IsDebugEnabled;
        public bool IsInfoEnabled => log.IsInfoEnabled;
        public bool IsWarnEnabled => log.IsWarnEnabled;
        public bool IsErrorEnabled => log.IsErrorEnabled;
        public bool IsFatalEnabled => log.IsFatalEnabled;

        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void Info(object message)
        {
            log.Info(message);
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Fatal(object message)
        {
            log.Fatal(message);
        }
    }

    //public static class DefaultLogHelper
    //{
    //    private static bool IsSetDb { get; set; }

    //    public static void InitDbInfo(HylWebConfig config)
    //    {
    //        if (!IsSetDb && config.ForceToLog)
    //        {
    //            var hier = LogManager.GetRepository() as Hierarchy;
    //            if (hier != null)
    //            {
    //                var adoNetAppenders = hier.GetAppenders().OfType<AdoNetAppender>();
    //                foreach (var adoNetAppender in adoNetAppenders)
    //                {
    //                    adoNetAppender.ConnectionString = config.ConnectionString;
    //                    adoNetAppender.ActivateOptions();
    //                    IsSetDb = true;
    //                }
    //            }
    //        }

    //    }
    //}
}
