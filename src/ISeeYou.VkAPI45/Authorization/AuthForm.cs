#region Using

using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion

namespace VkAPIAsync.Authorization
{
    /// <summary>
    /// Логика формы авторизации
    /// </summary>
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
            webBrowser.Navigate(VkAPI.AuthPath);
        }

        /// <summary>
        /// Событие успешной авторизации
        /// </summary>
        public event EventHandler Authorized;

        /// <summary>
        /// Событие, которое выпадает в случае ошибки во время авторизации
        /// </summary>
        public event EventHandler AuthFailed;

        /// <summary>
        /// Обработчик события загрузки страницы
        /// </summary>
        /// <remarks>Проверяется наличие в URL страницы токена доступа</remarks>
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var uri = e.Url;
            if (uri.Host != "oauth.vk.com")
                return;

            var isError = new Regex("error").IsMatch(uri.Fragment);
            if (isError & AuthFailed != null)
            {
                AuthFailed(this, EventArgs.Empty);
            }
            else
            {
                throw new Exception("Ошибка при авторизации");
            }

            var accessTokenRegex = new Regex("access_token");
            var userIdRegex = new Regex("user_id");
            var expiresInRegex = new Regex("expires_in");
            var chunks = uri.Fragment.Split(new[] {"&"}, StringSplitOptions.None);
            var canCreateApi = false;
            foreach (string chunk in chunks)
            {
                if (accessTokenRegex.IsMatch(chunk))
                {
                    VkAPI.AccessToken = chunk.Replace("access_token=", "")
                                             .Replace("#", "")
                                             .Replace("&", "");
                    canCreateApi = true;
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
            if (!canCreateApi) return;
            if (Authorized != null)
                Authorized(this, EventArgs.Empty);
        }
    }
}