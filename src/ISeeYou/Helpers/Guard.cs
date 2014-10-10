using System;

namespace ISeeYou.Helpers
{
    public static class Guard
    {
        public static void Against(bool condition, string errorMessage)
        {
            if (condition) throw new ArgumentException(errorMessage);
        }

        public static void Against<TExceptionType>(bool condition, string errorMessage)
            where TExceptionType : Exception, new()
        {
            if (condition) throw (TExceptionType)Activator.CreateInstance(typeof(TExceptionType), errorMessage);
        }

        public static void Against<TExceptionType>(bool condition, string errorMessageFormat, params string[] args)
            where TExceptionType : Exception, new()
        {
            if (condition) throw (TExceptionType)Activator.CreateInstance(typeof(TExceptionType),
                String.Format(errorMessageFormat, args));
        }
    }
}