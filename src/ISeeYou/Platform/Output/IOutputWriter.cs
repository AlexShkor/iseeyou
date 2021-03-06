using System;

namespace ISeeYou.Platform.Output
{
    public interface IOutputWriter
    {
        void Init(Action<string> callback);
        void Write(string format, params object[] args);
        void WriteLine(string format = "", params object[] args);
    }
}