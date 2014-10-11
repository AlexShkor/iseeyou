#region Using

using System;
using System.Reflection;
using VkAPIAsync.Authorization;
using System.Linq;

#endregion

namespace VkAPIAsync
{
    /// <summary>
    /// API Вконтакте
    /// </summary>
    public static class VkAPI
    {
        private static ApiManager _manager;
        private static ApiAccessPermissions _permissions;
        private static string _accessToken;

        /// <summary>
        ///     Версия API
        /// </summary>
        public static string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(2); }
        }

        /// <summary>
        ///     Обьект для осуществления запросов к API
        /// </summary>
        public static ApiManager Manager
        {
            get
            {
                return _manager ?? (_manager = new ApiManager());
            }
            private set { _manager = value; }
        }

        /// <summary>
        ///     Идентификатор приложения Вконтакте
        /// </summary>
        public static int AppId { get; set; }

        /// <summary>
        ///     Ссылка на страницу авторизации
        /// </summary>
        public static string AuthPath
        {
            get
            {
                return "https://oauth.vk.com/authorize?" +
                       "client_id=" + AppId + "&" +
                       "redirect_uri=http://oauth.vk.com/blank.html&" +
                       (Permissions.Values.Any() ? "scope=" + Permissions + "&" : string.Empty) +
                       "response_type=token&" +
                       "display=popup";
            }
        }

        /// <summary>
        /// Разрешения
        /// </summary>
        public static ApiAccessPermissions Permissions
        {
            get { return _permissions ?? (_permissions = new ApiAccessPermissions()); }
            set { _permissions = value; }
        }

        /// <summary>
        /// Токен доступа
        /// </summary>
        public static string AccessToken
        {
            get
            {
                return _accessToken;
            }
            set
            {
                _accessToken = value;
            }
        }

        /// <summary>
        /// Время и дата, когда токен станет недействиельным
        /// </summary>
        public static DateTime SessionExpire { get; internal set; }

        /// <summary>
        /// Идентификатор пользователя, от имени которого был осуществлен вход
        /// </summary>
        public static int UserId { get; internal set; }
    }
}