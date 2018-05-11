using System;

namespace Hyl.Core.Logs
{
    public interface ILogs<T>
    {
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }

        
        bool IsFatalEnabled { get; }
        void Debug(object message);
        void Info(object message);
        void Warn(object message);
        void Error(object message);
        void Error(object message,Exception exception);
        void Fatal(object message);
    }
}
