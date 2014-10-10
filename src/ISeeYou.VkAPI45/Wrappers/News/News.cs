#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Users;
using VkAPIAsync.Wrappers.Wall;

#endregion

namespace VkAPIAsync.Wrappers.News
{
    /// <summary>
    /// Новости
    /// </summary>
    public static class News
    {
        /// <summary>
        ///     Возвращает данные, необходимые для показа списка новостей для текущего пользователя.
        /// </summary>
        /// <param name="sourceIds">Перечисленные через запятую иcточники новостей, новости от которых необходимо получить.</param>
        /// <param name="filter">Перечисленные через запятую названия списков новостей, которые необходимо получить. </param>
        /// <param name="unixTimeStart">Время, в формате unixtime, начиная с которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным значению времени, которое было сутки назад.</param>
        /// <param name="unixTimeEnd">Время, в формате unixtime, до которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным текущему времени.</param>
        /// <param name="offset">Указывает, начиная с какого элемента в данном промежутке времени необходимо получить новости. по умолчанию 0.</param>
        /// <param name="count">Указывает, какое максимальное число новостей следует возвращать, но не более 100. По умолчанию 50. Для автоподгрузки Вы можете использовать возвращаемый данным методом параметр new_offset.</param>
        /// <param name="maxPhotos">Максимальное количество фотографий, информацию о которых необходимо вернуть. По умолчанию 5.</param>
        /// <param name="returnBanned">true - включить в выдачу также скрытых из новостей пользователей. false - не возвращать скрытых пользователей. </param>
        /// <param name="startFrom">Идентификатор, необходимый для получения следующей страницы результатов. Значение, необходимое для передачи в этом параметре, возвращается в поле ответа next_from. </param>
        public static async Task<NewsClassified> Get(IEnumerable<string> sourceIds = null, NewsType filter = null, int? unixTimeStart = null, int? unixTimeEnd = null,
                                         int? offset = null, string startFrom = null, int? count = null, int? maxPhotos = null, bool? returnBanned = null)
        {
            VkAPI.Manager.Method("newsfeed.get");
            if (sourceIds != null)
            {
                VkAPI.Manager.Params("source_ids", sourceIds.Aggregate((a,b) => a + "," + b));
            }
            if (filter != null)
            {
                VkAPI.Manager.Params("filters", filter.Value);
            }
            if (unixTimeStart != null)
            {
                VkAPI.Manager.Params("start_time", unixTimeStart.Value);
            }
            if (unixTimeEnd != null)
            {
                VkAPI.Manager.Params("end_time", unixTimeEnd.Value);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset.Value);
            }
            if (startFrom != null)
            {
                VkAPI.Manager.Params("start_from", startFrom);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (maxPhotos != null)
            {
                VkAPI.Manager.Params("max_photos", maxPhotos);
            }
            if (returnBanned != null)
            {
                VkAPI.Manager.Params("return_banned", returnBanned);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes ? null : new NewsClassified(resp);
            }
            return null;
        }

        /// <summary>
        /// Получает список новостей, рекомендованных пользователю
        /// </summary>
        /// <param name="unixTimeStart">Время в формате unixtime, начиная с которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным значению времени, которое было сутки назад. </param>
        /// <param name="unixTimeEnd">Время в формате unixtime, до которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным текущему времени. </param>
        /// <param name="offset">Указывает, начиная с какого элемента в данном промежутке времени необходимо получить новости. По умолчанию 0. Для автоподгрузки Вы можете использовать возвращаемый данным методом параметр new_offset</param>
        /// <param name="startFrom">Идентификатор, необходимый для получения следующей страницы результатов. Значение, необходимое для передачи в этом параметре, возвращается в поле ответа next_from. </param>
        /// <param name="count">Указывает, какое максимальное число новостей следует возвращать, но не более 100. По умолчанию 50</param>
        /// <param name="maxPhotos">Максимальное количество фотографий, информацию о которых необходимо вернуть. По умолчанию 5. </param>
        public static async Task<NewsClassified> GetRecommendations(int? unixTimeStart = null, int? unixTimeEnd = null,
                                         int? offset = null, string startFrom = null, int? count = null, int? maxPhotos = null)
        {
            VkAPI.Manager.Method("newsfeed.getRecommended");
            if (unixTimeStart != null)
            {
                VkAPI.Manager.Params("start_time", unixTimeStart.Value);
            }
            if (unixTimeEnd != null)
            {
                VkAPI.Manager.Params("end_time", unixTimeEnd.Value);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset.Value);
            }
            if (startFrom != null)
            {
                VkAPI.Manager.Params("start_from", startFrom);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (maxPhotos != null)
            {
                VkAPI.Manager.Params("max_photos", maxPhotos);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes ? null : new NewsClassified(resp);
            }
            return null;
        }

