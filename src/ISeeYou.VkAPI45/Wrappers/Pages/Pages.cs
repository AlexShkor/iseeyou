#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Photos;

#endregion

namespace VkAPIAsync.Wrappers.Pages
{
    /// <summary>
    /// Страницы
    /// </summary>
    public static class Pages
    {
        /// <summary>
        /// Возвращает информацию о вики-странице.
        /// </summary>
        /// <param name="pid">Идентификатор вики-страницы</param>
        /// <param name="title">Название страницы</param>
        /// <param name="oid">Идентификатор владельца вики-страницы</param>
        /// <param name="sitePreview">true — получаемая wiki страница является предпросмотром для прикрепленной ссылки. </param>
        /// <param name="global">true — требуется получить информацию о глобальной вики-странице.</param>
        /// <param name="needHTML">true — требуется вернуть html-представление страницы.</param>
        /// <param name="needSource">true — требуется вернуть содержимое страницы в вики-формате. </param>
        public static async Task<Page> Get(int oid, int pid, string title = null, bool? sitePreview = null,
                               bool? global = null, bool? needHTML = null, bool? needSource = null)
        {
            VkAPI.Manager.Method("pages.get");
            VkAPI.Manager.Params("owner_id", oid);
            VkAPI.Manager.Params("page_id", pid);

            if (title != null)
            {
                VkAPI.Manager.Params("title", title);
            }
            if (sitePreview != null)
            {
                VkAPI.Manager.Params("site_preview", sitePreview.Value ? 1 : 0);
            }
            if (global != null)
            {
                VkAPI.Manager.Params("global", global.Value ? 1 : 0);
            }
            if (needHTML != null)
            {
                VkAPI.Manager.Params("need_html", needHTML.Value ? 1 : 0);
            }
            if (needSource != null)
            {
                VkAPI.Manager.Params("need_source", needSource.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return new Page(result);
            }
            return null;
        }

        /// <summary>
        ///     Сохраняет текст вики-страницы.
        /// </summary>
        /// <param name="pid">ID вики-страницы. Вместо pid может быть передан параметр title - название вики-страницы. В этом случае если страницы с таким названием еще нет, она будет создана.</param>
        /// <param name="gid">ID группы, где создана страница. Вместо gid может быть передан параметр mid - id создателя вики-страницы. В этом случае произойдет обращение не к странице группы, а к одной из личных вики-страниц пользователя.</param>
        /// <param name="text">Новый текст страницы в вики-формате.</param>
        /// <param name="title">Название вики-страницы</param>
        /// <param name="userId">Идентификатор пользователя, создавшего вики-страницу</param>
        /// <returns>В случае успеха возвращает id созданной страницы. </returns>
        public static async Task<int> Save(int pid, int gid, string text, int userId, string title)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            VkAPI.Manager.Method("pages.save");
            VkAPI.Manager.Params("page_id", pid);
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("Text", text);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Сохраняет новые настройки доступа на чтение и редактирование вики-страницы.
        /// </summary>
        /// <param name="pid">ID вики-страницы.</param>
        /// <param name="gid">ID группы, где создана страница.</param>
        /// <param name="view">Значение настройки доступа на чтение; описание значений Вы можете узнать странице, посвященной методу pages.get.</param>
        /// <param name="edit">Значение настройки доступа на редактирование; описание значений Вы можете узнать странице, посвященной методу pages.get.</param>
        /// <param name="userId"> Идентификатор пользователя, создавшего вики-страницу</param>
        /// <returns>В случае успеха возвращает id страницы, доступ к которой был отредактирован. </returns>
        public static async Task<int> SaveAccess(int pid, int? gid = null, int? uid = null, AccessWikiPrivacy view = null, AccessWikiPrivacy edit = null)
        {
            VkAPI.Manager.Method("pages.saveAccess");
            VkAPI.Manager.Params("page_id", pid);
            
            if (gid != null)
                VkAPI.Manager.Params("group_id", gid);
            if (uid != null)
                VkAPI.Manager.Params("user_id", uid);
            if (view != null)
                VkAPI.Manager.Params("view", view.Value);
            if (edit != null)
                VkAPI.Manager.Params("edit", edit.Value);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Возвращает текст одной из старых версий страницы.
        /// </summary>
        /// <param name="hid">ID версии вики-страницы.</param>
        /// <param name="gid">ID группы, где создана страница.</param>
        /// <param name="needHTML">Определяет, требуется ли в ответе html-представление версии вики-страницы.</param>
        public static async Task<VersionPageObject> GetVersion(int hid, int? gid = null, int? uid = null, bool? needHTML = null)
        {
            VkAPI.Manager.Method("pages.getVersion");
            VkAPI.Manager.Params("version_id", hid);
            if(gid != null)
                VkAPI.Manager.Params("group_id", gid);
            if (uid != null)
                VkAPI.Manager.Params("user_id", uid);

            if (needHTML != null)
            {
                VkAPI.Manager.Params("need_html", needHTML.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return new VersionPageObject(result);
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список вики-страниц в группе.
        /// </summary>
        /// <param name="gid">ID группы, где создана страница. Если параметр не указывать, возвращается список всех страниц, созданных текущим пользователем.</param>
        /// <returns>Возвращает множество объектов page.</returns>
        public static async Task<List<Page>> GetTitles(int gid)
        {
            VkAPI.Manager.Method("pages.getTitles");
            VkAPI.Manager.Params("group_id", gid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("page");
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => new Page(x)).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Возвращает html-представление вики-разметки.
        /// </summary>
        /// <param name="text">Текст в вики-формате.</param>
        /// <param name="gid">Идентификатор группы, в контексте которой интерпретируется данная страница.</param>
        /// <returns>В случае успеха возвращает экранированный html, соответствующий вики-разметке. </returns>
        public static async Task<string> ParseWiki(string text, int? gid = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentNullException("text");
            }

            VkAPI.Manager.Method("pages.parseWiki");
            VkAPI.Manager.Params("text", text);

            if (gid != null)
            {
                VkAPI.Manager.Params("group_id", gid);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? result.InnerText : null;
        }

        /// <summary>
        /// Позволяет очистить кеш отдельных внешних страниц, которые могут быть прикреплены к записям ВКонтакте. После очистки кеша при последующем прикреплении ссылки к записи, данные о странице будут обновлены.
        /// </summary>
        /// <param name="url">Адрес страницы, закешированную версию которой необходимо очистить </param>
        public static async Task<bool> ClearCache(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("url");

            VkAPI.Manager.Method("pages.clearCache");
            VkAPI.Manager.Params("url", url);

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Возвращает список всех старых версий вики-страницы.
        /// </summary>
        /// <param name="pageId">Идентификатор вики-страницы </param>
        /// <param name="groupId">Идентификатор сообщества, которому принадлежит вики-страница </param>
        /// <param name="userId">Идентификатор пользователя, создавшего вики-страницу</param>
        /// <returns>Возвращает массив объектов page_version</returns>
        public static async Task<List<HistoryPageObject>> GetHistory(int pageId, int? groupId = null, int? userId = null)
        {
            VkAPI.Manager.Method("pages.getHistory");
            VkAPI.Manager.Params("page_id", pageId);

            if (groupId != null)
                VkAPI.Manager.Params("group_id", groupId);
            if (userId != null)
                VkAPI.Manager.Params("user_id", userId);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("page_version");
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => new HistoryPageObject(x)).ToList();
            }
            return null;
        }
    }
}