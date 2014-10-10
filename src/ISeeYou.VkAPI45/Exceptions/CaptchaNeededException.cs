#region Using

using System;
using System.Collections;

#endregion

namespace VkAPIAsync.Exceptions
{
    /// <summary>
    /// Ошибка, связанная с вводом captcha
    /// </summary>
    public class CaptchaNeededException : ApiRequestErrorException
    {
        public CaptchaNeededException(string errDesc, int code, string description, Hashtable paramsPassed,
                                      string captchaSid, string captchaImg)
            : base(errDesc, code, description, paramsPassed)
        {
            CaptchaSid = captchaSid;
            CaptchaImg = captchaImg;
        }

        public CaptchaNeededException(string errDesc, int code, string description, Hashtable paramsPassed)
            : base(errDesc, code, description, paramsPassed)
        {
        }

        public CaptchaNeededException()
        {
        }

        public CaptchaNeededException(string message)
            : base(message)
        {
        }

        public CaptchaNeededException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        ///     Идентификатор captcha
        /// </summary>
        public string CaptchaSid { get; private set; }

        /// <summary>
        ///     Ссылка на изображение, которое нужно показать пользователю, чтобы он ввел текст с этого изображения
        /// </summary>
        public string CaptchaImg { get; private set; }

        public override string ToString()
        {
            return String.Format("Сode: {0}, message: {1}", Code, Description);
        }
    }
}