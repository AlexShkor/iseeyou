#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Wall
{
    /// <summary>
    /// Стена
    /// </summary>
    public static class Wall
    {
        /// <summary>
        ///     Возвращает список записей со стены пользователя или сообщества.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь). Чтобы получить записи со стены группы (публичной страницы, встречи), укажите её идентификатор со знаком "минус": например, owner_id=-1 соответствует группе с идентификатором 1.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений.</param>
        /// <param name="filter">Определяет, какие типы сообщений на стене необходимо получить.</param>
        public static async Task<ListCount<WallEntity>> Get(int? ownerId = null, string domain = null, int? count = null, int? offset = null, WallEntityFilter filter = null)
        {
            VkAPI.Manager.Method("wall.get");
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
            if (filter != null)
            {
                VkAPI.Manager.Params("filter", filter.StringValue);
            }
            if (domain != null)
            {
                VkAPI.Manager.Params("domain", domain);
            }

            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                return new ListCount<WallEntity>(resp.Int("count").Value, resp.SelectNodes("items/post").Cast<XmlNode>().Select(y => new WallEntity(y)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Метод позволяющий осуществлять поиск по стенам пользователей.
        /// </summary>
        /// <param name="q">Поисковой запрос.</param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества. </param>
        /// <param name="domain">Короткий адрес пользователя или сообщества. </param>
        /// <param name="ownersOnly">true — возвращать только записи от имени владельца стены. </param>
        /// <param name="count">Количество записей, которые необходимо вернуть. </param>
        /// <param name="offset">Смещение. </param>
        /// <param name="extended">Возвращать ли расширенную информацию о записях. </param>
        public static async Task<ListCount<WallEntity>> Search(string q, int? ownerId = null, string domain = null, bool? ownersOnly = null,
                                                  int? count = null, int? offset = null, bool? extended = null)
        {
            VkAPI.Manager.Method("wall.search");
            VkAPI.Manager.Params("query", q);

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
            if (domain!= null)
            {
                VkAPI.Manager.Params("domain", domain);
            }
            if (ownersOnly != null)
            {
                VkAPI.Manager.Params("owners_only", ownersOnly);
            }
            if (extended != null)
            {
                VkAPI.Manager.Params("extended", extended);
            }

            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                return new ListCount<WallEntity>(resp.Int("count").Value, resp.SelectNodes("items/post").Cast<XmlNode>().Select(y => new WallEntity(y)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список записей со стен пользователей по их идентификаторам.
        /// </summary>
        /// <param name="posts">Перечисленные через запятую идентификаторы, которые представляют собой идущие через знак подчеркивания id владельцев стен и id самих записей на стене.</param>
        public static async Task<List<WallEntity>> GetById(IEnumerable<string> posts, short? copyHistoryDepth = null)
        {
            VkAPI.Manager.Method("wall.getById");

            VkAPI.Manager.Params("posts", posts.Aggregate((a, b) => a + "," + b));
            if (copyHistoryDepth != null)
            {
                VkAPI.Manager.Params("copy_history_depth", copyHistoryDepth);
            }

            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                return resp.SelectNodes("post").Cast<XmlNode>().Select(y => new WallEntity(y)).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Публикует новую запись на своей или чужой стене.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя, у которого должна быть опубликована запись. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя.</param>
        /// <param name="message">Текст сообщения (является обязательным, если не задан параметр attachments)</param>
        /// <param name="attachments">Cписок объектов, приложенных к записи и разделённых символом ",".</param>
        /// <param name="latitude">Географическая широта отметки, заданная в градусах (от -90 до 90).</param>
        /// <param name="longitude">Географическая долгота отметки, заданная в градусах (от -180 до 180).</param>
        /// <param name="placeId">Идентификатор места, в котором отмечен пользователь</param>
        /// <param name="fromGroup">Данный параметр учитывается, если owner_id меньше 0 (статус публикуется на стене группы). 1 - статус будет опубликован от имени группы, 0 - статус будет опубликован от имени пользователя (по умолчанию).</param>
        /// <param name="services">Список сервисов или сайтов, на которые необходимо экспортировать запись, в случае если пользователь настроил соответствующую опцию. Например, twitter, facebook. </param>
        /// <param name="placeId">Идентификатор места, в котором отмечен пользователь.</param>
        /// <param name="postId">Идентификатор записи, которую необходимо опубликовать. Данный параметр используется для публикации отложенных записей и предложенных новостей. </param>
        ///<param name="publishDate">Дата публикации записи в формате unixtime. Если параметр указан, публикация записи будет отложена до указанного времени. </param>
        /// <param name="signed">1 - у статуса, размещенного от имени группы будет добавлена подпись (имя пользователя, разместившего запись), 0 - подписи добавлено не будет. Параметр учитывается только при публикации на стене группы и указании параметра from_group. По умолчанию подпись не добавляется.</param>
        /// <param name="friendsOnly">1 - статус будет доступен только друзьям, 0 - всем пользователям. По умолчанию публикуемые статусы доступны всем пользователям.</param>
        /// <returns>В случае успеха возвращает идентификатор созданной записи. </returns>
        public static async Task<int> Post(int? ownerId, string message, int? publishDate = null, int? postId = null, int? placeId = null, IEnumerable<WallAttachment> attachments = null, short? latitude = null,
                               short? longitude = null, bool? fromGroup = null, bool? signed = null, bool? friendsOnly = null, string services = null)
        {
            if (message == null && attachments == null)
                throw new ArgumentNullException("message" + "&" + "attachments");
            VkAPI.Manager.Method("wall.post");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (postId != null)
            {
                VkAPI.Manager.Params("post_id", postId.Value);
            }
            if (placeId != null)
            {
                VkAPI.Manager.Params("place_id", placeId.Value);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (services != null)
            {
                VkAPI.Manager.Params("services", services);
            }
            if (publishDate != null)
            {
                VkAPI.Manager.Params("publish_date", publishDate.Value);
            }
            if (latitude != null)
            {
                VkAPI.Manager.Params("lat", latitude);
            }
            if (longitude != null)
            {
                VkAPI.Manager.Params("long", longitude);
            }
            if (fromGroup != null)
            {
                VkAPI.Manager.Params("from_group", fromGroup);
            }
            if (signed != null)
            {
                VkAPI.Manager.Params("signed", signed);
            }
            if (friendsOnly != null)
            {
                VkAPI.Manager.Params("friends_only", friendsOnly);
            }
            if (message != null)
            {
                VkAPI.Manager.Params("message", message);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && resp.Int("post_id").HasValue ? resp.Int("post_id").Value : -1;
        }

        /// <summary>
        /// Копирует объект на стену пользователя или сообщества.
        /// </summary>
        /// <param name="obj">Строковый идентификатор объекта, который необходимо разместить на стене, например, wall66748_3675 или wall-1_340364. </param>
        /// <param name="message">Сопроводительный текст, который будет добавлен к записи с объектом.</param>
        /// <param name="groupId">Идентификатор сообщества, на стене которого будет размещена запись с объектом. Если не указан, запись будет размещена на стене текущего пользователя. </param>
        public static async Task<RepostSuccess> Repost(string obj, string message = null, int? groupId = null)
        {
            VkAPI.Manager.Method("wall.repost");
            VkAPI.Manager.Params("object", obj);
            if (message != null)
            {
                VkAPI.Manager.Params("message", message);
            }
            if (groupId != null)
            {
                VkAPI.Manager.Params("group_id", groupId.Value);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                return new RepostSuccess(resp);
            }
            return null;
        }

        /// <summary>
        /// Позволяет получать список репостов заданной записи.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, на стене которого находится запись. Если параметр не задан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="postId">Идентификатор записи на стене.</param>
        /// <param name="count">Количество записей, которое необходимо получить. </param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества записей. </param>
        public static async Task<List<WallEntity>> GetReposts(int? ownerId = null, int? postId = null, int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("wall.getReposts");
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
            if (postId != null)
            {
                VkAPI.Manager.Params("post_id", postId);
            }

            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                return new List<WallEntity>(resp.SelectNodes("//items/item").Cast<XmlNode>().Select(y => new WallEntity(y)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Редактирует запись на своей или чужой стене.
        /// </summary>
        /// <param name="postId">Идентификатор записи на стене пользователя.</param>
        /// <param name="ownerId">Идентификатор пользователя, на чьей стене находится запись, которую необходимо отредактировать. Если параметр не задан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="message">Текст сообщения (является обязательным, если не задан параметр attachments)</param>
        /// <param name="attachments">Список объектов, приложенных к записи и разделённых символом ",". </param>
        /// <param name="latitude">Географическая широта отметки, заданная в градусах (от -90 до 90).</param>
        /// <param name="longitude">Географическая долгота отметки, заданная в градусах (от -180 до 180).</param>
        /// <param name="placeId">Идентификатор места, в котором отмечен пользователь</param>
        /// <param name="friendsOnly">true — запись будет доступна только друзьям, false — всем пользователям. Параметр учитывается только при редактировании отложенной записи. </param>
        /// <param name="publishDate">Дата публикации записи в формате unixtime. Если параметр не указан, отложенная запись будет опубликована. Параметр учитывается только при редактировании отложенной записи. </param>
        /// <param name="services">Список сервисов или сайтов, на которые необходимо экспортировать запись, в случае если пользователь настроил соответствующую опцию. Например, twitter, facebook. Параметр учитывается только при редактировании отложенной записи. </param>
        /// <param name="signed">true — у записи, размещенной от имени сообщества будет добавлена подпись (имя пользователя, разместившего запись), false — подписи добавлено не будет. Параметр учитывается только при редактировании записи на стене сообщества, опубликованной от имени группы. </param>
        /// <returns>В случае успешного сохранения записи метод возвратит true. </returns>
        public static async Task<bool> Edit(int postId, int? ownerId, string message, IEnumerable<string> attachments,
                                short? latitude, short? longitude, int? placeId, bool? friendsOnly = null, string services = null, int? publishDate = null, bool? signed = null)
        {
            if (message == null && attachments == null)
                throw new ArgumentNullException("message" + " & " + "attachments");
            VkAPI.Manager.Method("wall.post");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId); //((type == MessageType.Outgoing) ? "1" : "0"));;
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Aggregate((a, b) => a + "," + b));
            }
            if (latitude != null)
            {
                VkAPI.Manager.Params("lat", latitude);
            }
            if (longitude != null)
            {
                VkAPI.Manager.Params("long", longitude);
            }
            if (placeId != null)
            {
                VkAPI.Manager.Params("place_id", placeId);
            }
            if (services != null)
            {
                VkAPI.Manager.Params("services", services);
            }
            if (friendsOnly != null)
            {
                VkAPI.Manager.Params("friends_only", friendsOnly.Value ? "1" : "0");
            }
            if (publishDate != null)
            {
                VkAPI.Manager.Params("publish_date", publishDate);
            }
            if (signed != null)
            {
                VkAPI.Manager.Params("signed", signed.Value ? "1" : "0");
            }
            VkAPI.Manager.Params("post_id", postId);
            VkAPI.Manager.Params("message", message);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return false;
                }

                return (resp.InnerText.Equals("1"));
            }
            return false;
        }

        /// <summary>
        ///     Удаляет запись со стены пользователя.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя, на чьей стене необходимо удалить запись. Если параметр не задан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="postId">Идентификатор записи на стене пользователя.</param>
        /// <returns>В случае успешного удаления записи со стены пользователя возвращает true. </returns>
        public static async Task<bool> Delete(int postId, int? ownerId = null)
        {
            VkAPI.Manager.Method("wall.delete");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            VkAPI.Manager.Params("post_id", postId);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return false;
                }

                return (resp.InnerText.Equals("1"));
            }
            return false;
        }

        /// <summary>
        ///     Восстанавливает удаленную запись на стене пользователя.
        /// </summary>
        /// <param name="postId">Идентификатор записи на стене пользователя.</param>
        /// <param name="ownerId">Идентификатор пользователя, на чьей стене необходимо восстановить запись. Если параметр не задан, то он считается равным идентификатору текущего пользователя.</param>
        /// <returns></returns>
        public static async Task<bool> Restore(int postId, int? ownerId = null)
        {
            VkAPI.Manager.Method("wall.restore");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            VkAPI.Manager.Params("post_id", postId);
            
            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return false;
                }

                return (resp.InnerText.Equals("1"));
            }
            return false;
        }

        /// <summary>
        ///     Возвращает список комментариев к записи на стене пользователя.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя, на чьей стене находится запись, к которой необходимо получить комментарии. Если параметр не задан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="postId">Идентификатор записи на стене пользователя.</param>
        /// <param name="sortOrder">Порядок сортировки комментариев</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества комментариев.</param>
        /// <param name="count">Количество комментариев, которое необходимо получить (но не более 100).</param>
        /// <param name="needLikes">1 - будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается.</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать комментарии. Укажите 0, если Вы не хотите обрезать комментарии. (по умолчанию 90). Обратите внимание, что комментарии обрезаются по словам.</param>
        public static async Task<ListCount<Comment>> GetComments(int postId, int? ownerId = null, SortOrder sortOrder = null,
                                                     int? offset = null, int? count = null, bool? needLikes = null,
                                                     int? previewLength = null)
        {
            VkAPI.Manager.Method("wall.getComments");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            VkAPI.Manager.Params("post_id", postId);
            if (sortOrder != null)
            {
                VkAPI.Manager.Params("sort", sortOrder.StringValue);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (needLikes != null)
            {
                VkAPI.Manager.Params("need_likes", needLikes.Value ? 1 : 0);
            }
            if (previewLength != null)
            {
                VkAPI.Manager.Params("preview_length", previewLength);
            }

            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                XmlNodeList nodes = resp.SelectNodes("//comment");
                return nodes.Count > 0
                           ? new ListCount<Comment>(resp.Int("count").Value, (from XmlNode node in nodes select new Comment(node)).ToList<Comment>())
                           : null;
            }
            return null;
        }

        /// <summary>
        ///     Добавляет комментарий к записи на стене пользователя.
        /// </summary>
        /// <param name="postId">Идентификатор записи на стене пользователя.</param>
        /// <param name="text">Текст комментария к записи на стене пользователя.</param>
        /// <param name="ownerId">Идентификатор пользователя, на чьей стене находится запись к которой необходимо добавить комментарий. Если параметр не задан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="replyToCid">Идентификатор комментария, ответом на который является добавляемый комментарий.</param>
        /// <param name="attachments">Список объектов, приложенных к комментарию и разделённых символом ",".</param>
        /// <param name="fromGroup">Данный параметр учитывается, если owner_id менее 0 (комментарий публикуется на стене группы). true — комментарий будет опубликован от имени группы, false — комментарий будет опубликован от имени пользователя (по умолчанию). </param>
        /// <returns>В случае успешного добавления комментария к записи возвращает идентификатор добавленного комментария на стене пользователя. </returns>
        public static async Task<int> AddComment(int postId, string text, int? ownerId = null, int? replyToCid = null,
                                     IEnumerable<string> attachments = null, bool? fromGroup = null)
        {
            VkAPI.Manager.Method("wall.addComment");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            VkAPI.Manager.Params("post_id", postId);
            VkAPI.Manager.Params("text", text);
            if (fromGroup != null)
            {
                VkAPI.Manager.Params("from_group", fromGroup.Value ? "1" : "0");
            }
            if (replyToCid != null)
            {
                VkAPI.Manager.Params("reply_to_comment", replyToCid);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Aggregate((a, b) => a + "," + b));
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && resp.Int("comment_id").HasValue ? resp.Int("comment_id").Value : -1;
        }

        /// <summary>
        /// Редактирует комментарий на стене пользователя или сообщества.
        /// </summary>
        /// <param name="commentId">Идентификатор комментария, который необходимо отредактировать.</param>
        /// <param name="ownerId">Идентификатор владельца стены. </param>
        /// <param name="text">Новый текст комментария.</param>
        /// <param name="attachments">Новые вложения к комментарию. </param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> EditComment(int commentId, int? ownerId = null, string text = null, IEnumerable<string> attachments = null)
        {
            VkAPI.Manager.Method("wall.addComment");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            VkAPI.Manager.Params("comment_id", commentId);
            if (text != null)
            {
                VkAPI.Manager.Params("message", text);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Aggregate((a, b) => a + "," + b));
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Удаляет комментарий текущего пользователя к записи на своей или чужой стене.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя, на чьей стене находится комментарий к записи. Если параметр не задан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="commentId">Идентификатор комментария на стене пользователя.</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> DeleteComment(int commentId, int? ownerId)
        {
            VkAPI.Manager.Method("wall.deleteComment");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            VkAPI.Manager.Params("comment_id", commentId);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Восстанавливает комментарий текущего пользователя к записи на своей или чужой стене.
        /// </summary>
        /// <param name="ownerId">Идентификатор пользователя, на чьей стене находится комментарий к записи. Если параметр не задан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="commentId">Идентификатор комментария на стене пользователя.</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> RestoreComment(int commentId, int? ownerId)
        {
            VkAPI.Manager.Method("wall.restoreComment");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            VkAPI.Manager.Params("comment_id", commentId);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        /// Позволяет пожаловаться на запись.
        /// </summary>
        /// <param name="commentId">Идентификатор комментария. </param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит комментарий. </param>
        /// <param name="reason">Тип жалобы</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> ReportComment(int commentId, int ownerId, ReportReason reason = null)
        {
            VkAPI.Manager.Method("wall.reportComment");
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
        /// Позволяет пожаловаться на запись.
        /// </summary>
        /// <param name="postId">Идентификатор записи. </param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит запись. </param>
        /// <param name="reason">Тип жалобы</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> ReportPost(int postId, int ownerId, ReportReason reason = null)
        {
            VkAPI.Manager.Method("wall.reportPost");
            VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("post_id", postId);
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