        /// <summary>
        /// Возвращает список записей пользователей на своих стенах, в которых упоминается указанный пользователь.
        /// </summary>
        /// <param name="ownerId">Идентификатор группы или сообщества </param>
        /// <param name="unixTimeStart">Время в формате unixtime начиная с которого следует получать упоминания о пользователе. 
        /// Если параметр не задан, то будут возвращены все упоминания о пользователе, если не задан параметр end_time, в противном случае упоминания с учетом параметра end_time. </param>
        /// <param name="unixTimeEnd">Время, в формате unixtime, до которого следует получать упоминания о пользователе. 
        /// Если параметр не задан, то будут возвращены все упоминания о пользователе, если не задан параметр start_time, в противном случае упоминания с учетом параметра start_time. </param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества новостей. По умолчанию 0. </param>
        /// <param name="count">Количество возвращаемых записей. Если параметр не задан, то считается, что он равен 20. Максимальное значение параметра 50. </param>
        public static async Task<ListCount<WallEntity>> GetMentions(int? ownerId = null, int? unixTimeStart = null, int? unixTimeEnd = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("newsfeed.getMentions");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (unixTimeStart != null)
            {
                VkAPI.Manager.Params("start_time", unixTimeStart.Value);
            }
            if (unixTimeEnd != null)
            {
                VkAPI.Manager.Params("end_time", unixTimeEnd.Value);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                return new ListCount<WallEntity>(resp.Int("count").Value, resp.SelectNodes("items/*").Cast<XmlNode>().Select(y => new WallEntity(y)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список пользователей и групп, которые текущий пользователь скрыл из ленты новостей.
        /// </summary>
        /// <returns>В случае успеха возвращает объект, в котором содержатся поля groups и members. В поле groups содержится массив идентификаторов групп, которые пользователь скрыл из ленты новостей. В поле members содержится массив идентификаторов друзей, которые пользователь скрыл из ленты новостей. </returns>
        public static async Task<BannedInfo> GetBanned()
        {
            VkAPI.Manager.Method("newsfeed.getBanned");

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new BannedInfo(result) : null;
        }

        /// <summary>
        ///     Возвращает список пользователей и групп, которые текущий пользователь скрыл из ленты новостей.
        /// </summary>
        /// <param name="fields">Поля профилей, которые необходимо вернуть. См. Описание полей параметра fields</param>
        /// <returns>В случае успеха возвращает объект, в котором содержатся поля groups и members. В поле groups содержится массив идентификаторов групп, которые пользователь скрыл из ленты новостей. В поле members содержится массив идентификаторов друзей, которые пользователь скрыл из ленты новостей. </returns>
        public static async Task<BannedInfoExtended> GetBannedExtended(IEnumerable<string> fields, string nameCase = null)
        {
            if (fields == null)
            {
                throw new ArgumentNullException("fields");
            }
            VkAPI.Manager.Method("newsfeed.getBanned");
            VkAPI.Manager.Params("extended", 1);
            VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + "," + b));
            if (nameCase != null)
            {
                VkAPI.Manager.Params("name_case", nameCase);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new BannedInfoExtended(resp) : null;
        }

        /// <summary>
        ///     Запрещает показывать новости от заданных пользователей и групп в ленте новостей текущего пользователя.
        /// </summary>
        /// <param name="uids">Перечисленные через запятую идентификаторы друзей пользователя, новости от которых необходимо скрыть из ленты новостей текущего пользователя.</param>
        /// <param name="gids">Перечисленные через запятую идентификаторы групп пользователя, новости от которых необходимо скрыть из ленты новостей текущего пользователя.</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> AddBan(IEnumerable<int> uids = null, IEnumerable<int> gids = null)
        {
            VkAPI.Manager.Method("newsfeed.addBan");
            if (uids != null)
                VkAPI.Manager.Params("user_ids", uids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            if (gids != null)
                VkAPI.Manager.Params("group_ids", gids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Разрешает показывать новости от заданных пользователей и групп в ленте новостей текущего пользователя.
        /// </summary>
        /// <param name="uids">Перечисленные через запятую идентификаторы друзей пользователя, новости от которых необходимо вернуть в ленту новостей текущего пользователя.</param>
        /// <param name="gids">Перечисленные через запятую идентификаторы групп пользователя, новости от которых необходимо вернуть в ленту новостей текущего пользователя.</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> DeleteBan(IEnumerable<int> uids = null, IEnumerable<int> gids = null)
        {
            VkAPI.Manager.Method("newsfeed.deleteBan");
            if (uids != null)
                VkAPI.Manager.Params("user_ids", uids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            if (gids != null)
                VkAPI.Manager.Params("group_ids", gids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает данные, необходимые для показа раздела комментариев в новостях пользователя.
        /// </summary>
        /// <param name="filter">Перечисленные через запятую типы объектов, изменения комментариев к которым нужно вернуть.</param>
        /// <param name="unixTimeStart">Время, в формате unixtime, начиная с которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным значению времени, которое было сутки назад.</param>
        /// <param name="unixTimeEnd">Время, в формате unixtime, до которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным текущему времени.</param>
        /// <param name="count">Указывает, какое максимальное число новостей следует возвращать, но не более 100. По умолчанию 30.</param>
        /// <param name="lastCommentsCount">Количество комментариев к записям, которые нужно получить. По умолчанию 0, максимальное значение 10</param>
        /// <param name="reposts">Идентификатор объекта, комментарии к репостам которого необходимо вернуть, например wall1_45486. Если указан данный параметр, параметр filters указывать не обзательно.</param>
        /// <param name="startFrom">Идентификатор, необходимый для получения следующей страницы результатов. Значение, необходимое для передачи в этом параметре, возвращается в поле ответа next_from. </param>
        public static async Task<NewsClassified> GetComments(NewsType filter = null, int? unixTimeStart = null, string startFrom = null,
                                                 int? unixTimeEnd = null, int? count = null, ushort? lastCommentsCount = null,
                                                 string reposts = null)
        {
            VkAPI.Manager.Method("newsfeed.getComments");
            if (startFrom != null)
            {
                VkAPI.Manager.Params("start_from", startFrom);
            }
            if (filter != null)
            {
                VkAPI.Manager.Params("filters", filter.Value);
            }
            if (unixTimeStart != null)
            {
                VkAPI.Manager.Params("start_time", unixTimeStart.Value);
            }
            if (unixTimeEnd != null)
            {
                VkAPI.Manager.Params("end_time", unixTimeEnd.Value);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (lastCommentsCount != null)
            {
                VkAPI.Manager.Params("last_comments_count", lastCommentsCount);
            }
            if (reposts != null)
            {
                VkAPI.Manager.Params("reposts", reposts);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes ? null : new NewsClassified(resp);
            }
            return null;
        }

        /// <summary>
        ///     Возвращает результаты поиска по статусам.
        /// </summary>
        /// <param name="q">Поисковой запрос, по которому необходимо получить результаты.</param>
        /// <param name="count">Указывает, какое максимальное число записей следует возвращать, но не более 100.</param>
        /// <param name="startFrom">Идентификатор, необходимый для получения следующей страницы результатов. Значение, необходимое для передачи в этом параметре, возвращается в поле ответа next_from. </param>
        /// <param name="startTime">Время, в формате unixtime, начиная с которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным значению времени, которое было сутки назад.</param>
        /// <param name="endTime">Время, в формате unixtime, до которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным текущему времени.</param>
        /// <param name="startId">Строковый id последней полученной записи. (Возвращается в результатах запроса, для того, чтобы исключить из выборки нового запроса уже полученные записи)</param
        /// <param name="lng">Географическая долгота точки, в радиусе от которой необходимо производить поиск, заданная в градусах (от -180 до 180)</param>
        /// <param name="lat">Географическая широта точки, в радиусе от которой необходимо производить поиск, заданная в градусах (от -90 до 90)</param>
        public static async Task<ListCount<BaseNewsEntity>> Search(string q = null, int? count = null, string startFrom = null,
                                                  int? startTime = null, int? endTime = null, int? startId = null,
                                                  double? lat = null, double? lng = null)
        {
            VkAPI.Manager.Method("newsfeed.search");
            if (q != null)
            {
                VkAPI.Manager.Params("q", q);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (startFrom != null)
            {
                VkAPI.Manager.Params("start_from", startFrom);
            }
            if (startTime != null)
            {
                VkAPI.Manager.Params("start_time", startTime);
            }
            if (endTime != null)
            {
                VkAPI.Manager.Params("end_time", endTime);
            }
            if (startId != null)
            {
                VkAPI.Manager.Params("start_id", startId);
            }
            if (lat != null)
            {
                VkAPI.Manager.Params("latitude", lat);
            }
            if (lng != null)
            {
                VkAPI.Manager.Params("longtitude", lng);
            }

            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var items = resp.SelectNodes("items/*");
                if (items != null)
                {
                    return
                        new ListCount<BaseNewsEntity>(resp.Int("count").Value, items.Cast<XmlNode>()
                             .Select(y => new BaseNewsEntity(y))
                             .ToList());
                }
            }
            return null;
        }

        /// <summary>
        ///     Возвращает результаты поиска по статусам + информацию о пользователе или группе, разместившей запись.
        /// </summary>
        /// <param name="q">Поисковой запрос, по которому необходимо получить результаты.</param>
        /// <param name="count">Указывает, какое максимальное число записей следует возвращать, но не более 100.</param>
        /// <param name="startFrom">Идентификатор, необходимый для получения следующей страницы результатов. Значение, необходимое для передачи в этом параметре, возвращается в поле ответа next_from. </param>
        /// <param name="startTime">Время, в формате unixtime, начиная с которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным значению времени, которое было сутки назад.</param>
        /// <param name="endTime">Время, в формате unixtime, до которого следует получить новости для текущего пользователя. Если параметр не задан, то он считается равным текущему времени.</param>
        /// <param name="startId">Строковый id последней полученной записи. (Возвращается в результатах запроса, для того, чтобы исключить из выборки нового запроса уже полученные записи)</param>
        /// <param name="lng">Географическая долгота точки, в радиусе от которой необходимо производить поиск, заданная в градусах (от -180 до 180)</param>
        /// <param name="lat">Географическая широта точки, в радиусе от которой необходимо производить поиск, заданная в градусах (от -90 до 90)</param>
        public static async Task<List<SearchExtendedResult>> SearchExtended(string q = null, int? count = null, string startFrom = null,
                                                  int? startTime = null, int? endTime = null, int? startId = null,
                                                  double? lat = null, double? lng = null)
        {
            VkAPI.Manager.Method("newsfeed.search");
            VkAPI.Manager.Params("extended", 1);
            if (q != null)
            {
                VkAPI.Manager.Params("q", q);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (startFrom != null)
            {
                VkAPI.Manager.Params("start_from", startFrom);
            }
            if (startTime != null)
            {
                VkAPI.Manager.Params("start_time", startTime);
            }
            if (endTime != null)
            {
                VkAPI.Manager.Params("end_time", endTime);
            }
            if (startId != null)
            {
                VkAPI.Manager.Params("start_id", startId);
            }
            if (lat != null)
            {
                VkAPI.Manager.Params("latitude", lat);
            }
            if (lng != null)
            {
                VkAPI.Manager.Params("longtitude", lng);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var items = resp.SelectNodes("items/*");
                if (items != null)
                {
                    return
                        items.Cast<XmlNode>()
                             .Select(y => new SearchExtendedResult(y))
                             .ToList();
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает пользовательские списки новостей
        /// </summary>
        public static async Task<ListCount<IdTitleObject>> GetLists()
        {
            VkAPI.Manager.Method("newsfeed.getLists");

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<IdTitleObject>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Отписывает текущего пользователя от комментариев к заданному объекту
        /// </summary>
        /// <param name="itemId">Идентификатор объекта</param>
        /// <param name="type">Тип объекта, от комментариев к которому необходимо отписаться.</param>
        /// <param name="ownerId">Идентификатор владельца объекта</param>
        /// <returns>После успешного выполнения возвращает true</returns>
        public static async Task<bool> Unsubscribe(int itemId, CommentType type, int? ownerId = null)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            VkAPI.Manager.Method("newsfeed.unsubscribe");
            VkAPI.Manager.Params("item_id", itemId);
            VkAPI.Manager.Params("type", type.StringValue);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Возвращает сообщества, на которые пользователю рекомендуется подписаться
        /// </summary>
        /// <param name="count">Количество сообществ или пользователей, которое необходимо вернуть</param>
        /// <param name="offset">Отсуп, необходимый для выборки определенного подмножества сообществ или пользователей</param>
        /// <param name="shuffle">Перемешивать ли возвращаемый список</param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть.</param>
        public static async Task<UsersGroupsList> GetSuggestedSources(int? count = null, int? offset = null, bool? shuffle = null, IEnumerable<string> fields = null)
        {
            VkAPI.Manager.Method("newsfeed.getSuggestedSources");

            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (shuffle != null)
            {
                VkAPI.Manager.Params("shuffle", shuffle.Value ? 1 : 0);
            }
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a,b) => a + "," + b));
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                    return null;
                var r = new UsersGroupsList();
                r.Groups = resp.SelectNodes("//group").Cast<XmlNode>().Select(y => new Group(y)).ToList();
                r.Users = resp.SelectNodes("//user").Cast<XmlNode>().Select(y => new BaseUser(y)).ToList();
                return r;
            }
            return null;
        }
    }

    /// <summary>
    /// Результат выполнения метода News.SearchExtended
    /// </summary>
    public class SearchExtendedResult : NewsClassified
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? TotalCount { get; set; }

        public SearchExtendedResult(XmlNode node) : base(node)
        {
            Count = node.Int("count");
            TotalCount = node.Int("total_count");
        }
    }
}