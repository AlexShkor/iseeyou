#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Photos;
using VkAPIAsync.Wrappers.Wall;

#endregion

namespace VkAPIAsync.Wrappers.Videos
{
    /// <summary>
    /// Видеозаписи
    /// </summary>
    public static class Videos
    {
        /// <summary>
        ///     Возвращает информацию о видеозаписях.
        /// </summary>
        /// <param name="videos">Перечисленные через запятую идентификаторы – идущие через знак подчеркивания id пользователей, которым принадлежат видеозаписи, и id самих видеозаписей. Если видеозапись принадлежит группе, то в качестве первого параметра используется -id группы.</param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежат видеозаписи</param>
        /// <param name="extended">Определяет, возвращать ли информацию о настройках приватности видео для текущего пользователя. </param>
        /// <param name="aid">ID альбома видеозаписи из которого нужно вернуть.</param>
        /// <param name="width">Требуемая ширина изображений видеозаписей в пикселах. Возможные значения - 130, 160 (по умолчанию), 320.</param>
        /// <param name="count">Количество возвращаемых видеозаписей (максимум 200).</param>
        /// <param name="offset">Смещение относительно первой найденной видеозаписи для выборки определенного подмножества.</param>
        public static async Task<ListCount<Video>> Get(IEnumerable<string> videos = null, int? ownerId = null, bool? extended = null,
                                           int? aid = null, int? width = null, int? count = null, int? offset = null)
        {
            if (count > 200) throw new ArgumentException("Параметр 'count' должен быть < 200");

            VkAPI.Manager.Method("video.get");
            if (videos != null)
            {
                VkAPI.Manager.Params("videos", videos.Aggregate((str, cur) => str + "," + cur));
            }
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (aid != null)
            {
                VkAPI.Manager.Params("album_id", aid);
            }
            if (width != null)
            {
                VkAPI.Manager.Params("width", width);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (extended != null)
            {
                VkAPI.Manager.Params("extended", extended.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }
                var nodes = resp.SelectNodes("items/*");
                return new ListCount<Video>(resp.Int("count").Value, nodes.Cast<XmlNode>().Select(node => new Video(node)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Редактирует данные видеозаписи на странице пользователя.
        /// </summary>
        /// <param name="vid">ID видеозаписи.</param>
        /// <param name="oid">ID владельца видеозаписи.</param>
        /// <param name="name">Название видеозаписи.</param>
        /// <param name="description">Описание видеозаписи.</param>
        /// <param name="privacyView">Приватность на просмотр видео в соответствии с форматом приватности.</param>
        /// <param name="privacyComment">Приватность на просмотр видео в соответствии с форматом приватности.</param>
        /// <param name="repeat">Зацикливание воспроизведения видеозаписи</param>
        /// <returns>При успешном редактировании видеозаписи вернет "true". </returns>
        public static async Task<bool> Edit(int vid, int oid, string name = null, string description = null, AccessPrivacy privacyView = null,
                                AccessPrivacy privacyComment = null, bool? repeat = null)
        {
            VkAPI.Manager.Method("video.edit");
            VkAPI.Manager.Params("video_id", vid);
            VkAPI.Manager.Params("owner_id", oid);
            if (name != null)
            {
                VkAPI.Manager.Params("name", name);
            }
            if (description != null)
            {
                VkAPI.Manager.Params("desc", description);
            }
            if (privacyComment != null)
            {
                VkAPI.Manager.Params("privacy_comment", privacyComment);
            }
            if (privacyView != null)
            {
                VkAPI.Manager.Params("privacy_view", privacyView);
            }
            if (repeat != null)
            {
                VkAPI.Manager.Params("repeat", repeat.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Копирует видеозапись на страницу пользователя.
        /// </summary>
        /// <param name="videoId">ID видеозаписи.</param>
        /// <param name="ownerId">ID владельца видеозаписи.</param>
        /// <returns>При успешном копировании видеозаписи сервер вернет идентификатор созданной видеозаписи (vid). </returns>
        public static async Task<int> Add(int videoId, int ownerId)
        {
            VkAPI.Manager.Method("video.add");
            VkAPI.Manager.Params("video_id", videoId);
            VkAPI.Manager.Params("owner_id", ownerId);
            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Удаляет видеозапись со страницы пользователя.
        /// </summary>
        /// <param name="videoId">ID видеозаписи.</param>
        /// <param name="ownerId">ID владельца видеозаписи.</param>
        /// <returns>При успешном удалении видеозаписи сервер вернет true. </returns>
        public static async Task<bool> Delete(int videoId, int ownerId)
        {
            VkAPI.Manager.Method("video.delete");
            VkAPI.Manager.Params("video_id", videoId);
            VkAPI.Manager.Params("owner_id", ownerId);
            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Восстанавливает удаленную видеозапись
        /// </summary>
        /// <param name="videoId">ID видеозаписи.</param>
        /// <param name="ownerId">ID владельца видеозаписи.</param>
        /// <returns>При успешном восстановлении видеозаписи сервер вернет true. </returns>
        public static async Task<bool> Restore(int videoId, int ownerId)
        {
            VkAPI.Manager.Method("video.restore");
            VkAPI.Manager.Params("video_id", videoId);
            VkAPI.Manager.Params("owner_id", ownerId);
            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает адрес сервера (необходимый для загрузки) и данные видеозаписи.
        /// </summary>
        /// <param name="name">Название видеофайла.</param>
        /// <param name="description">Описание видеофайла.</param>
        /// <param name="gid">Группа, в которую будет сохранён видеофайл. По умолчанию видеофайл сохраняется на страницу пользователя.</param>
        /// <param name="isPrivate">Указывается 1 в случае последующей отправки видеозаписи личным сообщением. После загрузки с этим параметром видеозапись не будет отображаться в списке видеозаписей пользователя и не будет доступна другим пользователям по id. По умолчанию 0.</param>
        /// <param name="privacyView">Приватность на просмотр видео в соответствии с форматом приватности.</param>
        /// <param name="privacyComment">Приватность на комментирование видео в соответствии с форматом приватности.</param>
        /// <param name="link">url для встраивания видео с внешнего сайта, например, с youtube. В этом случае нужно вызвать полученный upload_url не прикрепляя файл, достаточно просто обратиться по этому адресу. </paramparam>
        /// <param name="repeat">Зацикливание воспроизведения видеозаписи</param>
        /// <param name="wallPost">Требуется ли после сохранения опубликовать запись с видео на стене</param>
        public static async Task<VideoSaveResult> Save(string name = null, string description = null, int? gid = null,  string link = null,
                                           bool? isPrivate = null, bool? wallPost = null, AccessPrivacy privacyView = null,
                                           AccessPrivacy privacyComment = null, bool? repeat = null)
        {
            VkAPI.Manager.Method("video.save");

            if (name != null)
            {
                VkAPI.Manager.Params("name", name);
            }
            if (gid != null)
            {
                VkAPI.Manager.Params("group_id", gid);
            }
            if (description != null)
            {
                VkAPI.Manager.Params("description", description);
            }
            if (isPrivate != null)
            {
                VkAPI.Manager.Params("isPrivate", isPrivate.Value ? 1 : 0);
            }
            if (wallPost != null)
            {
                VkAPI.Manager.Params("wallpost", wallPost.Value ? 1 : 0);
            }
            if (privacyView != null)
            {
                VkAPI.Manager.Params("privacy_view", privacyView);
            }
            if (privacyComment != null)
            {
                VkAPI.Manager.Params("privacy_comment", privacyComment);
            }
            if (link != null)
            {
                VkAPI.Manager.Params("link", link);
            }
            if (repeat != null)
            {
                VkAPI.Manager.Params("repeat", repeat.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }
                var video = new Video(resp.SelectSingleNode("video"));
                var uploadPath = resp.SelectSingleNode("video/upload_url").InnerText;
                return new VideoSaveResult {Video = video, UploadPath = uploadPath};
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список альбомов видеозаписей пользователя или группы.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца альбомов (пользователь или сообщество). По умолчанию — идентификатор текущего пользователя. </param>
        /// <param name="count">Количество альбомов, которое необходимо вернуть. (по умолчанию – не больше 50, максимум - 100).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества альбомов.</param>
        /// <param name="extended">true – позволяет получать поля count, photo_320 и photo_160 для каждого альбома. Если альбом пустой то поля photo_320 и photo_160 возвращены не будут. По умолчанию – 0. </param>
        public static async Task<ListCount<VideoAlbum>> GetAlbums(int? ownerId = null, int? count = null, int? offset = null, bool? extended = null)
        {
            VkAPI.Manager.Method("video.getAlbums");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (extended != null)
            {
                VkAPI.Manager.Params("extended", extended.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            var nodes = result.SelectNodes("items/*");
            return VkAPI.Manager.MethodSuccessed && nodes != null && nodes.Count > 0
                       ? new ListCount<VideoAlbum>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new VideoAlbum(x)).ToList())
                       : null;
        }

        /// <summary>
        ///     Создает пустой альбом видеозаписей.
        /// </summary>
        /// <param name="title">Название альбома.</param>
        /// <param name="gid">ID  группы, которой принадлежат видеозаписи. Если параметр не указан, то альбом создается у текущего пользователя.</param>
        /// <returns>Возвращает album_id - id созданного альбома. </returns>
        public static async Task<int> AddAlbum(string title, int? gid)
        {
            VkAPI.Manager.Method("video.addAlbum");
            VkAPI.Manager.Params("title", title);
            if (gid != null) 
                VkAPI.Manager.Params("group_id", gid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Редактирует название альбома видеозаписей.
        /// </summary>
        /// <param name="title">Новое название альбома.</param>
        /// <param name="albumId">ID редактируемого альбома.</param>
        /// <param name="gid">ID группы, которой принадлежат видеозаписи. Если параметр не указан, то изменяется альбом текущего пользователя.</param>
        /// <returns>Возвращает true в случае успеха. </returns>
        public static async Task<bool> EditAlbum(string title, int albumId, int? gid)
        {
            VkAPI.Manager.Method("video.editAlbum");
            VkAPI.Manager.Params("album_id", albumId);
            VkAPI.Manager.Params("title", title);
            if (gid != null) VkAPI.Manager.Params("group_id", gid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return result.BoolVal().Value;
        }

        /// <summary>
        ///     Удаляет альбом видеозаписей.
        /// </summary>
        /// <param name="albumId">ID удаляемого альбома.</param>
        /// <param name="gid">ID группы, которой принадлежат видеозаписи. Если параметр не указан, то альбом удаляется у текущего пользователя.</param>
        /// <returns>Возвращает true в случае успеха. </returns>
        public static async Task<bool> DeleteAlbum(int albumId, int? gid)
        {
            VkAPI.Manager.Method("video.deleteAlbum");
            VkAPI.Manager.Params("album_id", albumId);
            if (gid != null) VkAPI.Manager.Params("group_id", gid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return result.BoolVal().Value;
        }

        /// <summary>
        ///     Перемещает видеозаписи в альбом.
        /// </summary>
        /// <param name="vids">ID видеозаписей, перечисленные через запятую.</param>
        /// <param name="albumId">ID альбома, в который перемещаются видеозаписи.</param>
        /// <param name="groupId">ID группы, которой принадлежат видеозаписи. Если параметр не указан, то работа ведется с альбомом текущего пользователя.</param>
        /// <returns>Возвращает true в случае успеха. </returns>
        public static async Task<bool> MoveToAlbum(IEnumerable<string> vids, int albumId, int? groupId)
        {
            VkAPI.Manager.Method("video.moveToAlbum");
            VkAPI.Manager.Params("video_ids", vids.Aggregate((a, b) => a + "," + b));
            VkAPI.Manager.Params("album_id", albumId);
            if (groupId != null)
                VkAPI.Manager.Params("group_id", groupId);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает список видеозаписей на которых есть непросмотренные отметки.
        /// </summary>
        /// <param name="offset">Смещение необходимой для получения определённого подмножества видеозаписей.</param>
        /// <param name="count">Количество видеозаписей которые необходимо вернуть.</param>
        public static async Task<ListCount<TaggedVideo>> GetNewTags(int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("video.getNewTags");
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            var nodes = result.SelectNodes("items/*");
            return VkAPI.Manager.MethodSuccessed && nodes != null && nodes.Count > 0
                       ? new ListCount<TaggedVideo>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new TaggedVideo(x)).ToList())
                       : null;
        }

        /// <summary>
        ///     Возвращает список видеозаписей в соответствии с заданным критерием поиска.
        /// </summary>
        /// <param name="query">Строка поискового запроса. Например, The Beatles.</param>
        /// <param name="order">Вид сортировки.</param>
        /// <param name="hdOnly">Если не равен нулю, то поиск производится только по видеозаписям высокого качества.</param>
        /// <param name="count">Количество возвращаемых видеозаписей (максимум 200).</param>
        /// <param name="offset">Смещение относительно первой найденной видеозаписи для выборки определенного подмножества.</param>
        /// <param name="adult">Отключение опции "безопасный поиск".</param>
        /// <param name="filters">Список критериев, по которым требуется отфильтровать видео.</param>
        /// <param name="searchOwn">true – искать по видеозаписям пользователя, false – не искать по видеозаписям пользователя (по умолчанию).</param>
        public static async Task<ListCount<Video>> Search(string query, VideoSortOrder order = null,
                                              bool? hdOnly = null, VideoFilters filters = null, bool? searchOwn = null, int? count = null,
                                              int? offset = null, bool? adult = null)
        {
            VkAPI.Manager.Method("video.search");
            VkAPI.Manager.Params("q", query);
            if (order != null) VkAPI.Manager.Params("sort", order);
            if (hdOnly != null) VkAPI.Manager.Params("hd", hdOnly.Value ? 1 : 0);
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (adult != null)
            {
                VkAPI.Manager.Params("adult", adult);
            }
            if (searchOwn != null)
            {
                VkAPI.Manager.Params("search_own", searchOwn);
            }
            if (filters != null)
            {
                VkAPI.Manager.Params("filters", filters.ToString());
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            var nodes = result.SelectNodes("items/*");
            return VkAPI.Manager.MethodSuccessed && nodes != null && nodes.Count > 0
                       ? new ListCount<Video>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Video(x)).ToList())
                       : null;
        }

        /// <summary>
        ///     Возвращает список видеозаписей, на которых отмечен пользователь.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <param name="count">Количество видеозаписей, которое необходимо получить (но не более 100).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества видеозаписей.</param>
        public static async Task<ListCount<Video>> GetUserVideos(int? uid, int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("video.getUserVideos");
            if (uid != null)
            {
                VkAPI.Manager.Params("user_id", uid);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            var nodes = result.SelectNodes("items/*");
            return VkAPI.Manager.MethodSuccessed && nodes != null && nodes.Count > 0
                       ? new ListCount<Video>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Video(x)).ToList())
                       : null;
        }

        /// <summary>
        ///     Возвращает список комментариев к видеозаписи.
        /// </summary>
        /// <param name="vid">Идентификатор видеозаписи.</param>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <param name="needLikes">true — будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается. </param>
        /// <param name="count">Количество комментариев, которое необходимо получить (но не более 100).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества комментариев.</param>
        /// <param name="order">Порядок сортировки комментариев (asc - от старых к новым, desc - от новых к старым)</param>
        public static async Task<ListCount<Comment>> GetComments(int vid, int? ownerId = null, bool? needLikes = null, int? count = null, int? offset = null,
                                                     SortOrder order = null)
        {
            VkAPI.Manager.Method("video.getComments");
            VkAPI.Manager.Params("video_id", vid);
            if (needLikes != null)
            {
                VkAPI.Manager.Params("need_likes", needLikes.Value ? 1 : 0);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (order != null)
            {
                VkAPI.Manager.Params("sort", order.StringValue);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            var nodes = result.SelectNodes("comment");
            return VkAPI.Manager.MethodSuccessed && nodes != null && nodes.Count > 0
                       ? new ListCount<Comment>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Comment(x)).ToList())
                       : null;
        }

        /// <summary>
        ///     Cоздает новый комментарий к видеозаписи.
        /// </summary>
        /// <param name="vid">Идентификатор видеозаписи.</param>
        /// <param name="attachments">Список объектов, приложенных к комментарию</param>
        /// <param name="fromGroup">Данный параметр учитывается, если oid меньше 0 (комментарий к видеозаписи группы). true — комментарий будет опубликован от имени группы, false — комментарий будет опубликован от имени пользователя (по умолчанию). </param>
        /// <param name="replyToComment"></param>
        /// <param name="message">Текст комментария (минимальная длина - 2 символа).</param>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <returns>Возвращает идентификатор созданного комментария (cid) или код ошибки. </returns>
        public static async Task<int> CreateComment(int vid, string message = null, int? ownerId = null, IEnumerable<WallAttachment> attachments = null, bool? fromGroup = null, int? replyToComment = null)
        {
            if (message == null & attachments == null)
                throw new Exception("Должен быть указан хоть один параметр: message, attachments");

            VkAPI.Manager.Method("video.createComment");
            VkAPI.Manager.Params("video_id", vid);
            if(message != null)
            {
                VkAPI.Manager.Params("message", message);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Select(x => x.ToString()).Aggregate((a,b) => a + "," + b));
            }
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (fromGroup != null)
            {
                VkAPI.Manager.Params("from_group", fromGroup.Value ? 1 : 0);
            }
            if (replyToComment != null)
            {
                VkAPI.Manager.Params("reply_to_comment", replyToComment);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetXmlDocument();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Изменяет текст комментария к видеозаписи.
        /// </summary>
        /// <param name="cid">Идентификатор комментария.</param>
        /// <param name="message">Текст комментария (минимальная длина - 2 символа).</param>
        /// <param name="attachments">Новый список объектов, приложенных к комментарию</param>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <returns>Возвращает true в случае успешного изменения. </returns>
        public static async Task<bool> EditComment(int cid, string message = null, IEnumerable<WallAttachment> attachments = null, int? ownerId = null)
        {
            if (message == null & attachments == null)
                throw new Exception("Должен быть указан хоть один параметр: message, attachments");

            VkAPI.Manager.Method("video.editComment");
            VkAPI.Manager.Params("comment_id", cid);
            if (message != null)
            {
                VkAPI.Manager.Params("message", message);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetXmlDocument();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Удаляет комментарий к видеозаписи.
        /// </summary>
        /// <param name="cid">Идентификатор комментария.</param>
        /// <param name="ownerId">Идентификатор пользователя (по-умолчанию - текущий пользователь).</param>
        /// <returns>Возвращает true в случае успешного удаления. </returns>
        public static async Task<bool> DeleteComment(int cid, int? ownerId = null)
        {
            VkAPI.Manager.Method("video.deleteComment");
            VkAPI.Manager.Params("comment_id", cid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetXmlDocument();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Восстанавливает удаленный комментарий
        /// </summary>
        /// <param name="cid">Идентификатор комментария.</param>
        /// <param name="ownerId">Идентификатор пользователя (по-умолчанию - текущий пользователь).</param>
        /// <returns>Возвращает true в случае успешного удаления. </returns>
        public static async Task<bool> RestoreComment(int cid, int? ownerId = null)
        {
            VkAPI.Manager.Method("video.restoreComment");
            VkAPI.Manager.Params("comment_id", cid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetXmlDocument();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает список отметок на видеозаписи.
        /// </summary>
        /// <param name="vid">Идентификатор видеозаписи.</param>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        public static async Task<VideoTag> GetTags(int vid, int? ownerId)
        {
            VkAPI.Manager.Method("video.getTags");
            VkAPI.Manager.Params("video_id", vid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetXmlDocument();
            var tag = result.SelectSingleNode("tag");
            return VkAPI.Manager.MethodSuccessed && tag != null ? new VideoTag(tag) : null;
        }

        /// <summary>
        ///     Добавляет отметку на видеозапись.
        /// </summary>
        /// <param name="vid">Идентификатор видеозаписи.</param>
        /// <param name="userId">Идентификатор пользователя, которого нужно отметить на видеозаписи.</param>
        /// <param name="ownerId">Идентификатор владельца видеозаписи (по умолчанию - текущий пользователь).</param>
        /// <returns>Возвращает идентификатор созданной отметки (tag id) или код ошибки. </returns>
        public static async Task<int> PutTag(int vid, int uid, int? ownerId)
        {
            VkAPI.Manager.Method("video.putTag");
            VkAPI.Manager.Params("video_id", vid);
            VkAPI.Manager.Params("user_id", uid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetXmlDocument();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Удаляет отметку с видеозаписи.
        /// </summary>
        /// <param name="vid">Идентификатор видеозаписи.</param>
        /// <param name="tagId">Идентификатор отметки, которую нужно удалить.</param>
        /// <param name="ownerId">Идентификатор владельца видеозаписи (по умолчанию - текущий пользователь).</param>
        /// <returns>Возвращает true в случае успешного удаления или код ошибки. </returns>
        public static async Task<bool> RemoveTag(int vid, int tagId, int? ownerId)
        {
            VkAPI.Manager.Method("video.removeTag");
            VkAPI.Manager.Params("video_id", vid);
            VkAPI.Manager.Params("tag_id", tagId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetXmlDocument();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Позволяет пожаловаться на видеозапись.
        /// </summary>
        /// <param name="videoId">Идентификатор видеозаписи</param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит видеозапись</param>
        /// <param name="reason">Тип жалобы</param>
        /// <param name="comment">Комментарий для жалобы</param>
        /// <param name="searchQuery">Поисковой запрос, если видеозапись была найдена через поиск</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> ReportVideo(int videoId, int ownerId, ReportReason reason = null, string comment = null, string searchQuery = null)
        {
            VkAPI.Manager.Method("video.report");
            VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("video_id", videoId);
            if (reason != null)
            {
                VkAPI.Manager.Params("reason", reason.Value);
            }
            if (comment != null)
            {
                VkAPI.Manager.Params("comment", comment);
            }
            if (searchQuery != null)
            {
                VkAPI.Manager.Params("search_query", searchQuery);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        /// Позволяет пожаловаться на комментарий к видеозаписи.
        /// </summary>
        /// <param name="commentId">Идентификатор комментария. </param>
        /// <param name="ownerId">Идентификатор владельца видеозаписи к которой оставлен комментарий.</param>
        /// <param name="reason">Тип жалобы</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> ReportComment(int commentId, int ownerId, ReportReason reason = null)
        {
            VkAPI.Manager.Method("video.reportComment");
            VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("comment_id", commentId);
            if (reason != null)
            {
                VkAPI.Manager.Params("reason", reason.Value);
            }
            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        /// Результат сохранения видео
        /// </summary>
        public class VideoSaveResult
        {
            /// <summary>
            /// Сохраненное видео
            /// </summary>
            public Video Video;

            /// <summary>
            /// Путь для upload
            /// </summary>
            public string UploadPath;
        }
    }
}