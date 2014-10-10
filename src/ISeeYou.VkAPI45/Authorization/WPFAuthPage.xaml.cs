#region Using

using System;
using System.Text.RegularExpressions;
using System.Windows.Navigation;

#endregion

namespace VkAPIAsync.Authorization
{
    /// <summary>
    ///     Логика страницы авторизации
    /// </summary>
    public partial class WpfAuthPage
    {
        public WpfAuthPage(WpfAuthWindow wnd)
        {
            Window = wnd;

            InitializeComponent();
            WebView.LoadCompleted += webView_LoadCompleted;
            WebView.Navigate(VkAPI.AuthPath);
        }

        public WpfAuthPage()
        {
            InitializeComponent();
            WebView.LoadCompleted += webView_LoadCompleted;
            WebView.Navigate(VkAPI.AuthPath);
        }

        /// <summary>
        /// Окно, в котором отображается страница (может быть null)
        /// </summary>
        protected WpfAuthWindow Window { get; set; }

        /// <summary>
        /// Событие успешной авторизации
        /// </summary>
        public event EventHandler Authorized;

        /// <summary>
        /// Событие, которое выпадает в случае ошибки во время авторизации
        /// </summary>
        public event EventHandler AuthFailed;

        private void webView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var uri = e.Uri;
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
            var authDone = false;
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
            if (!authDone) return;
            if (Authorized != null)
                Authorized(this, EventArgs.Empty);
            if (Window != null)
                Window.Close();
        }
    }
}