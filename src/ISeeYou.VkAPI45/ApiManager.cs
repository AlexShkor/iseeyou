#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Exceptions;

#endregion

namespace VkAPIAsync
{
    /// <summary>
    ///     Класс для коммуникации с сервером VkontakteAPI
    /// </summary>
    public sealed class ApiManager
    {
        /// <summary>
        ///     Если последний запрос выполнен успешно - true, иначе false
        /// </summary>
        public bool MethodSuccessed;

        private string _apiResponseString;
        private XmlDocument _apiResponseXml;
        private ApiQueryBuilder _builder;

        /// <summary>
        /// Конструктор запросов
        /// </summary>
        public ApiQueryBuilder Builder
        {
            get { return _builder ?? (_builder = new ApiQueryBuilder()); }
            set { _builder = value; }
        }
        
        /// <summary>
        /// Флаг, который включает получение данных в формате JSON
        /// </summary>
        internal bool JSON { get; set; }

        /// <summary>
        ///     Задает название метода.
        /// </summary>
        /// <param name="methodName"> Название метода </param>
        public ApiManager Method(string methodName)
        {
            MethodSuccessed = false;
            Builder.Clear();
            Builder.Add("method", methodName);
            return this;
        }

        /// <summary>
        ///     Задает параметры метода
        /// </summary>
        /// <param name="key"> Имя параметра </param>
        /// <param name="value"> Значение параметра </param>
        public void Params(string key, object value)
        {
            Builder.Add(key, value.ToString());
        }

        /// <summary>
        ///     Выполняет метод
        /// </summary>
        public async Task<ApiManager> Execute(bool needAuth = true)
        {
            _apiResponseXml = null;
            string req = Builder.BuildQuery(needAuth);
            _apiResponseString = await ApiRequest.Send(req);
            if (!string.IsNullOrWhiteSpace(_apiResponseString) & !JSON)
            {
                _apiResponseXml = new XmlDocument();
                _apiResponseXml.LoadXml(_apiResponseString);
                XmlNode isError = _apiResponseXml.SelectSingleNode("/error");
                if (isError == null)
                {
                    MethodSuccessed = true;
                }
                else
                {
                    var code = Convert.ToInt32(isError.SelectSingleNode("error_code").InnerText);
                    var msg = isError.SelectSingleNode("error_msg").InnerText;
                    var ht = new Dictionary<string, string>();
                    var pparams = isError.SelectNodes("request_params/param");
                    foreach (XmlNode n in pparams)
                    {
                        ht[n.SelectSingleNode("key").InnerText] = n.SelectSingleNode("value").InnerText;
                    }

                    if (code == 14) //Captcha needed
                    {
                        var captchaSid = isError.SelectSingleNode("captcha_sid").InnerText;
                        var captchaImg = isError.SelectSingleNode("captcha_img").InnerText;
                        throw new CaptchaNeededException("Необходим ввод captcha", code, msg, new Hashtable(ht), captchaSid,
                                                         captchaImg);
                    }

                    throw new ApiRequestErrorException("Ошибка на сервере", code, msg, new Hashtable(ht));
                }
            }
            else if (!JSON)
            {
                throw new ApiRequestEmptyAnswerException(
                    "Сервер возвратил пустой ответ или истекло время ожидания ответа");
            }
            return this;
        }

        /// <summary>
        ///     Возвращает XmlNode, которая является корнем ответа сервера
        /// </summary>
        public XmlNode GetResponseXml()
        {
            return _apiResponseXml != null ? _apiResponseXml.SelectSingleNode("/response") : null;
        }

        /// <summary>
        ///     Возвращает строку-ответ сервера
        /// </summary>
        public string GetResponseString()
        {
            return _apiResponseString;
        }

        /// <summary>
        ///     Возвращает XmlDocument из строки
        /// </summary>
        /// <param name="str">Строка с XML </param>
        public XmlDocument GetXmlDocument(string str)
        {
            var x = new XmlDocument();
            x.LoadXml(str);
            return x;
        }

        /// <summary>
        ///     Возвращает XmlDocument
        /// </summary>
        public XmlDocument GetXmlDocument()
        {
            if (!JSON)
            {
                return GetXmlDocument(GetResponseString());
            }
            throw new Exception("Невозможно возвратить XmlDocument, JSON == true");
        }
    }
}