#region Using

using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

#endregion

namespace VkAPIAsync
{
    /// <summary>
    ///     Генератор запросов
    /// </summary>
    public class ApiQueryBuilder
    {
        private readonly Dictionary<string, string> _paramData;

        /// <summary>
        ///     Инициализирует генератор запросов
        /// </summary>
        /// <param name="apiId">API Id</param>
        /// <param name="si">Информация о сессии</param>
        public ApiQueryBuilder()
        {
            _paramData = new Dictionary<string, string>();
        }

        /// <summary>
        ///     Добавляет параметр в запрос
        /// </summary>
        /// <param name="key">Имя параметра</param>
        /// <param name="value">Значение параметра</param>
        public ApiQueryBuilder Add(string key, string value)
        {
            _paramData.Add(key, value);
            return this;
        }

        /// <summary>
        ///     Удаляет все параметры
        /// </summary>
        public void Clear()
        {
            _paramData.Clear();
        }

        /// <summary>
        ///     Возвращает готовую строку запроса
        /// </summary>
        public string BuildQuery(bool needAccessToken)
        {
            var sb = new StringBuilder("https://api.vk.com/method/" + _paramData["method"] + (!JSON ? ".xml?" : "?"));
            _paramData.Remove("method");

            if (!string.IsNullOrWhiteSpace(VkAPI.AccessToken))
            {
                _paramData.Add("access_token", VkAPI.AccessToken);
                _paramData.Add("api_id", VkAPI.AppId.ToString(CultureInfo.InvariantCulture));
            }
            else if (needAccessToken)
            {
                throw new Exception("Необходима авторизация");
            }

            _paramData.Add("v", VkAPI.Version);
            // Сортировка параметров
            var sortedParams = new List<KeyValuePair<string, string>>(_paramData);
            sortedParams.Sort(
                (keyfirst, keylast) => keyfirst.Key.CompareTo(keylast.Key)
                );

            sb.Append(sortedParams.Select(x => x.Key.ToString() + "=" + x.Value.ToString()).Aggregate((a, b) => a + "&" + b));
            return sb.ToString();
        }

        /// <summary>
        /// Флаг, который включает получение данных в формате JSON
        /// </summary>
        internal bool JSON { get; set; }
    }
}