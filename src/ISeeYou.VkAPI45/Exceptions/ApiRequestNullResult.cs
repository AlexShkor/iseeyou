#region Using

using System;

#endregion

namespace VkAPIAsync.Exceptions
{
    public class ApiRequestNullResult : Exception
    {
        public ApiRequestNullResult(string message)
            : base(message)
        {
        }

        public ApiRequestNullResult(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}