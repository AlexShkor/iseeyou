#region Using

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using VkAPIAsync.Exceptions;

#endregion

namespace VkAPIAsync
{
    /// <summary>
    /// Класс для отправки HTTP запросов
    /// </summary>
    public static class ApiRequest
    {
        /// <summary>
        /// Запрос
        /// </summary>
        private static HttpWebRequest _request;

        /// <summary>
        /// Ответ
        /// </summary>
        private static HttpWebResponse _response;

        /// <summary>
        /// Таймаут запроса
        /// </summary>
        public static int Timeout = 15000;

        /// <summary>
        ///     Отправляет запрос на сервер
        /// </summary>
        /// <param name="url"> Адрес сервера для запроса </param>
        public static async Task<string> Send(string url)
        {
            try
            {
                _request = (HttpWebRequest) WebRequest.Create(new Uri(url));
                _request.Timeout = Timeout;
                _request.UserAgent = "VkAPI.NET " + VkAPI.Version;

                _response = (HttpWebResponse)(await _request.GetResponseAsync());

                Stream responseStream = _response.GetResponseStream();

                if (responseStream == null)
                {
                    throw new ApiRequestNullResult("Не получено никаких данных");
                }
                var sr = new StreamReader(responseStream);
         
                var result = await sr.ReadToEndAsync();

                _response.Close();
                responseStream.Dispose();
                sr.Dispose();

                return result;
            }
            catch (Exception e)
            {
                throw new ApiRequestNullResult("Неизвестная ошибка: " + e.Message);
            }
        }
    }
}