namespace ISeeYou.Platform.Dispatching.Interfaces
{
    public interface IMessageHandlerInterceptor
    {
        void Intercept(DispatcherInvocationContext context);
    }
}
