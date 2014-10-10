using System;

namespace ISeeYou.Platform.Dispatching.Interfaces
{
    public interface IDispatcher
    {
        void Dispatch(Object message);
        void Dispatch(Object message, Action<Exception> exceptionObserver);
    }
}