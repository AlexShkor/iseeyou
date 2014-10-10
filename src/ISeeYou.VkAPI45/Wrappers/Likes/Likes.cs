#region Using

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Likes
{
    /// <summary>
    /// Лайки
    /// </summary>
    public static class Likes
    {
        /// <summary>
        ///     Добавляет указанный объект в список Мне нравится текущего пользователя.
        /// </summary>
        /// <param name="type">Идентификатор типа Like-объекта. Подробнее об идентификаторах объектов можно узнать на странице Список типов Like-объектов.</param>
        /// <param name="itemId">Идентификатор Like-объекта.</param>
        /// <param name="accessKey">Ключ доступа в случае работы с приватными объектами</param>
        /// <param name="ownerId">
        ///     Идентификатор владельца Like-объекта. Если параметр не задан, то считается, что он равен идентифкатору текущего пользователя.
        ///     В случае записей и комментариев на стене owner_id равен идентификатору страницы со стеной, а не автору записи.
        /// </param>
        /// <returns>В случае успеха возвращает объект с полем likes, в котором находится текущее количество пользователей, которые добавили данный объект в свой список Мне нравится. </returns>
        public static async Task<int> Add(LikeType type, int itemId, int? ownerId = null, string accessKey = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            VkAPI.Manager.Method("likes.add");
            VkAPI.Manager.Params("type", type.Value);
            VkAPI.Manager.Params("item_id", itemId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (accessKey != null)
            {
                VkAPI.Manager.Params("access_key", accessKey);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.Int("likes").HasValue ? result.Int("likes").Value : -1;
        }

        /// <summary>
        ///     Удаляет указанный объект из списка Мне нравится текущего пользователя.
        /// </summary>
        /// <param name="type">Идентификатор типа Like-объекта. Подробнее об идентификаторах объектов можно узнать на странице Список типов Like-объектов.</param>
        /// <param name="itemId">Идентификатор Like-объекта.</param>
        /// <param name="ownerId">
        ///     Идентификатор владельца Like-объекта. Если параметр не задан, то считается, что он равен идентифкатору текущего пользователя.
        ///     В случае записей и комментариев на стене owner_id равен идентификатору страницы со стеной, а не автору записи.
        /// </param>
        /// <returns>В случае успеха возвращает объект с полем likes, в котором находится текущее количество пользователей, которые добавили данный объект в свой список Мне нравится. </returns>
        public static async Task<int> Delete(LikeType type, int itemId, int? ownerId = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            VkAPI.Manager.Method("likes.delete");
            VkAPI.Manager.Params("type", type.Value);
            VkAPI.Manager.Params("item_id", itemId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.Int("likes").HasValue ? result.Int("likes").Value : -1;
        }

        /// <summary>
        ///     Получает список идентификаторов пользователей, которые добавили заданный объект в свой список Мне нравится.
        /// </summary>
        /// <param name="type">Тип Like-объекта. Подробнее о типах объектов можно узнать на странице Список типов Like-объектов.</param>
        /// <param name="ownerId">Идентификатор владельца Like-объекта (id пользователя или id приложения). Если параметр type равен sitepage, то в качестве owner_id необходимо передавать id приложения. Если параметр не задан, то считается, что он равен либо идентификатору текущего пользователя, либо идентификатору текущего приложения (если type равен sitepage).</param>
        /// <param name="itemId">Идентификатор Like-объекта. Если type равен sitepage, то параметр item_id может содержать значение параметра page_id, используемый при инициализации виджета «Мне нравится».</param>
        /// <param name="pageUrl">URL страницы, на которой установлен виджет «Мне нравится». Используется вместо параметра item_id.</param>
        /// <param name="filter">Указывает, следует ли вернуть всех пользователей, добавивших объект в список "Мне нравится" или только тех, которые рассказали о нем друзьям.</param>
        /// <param name="friendsOnly">Указывает, необходимо ли возвращать только пользователей, которые являются друзьями текущего пользователя.</param>
        /// <param name="offset">Смещение, относительно начала списка, для выборки определенного подмножества. Если параметр не задан, то считается, что он равен 0.</param>
        /// <param name="count">
        ///     Количество возвращаемых идентификаторов пользователей.
        ///     Если параметр не задан, то считается, что он равен 100, если не задан параметр friends_only, в противном случае 10.
        ///     Максимальное значение параметра 1000, если не задан параметр friends_only, в противном случае 100.
        /// </param>
        /// <returns>
        ///     В случае успеха возвращает объект со следующими полями:
        ///     count – общее количество пользователей, которые добавили заданный объект в свой список Мне нравится.
        ///     users – список индентификаторов пользователей с учетом параметров offset и count, которые добавили заданный объект в свой список Мне нравится.
        /// </returns>
        public static async Task<ListCount<int>> GetList(LikeType type, int? ownerId = null, int? itemId = null,
                                             string pageUrl = null, LikesFilter filter = null, bool? friendsOnly = null,
                                             int? offset = null, int? count = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            VkAPI.Manager.Method("likes.getList");
            VkAPI.Manager.Params("type", type.Value);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (pageUrl != null)
            {
                VkAPI.Manager.Params("page_url", pageUrl);
            }
            if (itemId != null)
            {
                VkAPI.Manager.Params("item_id", itemId);
            }
            if (filter != null)
            {
                VkAPI.Manager.Params("filter", filter);
            }
            if (friendsOnly != null)
            {
                VkAPI.Manager.Params("friends_only", friendsOnly.Value ? 1 : 0);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                {
                    var list = nodes.Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
                    return new ListCount<int>(result.Int("count").Value, list);
                }
            }
            return null;
        }

        /// <summary>
        ///     Проверяет находится ли объект в списке Мне нравится заданного пользователя.
        /// </summary>
        /// <param name="type">Идентификатор типа Like-объекта. Подробнее об идентификаторах объектов можно узнать на странице Список типов Like-объектов.</param>
        /// <param name="itemId">Идентификатор Like-объекта.</param>
        /// <param name="ownerId">Идентификатор владельца Like-объекта. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя.</param>
        /// <param name="userId">Идентификатор пользователя у которого необходимо проверить наличие объекта в списке Мне нравится. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя.</param>
        /// <returns>
        ///     В случае успеха возвращает одно из следующих числовых значений:
        ///     false – указанный Like-объект не входит в список Мне нравится пользователя с идентификатором user_id.
        ///     true – указанный Like-объект находится в списке Мне нравится пользователя с идентификатором user_id.
        /// </returns>
        public static async Task<bool> IsLiked(LikeType type, int itemId, int? ownerId = null, int? userId = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            VkAPI.Manager.Method("likes.isLiked");
            VkAPI.Manager.Params("type", type.Value);
            VkAPI.Manager.Params("item_id", itemId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (userId != null)
            {
                VkAPI.Manager.Params("user_id", userId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }
    }
}