#region Using

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace VkAPIAsync.Utils
{
    /// <summary>
    /// Общие методы коммуникации
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// Загружает файл на сервер методом POST
        /// </summary>
        /// <param name="url">URL на который будет производится выгрузка</param>
        /// <param name="file">Путь к файлу на диске</param>
        /// <param name="paramName">Имя параметра</param>
        /// <param name="contentType">Тип контента</param>
        /// <returns>Ответ сервера</returns>
        public static async Task<string> HttpUploadFile(string url, string file, string paramName, string contentType)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            var wr = (HttpWebRequest) WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = CredentialCache.DefaultCredentials;

            Stream rs = await wr.GetRequestStreamAsync();

            //string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            await rs.WriteAsync(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"; save_big=\"1\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            await rs.WriteAsync(headerbytes, 0, headerbytes.Length);

            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                await rs.WriteAsync(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            await rs.WriteAsync(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = await wr.GetResponseAsync();
                Stream stream2 = wresp.GetResponseStream();
                var reader2 = new StreamReader(stream2);
                return await reader2.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                }
            }
            return null;
        }

        /// <summary>
        ///     Отправляет GET-запрос на сервер
        /// </summary>
        /// <param name="url"> Адрес сервера </param>
        /// <param name="cookies">Контейнер с Cookies</param>
        /// <returns> Ответ на запрос </returns>
        public static async Task<HttpWebResponse> HttpRequest(string url, CookieContainer cookies)
        {
            try
            {
                var myHttpwebrequest = (HttpWebRequest) WebRequest.Create(url);
                myHttpwebrequest.CookieContainer = cookies;
                var myHttpWebresponse = (HttpWebResponse)(await myHttpwebrequest.GetResponseAsync());
                cookies.Add(myHttpWebresponse.Cookies);
                return myHttpWebresponse;
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
    }
}