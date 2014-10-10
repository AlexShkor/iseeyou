#region Using

using System;

#endregion

namespace VkAPIAsync.Exceptions
{
    public class ApiRequestEmptyAnswerException : Exception
    {
        public ApiRequestEmptyAnswerException()
        {
        }

        public ApiRequestEmptyAnswerException(string message)
            : base(message)
        {
        }

        public ApiRequestEmptyAnswerException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public override string ToString()
        {
            return String.Format("Сервер возвратил пустой ответ");
        }
    }
}