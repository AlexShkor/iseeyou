#region Using

using System;
using System.Collections;

#endregion

namespace VkAPIAsync.Exceptions
{
    public class ApiRequestErrorException : Exception
    {
        public int Code;
        public string Description;
        public Hashtable ParamsPassed;

        public ApiRequestErrorException(string errDesc, int code, string description, Hashtable paramsPassed)
            : base(errDesc)
        {
            Code = code;
            Description = description;
            ParamsPassed = paramsPassed;
        }

        public ApiRequestErrorException()
        {
        }

        public ApiRequestErrorException(string message)
            : base(message)
        {
        }

        public ApiRequestErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public override string ToString()
        {
            return String.Format("Код ошибки: {0}, Сообщение: {1}", Code, Description);
        }
    }
}