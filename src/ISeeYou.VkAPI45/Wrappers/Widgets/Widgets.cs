#region Using

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace VkAPIAsync.Wrappers.Widgets
{
    /// <summary>
    /// Виджеты
    /// </summary>
    public static class Widgets
    {
        /// <summary>
        ///     Получает список страниц приложения/сайта, на которых установлен Виджет комментариев или «Мне нравится».
        ///     Данный метод может быть вызван без использования авторизационных данных (параметры session или access_token).
        /// </summary>
        /// <param name="widgetApiId">Идентификатор приложения/сайта, с которым инициализируются виджеты.</param>
        /// <param name="order">Тип сортировки страниц. Возможные значения: date, comments, likes, friend_likes. Значение по умолчанию - friend_likes.</param>
        /// <param name="period">Период выборки. Возможные значения: day, week, month, alltime. Значение по умолчанию - week.</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества результатов поиска. Значение по умолчанию - 0.</param>
        /// <param name="count">Количество страниц которое необходимо вернуть, 10-200. Значение по умолчанию - 10.</param>
        public static async Task<PagesInfo> GetPages(int widgetApiId, PagesSortOrder order = null, SelectPeriod period = null,
                                         int? offset = null,
                                         int? count = null)
        {
            VkAPI.Manager.Method("widgets.getPages");

            var apiManager = await VkAPI.Manager.Execute(false);
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes ? null : new PagesInfo(resp);
            }
            return null;
        }

        /// <summary>
        ///     Получает список комментариев к странице, оставленных через Виджет комментариев.
        ///     Данный метод может быть вызван без использования авторизационных данных (параметры session или access_token).
        /// </summary>
        /// <param name="widgetApiId">Идентификатор приложения/сайта, с которым инициализируются виджеты.</param>
        /// <param name="url">URL-адрес страницы</param>
        /// <param name="pageId">Внутренний идентификатор страницы в приложении/сайте (в случае, если для инициализации виджетов использовался параметр page_id)</param>
        /// <param name="order">Тип сортировки комментариев. Возможные значения: date, likes, last_comment. Значение по умолчанию - date.</param>
        /// <param name="fields">Перечисленные через запятую поля анкет, необходимые для получения. Если среди полей присутствует replies, будут возращены последние комментарии второго уровня для каждого комментария 1го уровня.</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества результатов поиска. Значение по умолчанию - 0.</param>
        /// <param name="count">Количество комментариев, которое необходимо вернуть, 10-200. Значение по умолчанию - 10.</param>
        public static async Task<CommentsInfo> GetComments(int widgetApiId, string url = null, int? pageId = null,
                                               CommentsSortOrder order = null, IEnumerable<string> fields = null,
                                               int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("widgets.getComments");
            VkAPI.Manager.Params("widget_api_id", widgetApiId);
            if (url != null)
            {
                VkAPI.Manager.Params("url", url);
            }
            if (pageId != null)
            {
                VkAPI.Manager.Params("page_id", pageId);
            }
            if (order != null)
            {
                VkAPI.Manager.Params("order", order.Value);
            }
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + "," + b));
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes ? null : new CommentsInfo(resp);
            }
            return null;
        }
    }
}