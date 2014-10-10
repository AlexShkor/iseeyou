#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace VkAPIAsync.Authorization
{
    /// <summary>
    /// Класс, который реализует выбор нужных приложению разрешений
    /// </summary>
    public class ApiAccessPermissions
    {
        private List<string> _values;

        /// <summary>
        /// Список разрешений
        /// </summary>
        public List<string> Values
        {
            get { return _values ?? (_values = new List<string>()); }
            private set { _values = value; }
        }

        /// <summary>
        ///     Пользователь разрешил отправлять ему уведомления.
        /// </summary>
        public ApiAccessPermissions Notify()
        {
            Values.Add("notify");
            return this;
        }

        /// <summary>
        ///     Доступ к друзьям.
        /// </summary>
        public ApiAccessPermissions Friends()
        {
            Values.Add("friends");
            return this;
        }

        /// <summary>
        ///     Доступ к фотографиям.
        /// </summary>
        public ApiAccessPermissions Photos()
        {
            Values.Add("photos");
            return this;
        }

        /// <summary>
        ///     Доступ к аудиозаписям.
        /// </summary>
        public ApiAccessPermissions Audio()
        {
            Values.Add("audio");
            return this;
        }

        /// <summary>
        ///     Доступ к видеозаписям.
        /// </summary>
        /// <returns></returns>
        public ApiAccessPermissions Video()
        {
            Values.Add("video");
            return this;
        }

        /// <summary>
        ///     Доступ к документам.
        /// </summary>
        public ApiAccessPermissions Docs()
        {
            Values.Add("docs");
            return this;
        }

        /// <summary>
        ///     Доступ заметкам пользователя.
        /// </summary>
        public ApiAccessPermissions Notes()
        {
            Values.Add("notes");
            return this;
        }

        /// <summary>
        ///     Доступ к wiki-страницам.
        /// </summary>
        public ApiAccessPermissions Pages()
        {
            Values.Add("pages");
            return this;
        }

        /// <summary>
        ///     Доступ к статусу пользователя.
        /// </summary>
        public ApiAccessPermissions Status()
        {
            Values.Add("status");
            return this;
        }

        /// <summary>
        ///     Доступ к обычным и расширенным методам работы со стеной.
        /// </summary>
        public ApiAccessPermissions Wall()
        {
            Values.Add("wall");
            return this;
        }

        /// <summary>
        ///     Доступ к группам пользователя.
        /// </summary>
        public ApiAccessPermissions Groups()
        {
            Values.Add("groups");
            return this;
        }

        /// <summary>
        ///     (для Standalone-приложений) Доступ к расширенным методам работы с сообщениями.
        /// </summary>
        public ApiAccessPermissions Messages()
        {
            Values.Add("messages");
            return this;
        }

        /// <summary>
        ///     Доступ к оповещениям об ответах пользователю.
        /// </summary>
        public ApiAccessPermissions Notifications()
        {
            Values.Add("notifications");
            return this;
        }

        /// <summary>
        ///     Доступ к статистике групп и приложений пользователя, администратором которых он является.
        /// </summary>
        public ApiAccessPermissions Stats()
        {
            Values.Add("stats");
            return this;
        }

        /// <summary>
        ///     Доступ к расширенным методам работы с рекламным API.
        /// </summary>
        public ApiAccessPermissions Ads()
        {
            Values.Add("ads");
            return this;
        }

        /// <summary>
        ///     Доступ к API в любое время со стороннего сервера.
        /// </summary>
        public ApiAccessPermissions Offline()
        {
            Values.Add("friends");
            return this;
        }

        /// <summary>
        ///     Возможность осуществлять запросы к API без HTTPS.
        ///     Внимание, данная возможность находится на этапе тестирования и может быть изменена.
        /// </summary>
        public ApiAccessPermissions NoHTTPS()
        {
            Values.Add("nohttps");
            return this;
        }

        public override string ToString()
        {
            if (Values.Count == 0)
            {
                Debug.WriteLine("Не указано ни одно разрешение");
            }
            return Values.Aggregate((a, b) => a + "," + b);
        }
    }
}