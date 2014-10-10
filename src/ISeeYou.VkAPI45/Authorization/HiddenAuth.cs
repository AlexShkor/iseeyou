#region Using

using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using VkAPIAsync.Utils;
using System.Threading.Tasks;

#endregion

namespace VkAPIAsync.Authorization
{
    /// <summary>
    /// Скрытая авторизация
    /// </summary>
    public class HiddenAuth
    {
        private static CookieContainer _cookies;

        /// <summary>
        ///     Контейнер с cookies
        /// </summary>
        private static CookieContainer Cookies
        {
            get { return _cookies ?? (_cookies = new CookieContainer()); }
            set { _cookies = value; }
        }

        /// <summary>
        ///     Событие успешного завершения авторизации
        /// </summary>
        public event EventHandler Authorized;

        /// <summary>
        ///     Авторизует пользователя в сети Вконтакте
        /// </summary>
        /// <param name="login">Моб. телефон или email</param>
        /// <param name="pass">Пароль</param>
        public async void Auth(string login, string pass)
        {
            //Выключение заголовка Expect
            ServicePointManager.Expect100Continue = false;

            //Получение документа со страницей авторизации
            HtmlDocument doc = await GetHtmlDocument(VkAPI.AuthPath);

            //Забираем со страницы нужные хешы
            string ipH = doc.DocumentNode.SelectSingleNode("//input[@name='ip_h']").Attributes["value"].Value;
            string to = doc.DocumentNode.SelectSingleNode("//input[@name='to']").Attributes["value"].Value;
            string origin = doc.DocumentNode.SelectSingleNode("//input[@name='_origin']").Attributes["value"].Value;

            //Отправляем POST со всеми данными на сервер авторизации
            HttpWebResponse response = await
                Post(
                    "ip_h=" + ipH + "&_origin=" + origin + "&to=" + to + "&expire=0" + "&email=" + login + "&pass=" +
                    pass, "https://login.vk.com/?act=login&soft=1", VkAPI.AuthPath);

            if (!TryGetToken(response.ResponseUri)) //Проверяем успешность авторизации
            {
                //Вконтакте просит подтверджение авторизации

                using (Stream stream = response.GetResponseStream())
                {
                    string source;
                    using (var sr = new StreamReader(stream))
                    {
                        source = sr.ReadToEnd(); //Получаем html страницы
                    }
                    using (var reader = new StringReader(source))
                    {
                        reader.ReadLine(); //Пропускаем 1 ненужную строчку с <xml>
                        doc = new HtmlDocument();
                        doc.LoadHtml(source); //Загружаем html в документ
                    }
                }
                string allowUrl = doc.DocumentNode.SelectSingleNode("//form[@action]").Attributes["action"].Value;

                //Делаем GET-запрос на полученный url
                response = await CommonUtils.HttpRequest(allowUrl, Cookies);

                //Проверяем успешность авторизации еще раз
                if (!TryGetToken(response.ResponseUri))
                    throw new Exception("Ошибка авторизации");
            }

            //Вызываем событие успешной авторизации
            if (Authorized != null)
                Authorized(this, new EventArgs());
        }

        /// <summary>
        ///     Пытается получить токен из URL
        /// </summary>
        /// <param name="respUri">URL</param>
        /// <returns>true - если удалось</returns>
        private bool TryGetToken(Uri respUri)
        {
            //Поиск ошибки
            bool isError = new Regex("error").IsMatch(respUri.Fragment);
            if (isError)
            {
                throw new Exception("Ошибка при авторизации");
            }

            //Инициализация регулярных выражений
            var accessTokenRegex = new Regex("access_token");
            var userIdRegex = new Regex("user_id");
            var expiresInRegex = new Regex("expires_in");

            //Разбиение url на куски
            string[] chunks = respUri.Fragment.Split(new[] {"&"}, StringSplitOptions.None);

            bool authDone = false; //Флаг успешной авторизации
            //Проход по всем кускам и поиск нужной информации
            foreach (string chunk in chunks)
            {
                if (accessTokenRegex.IsMatch(chunk))
                {
                    VkAPI.AccessToken = chunk.Replace("access_token=", "")
                                             .Replace("#", "")
                                             .Replace("&", "");
                    authDone = true;
                }
                if (userIdRegex.IsMatch(chunk))
                    VkAPI.UserId =
                        int.Parse(chunk.Replace("user_id=", "").Replace("#", "").Replace("&", ""));
                if (expiresInRegex.IsMatch(chunk))
                    VkAPI.SessionExpire =
                        (DateTime.Now + TimeSpan.FromSeconds(double.Parse(chunk.Replace("expires_in=", "")
                                                                               .Replace("#", "")
                                                                               .Replace("&", ""))));
            }
            return authDone;
        }

        /// <summary>
        ///     Возвращает HTML документ
        /// </summary>
        /// <param name="url">url страницы</param>
        private async Task<HtmlDocument> GetHtmlDocument(string url)
        {
            HtmlDocument doc;
            HttpWebResponse response = await CommonUtils.HttpRequest(url, Cookies);
            using (Stream stream = response.GetResponseStream())
            {
                string source;
                using (var sr = new StreamReader(stream))
                {
                    source = await sr.ReadToEndAsync(); //Получаем html страницы
                }
                using (var reader = new StringReader(source))
                {
                    reader.ReadLine(); //Пропускаем 1 ненужную строчку с <xml>
                    doc = new HtmlDocument();
                    doc.LoadHtml(source); //Загружаем html в документ
                }
            }
            return doc;
        }

        /// <summary>
        ///     Отравляет POST-запрос на сервер авторизации
        /// </summary>
        /// <param name="data">Строка, которая будет внедрена в запрос</param>
        /// <param name="url">Адрес сервера</param>
        /// <param name="referer">Значение для заголовка Referer</param>
        /// <returns>Ответ на запрос</returns>
        public static async Task<HttpWebResponse> Post(string data, string url, string referer)
        {
            //Создаем запрос
            var req = (HttpWebRequest) WebRequest.Create(url);

            //Отключаем автоперенаправление
            req.AllowAutoRedirect = false;

            //Инициализируем запрос
            req.Headers.Add("Origin", "https://oauth.vk.com");
            req.Headers.GetType()
               .InvokeMember("ChangeInternal",
                             BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null,
                             req.Headers, new object[] {"Connection", "keep-alive"});
            req.Headers.GetType()
               .InvokeMember("ChangeInternal",
                             BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null,
                             req.Headers, new object[] {"Cache-Control", "max-age=0"});
            req.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            req.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            req.Referer = referer;
            req.CookieContainer = Cookies;
            req.UserAgent =
                "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/28.0.1500.20 Safari/537.36";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Method = WebRequestMethods.Http.Post;
            req.Timeout = 100000;
            req.ContentType = "application/x-www-form-urlencoded";

            //Записываем data в RequestStream
            byte[] sentData = Encoding.UTF8.GetBytes(data);
            req.ContentLength = sentData.Length;
            using (Stream inputStream = req.GetRequestStream())
            {
                inputStream.Write(sentData, 0, data.Length);
            }

            //Получаем ответ
            var res = (HttpWebResponse)(await req.GetResponseAsync());

            //Сохраняем cookie
            Cookies.Add(res.Cookies);

            //Возвращаем ответ
            return res.Headers["Location"] != null ? await CommonUtils.HttpRequest(res.Headers["Location"], Cookies) : res;
        }
    }
}