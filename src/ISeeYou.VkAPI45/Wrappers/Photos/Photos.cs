#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.News;
using VkAPIAsync.Wrappers.Wall;

#endregion

namespace VkAPIAsync.Wrappers.Photos
{
    /// <summary>
    /// Фотографии
    /// </summary>
    public static class Photos
    {
        /// <summary>
        /// Возвращает список альбомов пользователя или сообщества.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежат альбомы.</param>
        /// <param name="albums">ID альбомов. </param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества альбомов. </param>
        /// <param name="count">Количество альбомов, которое нужно вернуть. (по умолчанию – все альбомы) </param>
        /// <param name="needCovers">true — будет возвращено дополнительное поле thumb_src. По умолчанию поле thumb_src не возвращается. </param>
        /// <param name="needSystem">true – будут возвращены системные альбомы, имеющие отрицательные идентификаторы. </param>
        /// <param name="photoSizes">true — будут возвращены размеры фотографий в специальном формате. </param>
        public static async Task<ListCount<PhotoAlbum>> GetAlbums(int? ownerId = null, int[] albums = null, int? offset = null, int? count = null,
                                                 bool? needCovers = null, bool? needSystem = null, bool? photoSizes = null)
        {
            VkAPI.Manager.Method("photos.getAlbums");
            if (ownerId != null)
                VkAPI.Manager.Params("owner_id", ownerId);
            if (albums != null)
                VkAPI.Manager.Params("album_ids", albums.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            if (needCovers != null)
                VkAPI.Manager.Params("need_covers", needCovers.Value ? 1 : 0);
            if (needSystem != null)
                VkAPI.Manager.Params("need_system", needSystem.Value ? 1 : 0);
            if (photoSizes != null)
                VkAPI.Manager.Params("photo_sizes", photoSizes.Value ? 1 : 0);
            if (offset != null)
                VkAPI.Manager.Params("offset", offset);
            if (count != null)
                VkAPI.Manager.Params("count", count);

            XmlNode result = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<PhotoAlbum>(result.Int("count").Value, result.SelectNodes("items/*").Cast<XmlNode>().Select(x => new PhotoAlbum(x)).ToList())
                       : null;
        }

        /// <summary>
        ///     Возвращает количество доступных альбомов пользователя.
        /// </summary>
        /// <param name="userId">ID пользователя, которому принадлежат альбомы. По умолчанию – ID текущего пользователя.</param>
        /// <param name="gid">ID группы, которой принадлежат альбомы.</param>
        public static async Task<int> GetAlbumsCount(int? uid, int? gid)
        {
            VkAPI.Manager.Method("photos.getAlbumsCount");
            if (uid != null) VkAPI.Manager.Params("user_id", uid);
            if (gid != null) VkAPI.Manager.Params("group_id", gid);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        /// Возвращает список фотографий в альбоме.
        /// </summary>
        /// <param name="albumId">Идентификатор альбома</param>
        /// <param name="ownerId">Идентификатор владельца альбома</param>
        /// <param name="photoIds">Идентификаторы фотографий, информацию о которых необходимо вернуть</param>
        /// <param name="antiChronologic">Порядок сортировки фотографий (true — антихронологический, false — хронологический). </param>
        /// <param name="extended">true — будут возвращены дополнительные поля likes, comments, tags, can_comment. Поля comments и tags содержат только количество объектов. По умолчанию данные поля не возвращается. </param>
        /// <param name="feed">Unixtime, который может быть получен методом newsfeed.get в поле date, для получения всех фотографий загруженных пользователем в определённый день либо на которых пользователь был отмечен. Также нужно указать параметр userId пользователя, с которым произошло событие. </param>
        /// <param name="feedType">Тип новости получаемый в поле type метода newsfeed.get, для получения только загруженных пользователем фотографий, либо только фотографий, на которых он был отмечен. Может принимать значения photo, photo_tag. </param>
        /// <param name="photoSizes">Возвращать ли доступные размеры фотографии</param>
        public static async Task<ListCount<Photo>> GetPhotos(int albumId, int? ownerId = null, int[] photoIds = null, bool? antiChronologic = null,
                                                     bool? extended = null, int? feed = null,
                                                     NewsType feedType = null, bool? photoSizes = null)
        {
            VkAPI.Manager.Method("photos.get");

            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("album_id", albumId);
            if (photoIds != null)
            {
                VkAPI.Manager.Params("photo_ids", photoIds.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (antiChronologic != null)
            {
                VkAPI.Manager.Params("rev", antiChronologic.Value ? 1 : 0);
            }
            if (photoSizes != null)
            {
                VkAPI.Manager.Params("photo_sizes", photoSizes.Value ? 1 : 0);
            }
            if (extended != null)
                VkAPI.Manager.Params("extended", extended);
            if (feed != null) VkAPI.Manager.Params("feed", feed);
            if (feedType != null) VkAPI.Manager.Params("feedType", feedType.Value);

            XmlNode result = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<Photo>(result.Int("count").Value, result.SelectNodes("items/*")
                               .Cast<XmlNode>()
                               .Select(x => new Photo(x))
                               .ToList())
                       : null;
        }

        /// <summary>
        /// Осуществляет поиск изображений по местоположению или описанию.
        /// </summary>
        /// <param name="q">Строка поискового запроса, например, "Nature". </param>
        /// <param name="lat">Географическая широта отметки, заданная в градусах (от -90 до 90). </param>
        /// <param name="longt">Географическая долгота отметки, заданная в градусах (от -180 до 180). </param>
        /// <param name="startTime">Время в формате unixtime, не раньше которого должны были быть загружены найденные фотографии. </param>
        /// <param name="endTime">Время в формате unixtime, не позже которого должны были быть загружены найденные фотографии. </param>
        /// <param name="likesSort">true – сортировать по количеству отметок «Мне нравится», false – сортировать по дате добавления фотографии. </param>
        /// <param name="count">Количество возвращаемых фотографий. </param>
        /// <param name="offset">Смещение относительно первой найденной фотографии для выборки определенного подмножества. </param>
        /// <param name="radius">Радиус поиска в метрах. (работает очень приближенно, поэтому реальное расстояние до цели может отличаться от заданного). Может принимать значения: 10, 100, 800, 6000, 50000 </param>
        public static async Task<ListCount<Photo>> Search(string q = null, double? lat = null, double? longt = null, int? startTime = null,
                                             int? endTime = null, bool? likesSort = null, int? count = null, int? offset = null, int? radius = null)
        {
            VkAPI.Manager.Method("photos.search");
            if (q != null)
            {
                VkAPI.Manager.Params("q", q);
            }
            if (lat != null)
            {
                VkAPI.Manager.Params("lat", lat);
            }
            if (longt != null)
            {
                VkAPI.Manager.Params("long", longt);
            }
            if (startTime != null)
            {
                VkAPI.Manager.Params("start_time", startTime);
            }
            if (endTime != null)
            {
                VkAPI.Manager.Params("end_time", endTime);
            }
            if (likesSort != null)
            {
                VkAPI.Manager.Params("sort", likesSort.Value ? 1 : 0);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            XmlNode result = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<Photo>(result.Int("count").Value, result.SelectNodes("items/*")
                               .Cast<XmlNode>()
                               .Select(x => new Photo(x))
                               .ToList())
                       : null;
        }

        /// <summary>
        /// Возвращает список фотографий со страницы пользователя или сообщества.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, фотографии которого нужно получить.</param>
        /// <param name="albumId">Идентификатор альбома. </param>
        /// <param name="photoIds">Идентификаторы фотографий, информацию о которых необходимо вернуть. </param>
        /// <param name="antiChronologic">Порядок сортировки фотографий (true — антихронологический, false — хронологический).</param>
        /// <param name="extended">true — будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается. </param>
        /// <param name="feedType">Тип новости получаемый в поле type метода newsfeed.get, для получения только загруженных пользователем фотографий, либо только фотографий, на которых он был отмечен. Может принимать значения photo, photo_tag. </param>
        /// <param name="photoSizes">Возвращать ли доступные размеры фотографии</param>
        public static async Task<ListCount<Photo>> GetProfile(int? ownerId = null, int? albumId = null, int[] photoIds = null, bool? antiChronologic = null, bool? extended = null, NewsType feedType = null, bool? photoSizes = null)
        {
            VkAPI.Manager.Method("photos.getProfile");
            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            if (albumId != null) VkAPI.Manager.Params("album_id", albumId);
            if (photoIds != null)
            {
                VkAPI.Manager.Params("photo_ids", photoIds.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (antiChronologic != null)
            {
                VkAPI.Manager.Params("rev", antiChronologic.Value ? 1 : 0);
            }
            if (photoSizes != null)
            {
                VkAPI.Manager.Params("photo_sizes", photoSizes.Value ? 1 : 0);
            }
            if (extended != null)
                VkAPI.Manager.Params("extended", extended);
            if (feedType != null) VkAPI.Manager.Params("feedType", feedType.Value);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<Photo>(result.Int("count").Value, result.SelectNodes("items/*")
                               .Cast<XmlNode>()
                               .Select(x => new Photo(x))
                               .ToList())
                       : null;
        }

        /// <summary>
        ///     Возвращает все фотографии пользователя или группы в антихронологическом порядке.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя или сообщества, фотографии которого нужно получить.</param>
        /// <param name="count">Количество фотографий, которое необходимо получить (но не более 100).</param>
        /// <param name="photoSizes">true — будут возвращены размеры фотографий в специальном формате.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества фотографий.</param>
        /// <param name="extended">true - будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается.</param>
        /// <param name="noServiceAlbums">false - вернуть все фотографии, включая находящиеся в сервисных альбомах, таких как "Фотографии на моей стене". (по умолчанию); true - вернуть фотографии только из стандартных альбомов пользователя. </param>
        public static async Task<ListCount<Photo>> GetAll(int? userId = null, int? count = null, int? offset = null,
                                                   bool? extended = null, bool? noServiceAlbums = null, bool? photoSizes = null)
        {
            VkAPI.Manager.Method("photos.getAll");
            if (userId != null)
            {
                VkAPI.Manager.Params("owner_id", userId);
            }
            if (extended != null) VkAPI.Manager.Params("extended", extended.Value ? 1 : 0);
            if (noServiceAlbums != null) VkAPI.Manager.Params("no_service_albums", noServiceAlbums.Value ? 1 : 0);
            if (photoSizes != null) VkAPI.Manager.Params("photo_sizes", photoSizes.Value ? 1 : 0);
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<Photo>(result.Int("int").Value, result.SelectNodes("items/*")
                               .Cast<XmlNode>()
                               .Select(x => new Photo(x))
                               .ToList())
                       : null;
        }

        /// <summary>
        /// Возвращает список комментариев к фотографии.
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии. </param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит фотография.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества комментариев. По умолчанию — 0. </param>
        /// <param name="count">Количество комментариев, которое необходимо получить. </param>
        /// <param name="order">Порядок сортировки комментариев (asc — от старых к новым, desc - от новых к старым) </param>
        /// <param name="needLikes">true — будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается. </param>
        /// <param name="accessKey"></param>
        public static async Task<ListCount<Comment>> GetComments(int photoId, int? ownerId = null, int? offset = null, int? count = null, SortOrder order = null, bool? needLikes = null, string accessKey = null)
        {
            VkAPI.Manager.Method("photos.getComments");
            VkAPI.Manager.Params("photo_id", photoId);
            if(ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            if (offset != null) VkAPI.Manager.Params("offset", offset);
            if (count != null) VkAPI.Manager.Params("count", count);
            if (order != null) VkAPI.Manager.Params("sort", order.StringValue);
            if (needLikes != null) VkAPI.Manager.Params("need_likes", needLikes.Value ? 1 : 0);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<Comment>( result.Int("count").Value, result.SelectNodes("items/*").Cast<XmlNode>().Select(x => new Comment(x)).ToList())
                       : null;
        }

        /// <summary>
        /// Возвращает отсортированный в антихронологическом порядке список всех комментариев к конкретному альбому или ко всем альбомам пользователя.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежат фотографии.</param>
        /// <param name="aid">Идентификатор альбома. Если параметр не задан, то считается, что необходимо получить комментарии ко всем альбомам пользователя или сообщества. </param>
        /// <param name="count">Количество комментариев, которое необходимо получить. Если параметр не задан, то считается что он равен 20. Максимальное значение параметра 100.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества комментариев. Если параметр не задан, то считается, что он равен 0. </param>
        /// <param name="needLikes">true — будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается. </param>
        public static async Task<ListCount<PhotoComment>> GetAllComments(int? ownerId = null, int? aid = null, int? count = null, int? offset = null, bool? needLikes = null)
        {
            VkAPI.Manager.Method("photos.getAllComments");
            if (offset != null) VkAPI.Manager.Params("offset", offset);
            if (count != null) VkAPI.Manager.Params("count", count);
            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            if (aid != null) VkAPI.Manager.Params("album_id", aid);
            if (needLikes != null) VkAPI.Manager.Params("need_likes", needLikes.Value ? 1 : 0);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<PhotoComment>(result.Int("count").Value, result.SelectNodes("items/*").Cast<XmlNode>().Select(x => new PhotoComment(x)).ToList())
                       : null;
        }

        /// <summary>
        /// Создает новый комментарий к фотографии.
        /// </summary>
        /// <param name="pid">Идентификатор фотографии. </param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит фотография.</param>
        /// <param name="message">Текст комментария (является обязательным, если не задан параметр attachments). </param>
        /// <param name="replyToCid"></param>
        /// <param name="fromGroup">Данный параметр учитывается, если oid менее 0 (комментарий к фотографии группы). true — комментарий будет опубликован от имени группы, false — комментарий будет опубликован от имени пользователя (по умолчанию). </param>
        /// <param name="accessKey"></param>
        /// <param name="attachments">Cписок объектов, приложенных к комментарию</param>
        public static async Task<int> CreateComment(int pid, int? ownerId, string message, int? replyToCid = null, bool? fromGroup = null, string accessKey = null, IEnumerable<WallAttachment> attachments = null)
        {
            VkAPI.Manager.Method("photos.createComment");
            VkAPI.Manager.Params("photo_id", pid);
            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("message", message);
            if (replyToCid != null) VkAPI.Manager.Params("reply_to_comment", replyToCid);
            if (accessKey != null)
            {
                VkAPI.Manager.Params("access_key", accessKey);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Select( x=> x.ToString()).Aggregate((a,b)=> a + "," + b));
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        /// Изменяет текст комментария к фотографии. Обратите внимание, что редактирование комментария доступно только в течение суток после его создания.
        /// </summary>
        /// <param name="cid">Идентификатор комментария. </param>
        /// <param name="message">Новый текст комментария (является обязательным, если не задан параметр attachments). </param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит фотография.</param>
        /// <param name="attachments">Новый список объектов, приложенных к комментарию и разделённых символом ",".</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> EditComment(int cid, string message, int? ownerId = null, IEnumerable<WallAttachment> attachments = null)
        {
            VkAPI.Manager.Method("photos.editComment");
            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("message", message);
            VkAPI.Manager.Params("comment_id", cid);
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Удаляет комментарий к фотографии.
        /// </summary>
        /// <param name="cid">Идентификатор комментария.</param>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <returns>
        ///     После успешного выполнения возвращает true (false, если комментарий не найден). 
        /// </returns>
        public static async Task<bool> DeleteComment(int cid, int? ownerId = null)
        {
            VkAPI.Manager.Method("photos.deleteComment");
            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("comment_id", cid);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Восстанавливает удаленный комментарий к фотографии.
        /// </summary>
        /// <param name="cid">Идентификатор комментария.</param>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <returns>
        /// После успешного выполнения возвращает true (false, если комментарий с таким идентификатором не является удаленным).
        /// </returns>
        public static async Task<bool> RestoreComment(int cid, int? ownerId = null)
        {
            VkAPI.Manager.Method("photos.restoreComment");
            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("comment_id", cid);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает список фотографий, на которых отмечен пользователь.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества фотографий.</param>
        /// <param name="count">Количество фотографий, которое необходимо получить (но не более 100).</param>
        /// <param name="order">Сортировка результатов (0 - по дате добавления отметки в порядке убывания, 1 - по дате добавления отметки в порядке возрастания).</param>
        /// <param name="extended">true - будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается.</param>
        /// <returns>
        ///     Возвращает массив объектов - фотографий, каждый из которых содержит поля pid, aid, owner_id, created, text, src, src_big, src_small.
        ///     Первый элемент массива представляет собой общее количество фотографий.
        /// </returns>
        public static async Task<ListCount<Photo>> GetUserPhotos(int? uid = null, int? offset = null, int? count = null, SortOrder order = null,
                                                          bool? extended = null)
        {
            VkAPI.Manager.Method("photos.getUserPhotos");
            if (uid != null) VkAPI.Manager.Params("user_id", uid);
            if (offset != null) VkAPI.Manager.Params("offset", offset);
            if (count != null) VkAPI.Manager.Params("count", count);
            if (order != null) VkAPI.Manager.Params("sort", order.Value);
            if (extended != null) VkAPI.Manager.Params("extended", extended.Value ? 1 : 0);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<Photo>(result.Int("count").Value, result.SelectNodes("items/*")
                               .Cast<XmlNode>()
                               .Select(x => new Photo(x))
                               .ToList())
                       : null;
        }

        /// <summary>
        ///     Возвращает список отметок на фотографии.
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии.</param>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <returns></returns>
        public static async Task<List<PhotoTag>> GetTags(int photoId, int? ownerId = null)
        {
            VkAPI.Manager.Method("photos.getTags");
            VkAPI.Manager.Params("photo_id", photoId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? result.SelectNodes("tag").Cast<XmlNode>().Select(x => new PhotoTag(x)).ToList()
                       : null;
        }

        /// <summary>
        ///     Добавляет отметку на фотографию.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца фотографии (по умолчанию - текущий пользователь).</param>
        /// <param name="photoId">Идентификатор фотографии.</param>
        /// <param name="userId">Идентификатор пользователя, которого нужно отметить на фотографии.</param>
        /// <param name="x">Координата верхнего-левого угла отметки в % от ширины фотографии.</param>
        /// <param name="y">Координата верхнего-левого угла отметки в % от высоты фотографии.</param>
        /// <param name="x2">Координата правого-нижнего угла отметки в % от ширины фотографии.</param>
        /// <param name="y2">Координата правого-нижнего угла отметки в % от высоты фотографии.</param>
        /// <returns>Возвращает идентификатор созданной отметки (tag id) </returns>
        public static async Task<int> PutTag(int? ownerId, int photoId, int userId, float x, float y, float x2, float y2)
        {
            VkAPI.Manager.Method("photos.putTag");
            VkAPI.Manager.Params("photo_id", photoId);
            VkAPI.Manager.Params("user_id", userId);
            VkAPI.Manager.Params("x", x);
            VkAPI.Manager.Params("y", y);
            VkAPI.Manager.Params("x2", x2);
            VkAPI.Manager.Params("y2", y2);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        /// Подтверждает отметку на фотографии.
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии.</param>
        /// <param name="tagId">Идентификатор отметки на фотографии.</param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит фотография.</param>
        public static async Task<bool> ConfirmTag(int photoId, int tagId, int? ownerId = null)
        {
            VkAPI.Manager.Method("photos.confirmTag");
            VkAPI.Manager.Params("photo_id", photoId);
            VkAPI.Manager.Params("tag_id", tagId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Удаляет отметку с фотографии.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца фотографии (по умолчанию - текущий пользователь).</param>
        /// <param name="photoId">Идентификатор фотографии.</param>
        /// <param name="tagId">Идентификатор отметки, которую нужно удалить.</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> RemoveTag(int? ownerId, int photoId, int tagId)
        {
            VkAPI.Manager.Method("photos.removeTag");
            VkAPI.Manager.Params("photo_id", photoId);
            VkAPI.Manager.Params("tag_id", tagId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Удаляет указанный альбом для фотографий у текущего пользователя.
        /// </summary>
        /// <param name="aid">Идентификатор удаляемого альбома.</param>
        /// <param name="gid">Идентификатор группы в том случае, если альбом удаляется из группы.</param>
        public static async Task<bool> DeleteAlbum(int aid, int? gid = null)
        {
            VkAPI.Manager.Method("photos.deleteAlbum");
            VkAPI.Manager.Params("album_id", aid);
            if (gid != null)
            {
                VkAPI.Manager.Params("group_id", gid);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Удаление фотографии на сайте.
        /// </summary>
        /// <param name="oid">Идентификатор пользователя, которому принадлежит фотография. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя. Если передано отрицательное значение, будет удалена фотография группы с идентификатором -owner_id.</param>
        /// <param name="pid">ID фотографии, которую необходимо удалить.</param>
        /// <returns>Возвращает true в случае успешного удаления фотографии. </returns>
        public static async Task<bool> Delete(int oid, int pid)
        {
            VkAPI.Manager.Method("photos.delete");
            VkAPI.Manager.Params("owner_id", oid);
            VkAPI.Manager.Params("photo_id", pid);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает список фотографий на которых есть непросмотренные отметки.
        /// </summary>
        /// <param name="offset">Смещение необходимой для получения определённого подмножества фотографий.</param>
        /// <param name="count">Количество фотографий которые необходимо вернуть.</param>
        public static async Task<ListCount<PhotoTagInfo>> GetNewTags(int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("photos.getNewTags");
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<PhotoTagInfo>(result.Int("count").Value, result.SelectNodes("items/*").Cast<XmlNode>().Select(x => new PhotoTagInfo(x)).ToList())
                       : null;
        }

        /// <summary>
        /// Возвращает информацию о фотографиях по их идентификаторам.
        /// </summary>
        /// <param name="photos">перечисленные через запятую идентификаторы, которые представляют собой идущие через знак подчеркивания id пользователей, разместивших фотографии, и id самих фотографий. Чтобы получить информацию о фотографии в альбоме группы, вместо id пользователя следует указать -id группы. 
        /// Некоторые фотографии, идентификаторы которых могут быть получены через API, закрыты приватностью, и не будут получены. В этом случае следует использовать ключ доступа фотографии (access_key) в её идентификаторе.</param>
        /// <param name="extended">true — будут возвращены дополнительные поля likes, comments, tags, can_comment, can_repost. Поля comments и tags содержат только количество объектов. По умолчанию данные поля не возвращается. </param>
        /// <param name="photoSizes">Возвращать ли доступные размеры фотографии в специальном формате. </param>
        /// <returns>После успешного выполнения возвращает список объектов photo. </returns>
        public static async Task<List<Photo>> GetPhotosById(IEnumerable<string> photos, bool? extended = null, bool? photoSizes = null)
        {
            VkAPI.Manager.Method("photos.getById");
            if (photos != null)
            {
                VkAPI.Manager.Params("photos", string.Join(",", photos));
            }
            else throw new ArgumentNullException("photos");
            if (extended != null) VkAPI.Manager.Params("extended", extended.Value ? 1 : 0);
            if (photoSizes != null) VkAPI.Manager.Params("photo_sizes", photoSizes.Value ? 1 : 0);
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? result.SelectNodes("photo")
                               .Cast<XmlNode>()
                               .Select(x => new Photo(x))
                               .ToList()
                       : null;
        }

        /// <summary>
        ///     Создает пустой альбом для фотографий.
        /// </summary>
        /// <param name="title">Название альбома.</param>
        /// <param name="access">Уровень доступа к альбому. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только я.</param>
        /// <param name="comment">Уровень доступа к комментированию альбома. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только я.</param>
        /// <param name="description">Текст описания альбома.</param>
        public static async Task<PhotoAlbum> CreateAlbum(string title, AccessPrivacy access = null, AccessPrivacy comment = null,
                                             string description = null)
        {
            if (title == null) throw new ArgumentNullException("title");
            VkAPI.Manager.Method("photos.createAlbum");
            VkAPI.Manager.Params("title", title);
            if (access != null) VkAPI.Manager.Params("privacy", access.Value);
            if (comment != null) VkAPI.Manager.Params("comment_privacy", comment.Value);
            if (description != null)
            {
                VkAPI.Manager.Params("description", description);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new PhotoAlbum(result.SelectSingleNode("album")) : null;
        }

        /// <summary>
        ///     Создает пустой альбом для фотографий.
        /// </summary>
        /// <param name="gid">Идентификатор группы, в которой создаётся альбом. В этом случае privacy и comment_privacy могут принимать два значения: 0 - доступ для всех пользователей, 1 - доступ только для участников группы.</param>
        /// <param name="title">Название альбома.</param>
        /// <param name="access">Уровень доступа к альбому. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только я.</param>
        /// <param name="comment">Уровень доступа к комментированию альбома. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только я.</param>
        /// <param name="description">Текст описания альбома.</param>
        public static async Task<PhotoAlbum> CreateGroupAlbum(int gid, string title, AccessGroupPrivacy access,
                                                  AccessGroupPrivacy comment, string description)
        {
            if (title == null) throw new ArgumentNullException("title");
            VkAPI.Manager.Method("photos.createAlbum");
            VkAPI.Manager.Params("title", title);
            if (access != null) VkAPI.Manager.Params("privacy", access.Value);
            if (comment != null) VkAPI.Manager.Params("comment_privacy", comment.Value);
            VkAPI.Manager.Params("group_id", gid);
            if (description != null)
            {
                VkAPI.Manager.Params("description", description);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new PhotoAlbum(result.SelectSingleNode("album")) : null;
        }

        /// <summary>
        ///     Редактирует данные альбома для фотографий пользователя.
        /// </summary>
        /// <param name="albumId">Идентификатор редактируемого альбома.</param>
        /// <param name="title">Новое название альбома.</param>
        /// <param name="access">Новый уровень доступа к альбому. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только я.</param>
        /// <param name="comment">Новый уровень доступа к комментированию альбома. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только я.</param>
        /// <param name="description">Новый текст описания альбома.</param>
        /// <param name="ownerId">Идентификатор владельца альбома (пользователь или сообщество).</param>
        public static async Task<bool> EditAlbum(int albumId, string title = null, AccessPrivacy access = null, AccessPrivacy comment = null,
                                     string description = null, int? ownerId = null)
        {
            if (title == null) throw new ArgumentNullException("title");

            VkAPI.Manager.Method("photos.editAlbum");
            VkAPI.Manager.Params("album_id", albumId);
            if (title != null)
            {
                VkAPI.Manager.Params("title", title);
            }
            if (access != null) VkAPI.Manager.Params("privacy", access.Value);
            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId.Value);
            if (comment != null) VkAPI.Manager.Params("comment_privacy", comment.Value);
            if (description != null)
            {
                VkAPI.Manager.Params("description", description);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Возвращает адрес сервера для загрузки фотографий. После успешной загрузки фотография может быть сохранена с помощью метода photos.save.
        /// </summary>
        /// <param name="albumId">ID альбома, в который необходимо загрузить фотографии.</param>
        /// <param name="groupId">ID группы, при загрузке фотографии в группу.</param>
        public static async Task<PhotoUploadInfo> GetUploadServer(int albumId, int? groupId = null)
        {
            VkAPI.Manager.Method("photos.getUploadServer");
            VkAPI.Manager.Params("album_id", albumId);
            if (groupId != null)
            {
                VkAPI.Manager.Params("group_id", groupId);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new PhotoUploadInfo(result) : null;
        }

        /// <summary>
        ///     Изменяет описание у выбранной фотографии.
        /// </summary>
        /// <param name="photoid">ID фотографии, у которой необходимо изменить описание.</param>
        /// <param name="text">Новый текст описания к фотографии. Если параметр не задан, то считается, что он равен пустой строке.</param>
        /// <param name="userId">Идентификатор пользователя (по умолчанию - текущий пользователь). Если передано отрицательное значение, вместо фотографии пользователя будет изменена фотография группы с идентификатором -owner_id.</param>
        public static async Task<bool> EditPhoto(int photoid, string text, int? userId = null)
        {
            VkAPI.Manager.Method("photos.edit");
            if (userId != null) VkAPI.Manager.Params("owner_id", userId);
            VkAPI.Manager.Params("photo_id", photoid);
            VkAPI.Manager.Params("caption", text);
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Переносит фотографию из одного альбома в другой.
        /// </summary>
        /// <param name="photoId">ID переносимой фотографии.</param>
        /// <param name="toAlbumId">ID альбома, куда переносится фотография.</param>
        /// <param name="ownerId">ID владельца переносимой фотографии, по умолчанию id текущего пользователя.</param>
        public static async Task<bool> MovePhoto(int photoId, int toAlbumId, int? ownerId)
        {
            VkAPI.Manager.Method("photos.move");
            VkAPI.Manager.Params("photo_id", photoId);
            VkAPI.Manager.Params("target_album_id", toAlbumId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Делает фотографию обложкой альбома.
        /// </summary>
        /// <param name="photoId">ID фотографии, которая должна стать обложкой альбома.</param>
        /// <param name="albumId">ID альбома.</param>
        /// <param name="ownerId">ID владельца альбома, по умолчанию id текущего пользователя.</param>
        public static async Task<bool> UsePhotoAsCover(int photoId, int albumId, int? ownerId = null)
        {
            VkAPI.Manager.Method("photos.makeCover");
            VkAPI.Manager.Params("photo_id", photoId);
            VkAPI.Manager.Params("album_id", albumId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Меняет порядок альбома в списке альбомов пользователя.
        /// </summary>
        /// <param name="albumId">ID альбома, порядок которого нужно изменить.</param>
        /// <param name="before">ID альбома, перед которым следует поместить альбом.</param>
        /// <param name="after">ID альбома, после которого следует поместить альбом.</param>
        /// <param name="ownerId">ID владельца альбома, по умолчанию id текущего пользователя.</param>
        public static async Task<bool> ReorderAlbums(int albumId, int before, int after, int? ownerId = null)
        {
            VkAPI.Manager.Method("photos.reorderAlbums");
            VkAPI.Manager.Params("aid", albumId);
            VkAPI.Manager.Params("before", before);
            VkAPI.Manager.Params("after", after);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Меняет порядок фотографии в списке фотографий альбома пользователя.
        /// </summary>
        /// <param name="photoId">ID фотографии, порядок которой нужно изменить.</param>
        /// <param name="before">ID фотографии, перед которой следует поместить фотографию.</param>
        /// <param name="after">ID фотографии, после которой следует поместить фотографию.</param>
        /// <param name="ownerId">ID владельца фотографии, по умолчанию id текущего пользователя.</param>
        public static async Task<bool> ReorderPhotos(int photoId, int before, int after, int? ownerId = null)
        {
            VkAPI.Manager.Method("photos.reorderPhotos");
            VkAPI.Manager.Params("photo_id", photoId);
            VkAPI.Manager.Params("before", before);
            VkAPI.Manager.Params("after", after);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        /// Возвращает адрес сервера для загрузки главной фотографии на страницу пользователя или сообщества.
        /// </summary>
        /// <param name="ownerId">Идентификатор сообщества или текущего пользователя.</param>
        public static async Task<string> GetOwnerUploadServer(int? ownerId = null)
        {
            VkAPI.Manager.Method("photos.getOwnerUploadServer");

            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (!VkAPI.Manager.MethodSuccessed)
                return null;

            return result.String("upload_url");
        }

        /// <summary>
        /// Позволяет получить адрес для загрузки фотографий мультидиалогов.
        /// </summary>
        /// <param name="cropX"></param>
        /// <param name="cropY"></param>
        /// <param name="cropWidth">Ширина фотографии после обрезки в px. </param>
        /// <param name="chatId">Идентификатор беседы, для которой нужно загрузить фотографию. </param>
        public static async Task<string> GetChatUploadServer(int chatId, int? cropX = null, int? cropY = null, int? cropWidth = null)
        {
            VkAPI.Manager.Method("photos.getChatUploadServer");
            VkAPI.Manager.Params("chat_id", chatId);
            if (cropX != null)
            {
                VkAPI.Manager.Params("crop_x", cropX);
            }
            if (cropY != null)
            {
                VkAPI.Manager.Params("crop_y", cropY);
            }
            if (cropWidth != null)
            {
                VkAPI.Manager.Params("crop_width", cropWidth);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (!VkAPI.Manager.MethodSuccessed)
                return null;

            return result.String("upload_url");
        }

        /// <summary>
        ///     Возвращает адрес сервера для загрузки﻿ фотографии в личное сообщение пользователю. После успешной загрузки Вы можете сохранить фотографию, воспользовавшись методом photos.saveMessagesPhoto.
        /// </summary>
        /// <param name="groupId">Идентификатор сообщества, в альбом которого нужно загрузить фотографию.</param>
        /// <returns>Возвращает объект с единственным полем upload_url. </returns>
        public static async Task<string> GetMessagesUploadServer(int? groupId = null)
        {
            VkAPI.Manager.Method("photos.getMessagesUploadServer");
            if (groupId != null)
            {
                VkAPI.Manager.Params("group_id", groupId);
            }
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (!VkAPI.Manager.MethodSuccessed)
                return null;

            return result.String("upload_url");
        }

        /// <summary>
        ///     Возвращает адрес сервера для загрузки﻿ фотографии на стену пользователя. После успешной загрузки Вы можете сохранить фотографию, воспользовавшись методом photos.saveWallPhoto.
        /// </summary>
        /// <param name="gid">ID группы, при загрузке фотографии на стену группы.</param>
        public static async Task<string> GetWallUploadServer(int? gid = null)
        {
            VkAPI.Manager.Method("photos.getWallUploadServer");
            if (gid != null) VkAPI.Manager.Params("group_id", gid);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (!VkAPI.Manager.MethodSuccessed)
                return "";

            return result.String("upload_url");
        }

        /// <summary>
        /// Сохраняет фотографии после успешной загрузки на URI, полученный методом photos.getWallUploadServer.
        /// </summary>
        /// <param name="photo">Параметр, возвращаемый в результате загрузки фотографии на сервер.</param>
        /// <param name="userId">Идентификатор пользователя, на стену которого нужно сохранить фотографию.</param>
        /// <param name="gid">Идентификатор сообщества, на стену которого нужно сохранить фотографию.</param>
        public static async Task<Photo> SaveWallPhoto(string photo, int? uid = null,
                                                    int? gid = null)
        {
            if (uid != null & gid != null)
                throw new ArgumentException("Невозможно выполнить метод с обоми параетрами одновременно");

            VkAPI.Manager.Method("photos.saveWallPhoto");
            VkAPI.Manager.Params("photo", photo);
            if (uid != null) VkAPI.Manager.Params("user_id", uid);
            if (gid != null) VkAPI.Manager.Params("group_id", gid);
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new Photo(result.SelectSingleNode("photo")) : null;
        }

        /// <summary>
        /// Позволяет сохранить главную фотографию пользователя или сообщества.
        /// </summary>
        /// <param name="server">Параметр, возвращаемый в результате загрузки фотографии на сервер.</param>
        /// <param name="photo">Параметр, возвращаемый в результате загрузки фотографии на сервер.</param>
        /// <param name="hash">Параметр, возвращаемый в результате загрузки фотографии на сервер.</param>
        public static async Task<Photo> SaveOwnerPhoto(string server, string photo, string hash, bool? photoSizes = null)
        {
            VkAPI.Manager.Method("photos.saveProfilePhoto");
            VkAPI.Manager.Params("server", server);
            VkAPI.Manager.Params("photo", photo);
            VkAPI.Manager.Params("hash", hash);
            if (photoSizes != null)
            {
                VkAPI.Manager.Params("photo_sizes", photoSizes);
            }
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new Photo(result.SelectSingleNode("photo")) : null;
        }

        /// <summary>
        ///     Сохраняет фотографию после успешной загрузки на URI, полученный методом photos.getMessagesUploadServer.
        /// </summary>
        /// <param name="photo">Параметр, возвращаемый в результате загрузки фотографий на сервер.</param>
        /// <returns>Возвращает массив с загруженной фотографией, возвращённый объект имеет поля id, pid, aid, owner_id, src, src_big, src_small, created. В случае наличия фотографий в высоком разрешении также будут возвращены адреса с названиями src_xbig и src_xxbig. </returns>
        public static async Task<Photo> SaveMessagesPhoto(string photo)
        {
            VkAPI.Manager.Method("photos.saveMessagesPhoto");
            VkAPI.Manager.Params("photo", photo);
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new Photo(result.SelectSingleNode("photo")) : null;
        }

        /// <summary>
        ///     Сохраняет фотографии после успешной загрузки.
        /// </summary>
        /// <param name="aid">ID альбома, в который необходимо загрузить фотографии.</param>
        /// <param name="server">Параметр, возвращаемый в результате загрузки фотографий на сервер.</param>
        /// <param name="photosList">Параметр, возвращаемый в результате загрузки фотографий на сервер.</param>
        /// <param name="hash">Параметр, возвращаемый в результате загрузки фотографий на сервер.</param>
        /// <param name="gid">ID группы, при загрузке фотографии в группу.</param>
        /// <param name="caption">Описание фотографии.</param>
        /// <param name="description">Текст описания альбома.</param>
        /// <param name="lat">Географическая широта, заданная в градусах (от -90 до 90)</param>
        /// <param name="lng">Географическая долгота, заданная в градусах</param>
        public static async Task<Photo> SavePhoto(int aid, string server, string photosList, string hash, int? gid,
                                                string caption = null, string description = null, double? lat = null, double? lng = null)
        {
            VkAPI.Manager.Method("photos.save");
            VkAPI.Manager.Params("album_id", aid);
            VkAPI.Manager.Params("server", server);
            VkAPI.Manager.Params("photos_list", photosList);
            VkAPI.Manager.Params("hash", hash);
            if (gid != null) VkAPI.Manager.Params("group_id", gid);
            if (caption != null) VkAPI.Manager.Params("caption", caption);
            if (description != null) VkAPI.Manager.Params("description", description);
            if (lat != null) VkAPI.Manager.Params("latitude", lat);
            if (lng != null) VkAPI.Manager.Params("longtitude", lng);

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new Photo(result.SelectSingleNode("photo")) : null;
        }

        /// <summary>
        /// Позволяет пожаловаться на комментарий к фотографии.
        /// </summary>
        /// <param name="commentId">Идентификатор комментария. </param>
        /// <param name="ownerId">Идентификатор владельца фотографии к которой оставлен комментарий.</param>
        /// <param name="reason">Тип жалобы</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> ReportComment(int commentId, int ownerId, ReportReason reason = null)
        {
            VkAPI.Manager.Method("photos.reportComment");
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
        /// Позволяет пожаловаться на фотографию.
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии. </param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит фотография. </param>
        /// <param name="reason">Тип жалобы</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> ReportPhoto(int photoId, int ownerId, ReportReason reason = null)
        {
            VkAPI.Manager.Method("photos.report");
            VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("photo_id", photoId);
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
    }
}