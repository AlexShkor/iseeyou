using System;
using System.Linq;
using System.Web;

namespace ISeeYou.Web
{
    public static class UrlUtility
    {
        public static string ApplicationBaseUrl
        {
            get
            {
                var httpContext = HttpContext.Current;
                var url = httpContext.Request.Url.AbsoluteUri.Replace(httpContext.Request.Url.PathAndQuery, "");
                if (httpContext.Request.Url.Host != "localhost")
                {
                    url = url.Replace(":" + httpContext.Request.Url.Port, "");
                }

                return url;
            }
        }

        public static string CurrentUrl
        {
            get { return ApplicationBaseUrl + HttpContext.Current.Request.Url.PathAndQuery; }
        }

        public static string LastSegment(string url)
        {
            return (new Uri(url).Segments).Last();
        }

        public static string ExtractVkUserId(string url)
        {
            for (int i = url.Length - 1; i > 0; i--)
            {
                if (!char.IsDigit(url, i))
                {
                    return url.Substring(i + 1);
                }
            }

            throw new Exception("wrong group url format");
        }


        public static string ExtractToken(string url)
        {
            var uri = new Uri(url.Replace("#", "?"));
            return HttpUtility.ParseQueryString(uri.Query).Get("access_token");
        }

        public static string ParseVkGroupId(string url)
        {
            var id = LastSegment(url);
            return id;
        }
    }
}