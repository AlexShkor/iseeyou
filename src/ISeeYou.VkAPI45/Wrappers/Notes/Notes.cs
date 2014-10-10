#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Photos;

#endregion

namespace VkAPIAsync.Wrappers.Notes
{
    /// <summary>
    /// Заметки
    /// </summary>
    public static class Notes
    {
        /// <summary>
        ///     Возвращает список заметок, созданных пользователем.
        /// </summary>
        /// <param name="userId">ID пользователя, заметки которого нужно вернуть. По умолчанию – id текущего пользователя.</param>
        /// <param name="nids">Перечисленные через запятую id заметок, входящие в выборку по userId.</param>
        /// <param name="order">Сортировка результатов (0 - по дате создания в порядке убывания, 1 - по дате создания в порядке возрастания).</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100). По умолчанию выставляется 20.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества заметок.</param>
        public static async Task<ListCount<Note>> Get(int? uid = null, IEnumerable<string> nids = null, SortOrder order = null,
                                          int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("notes.get");
            if (uid != null)
            {
                VkAPI.Manager.Params("user_id", uid);
            }
            if (nids != null)
            {
                VkAPI.Manager.Params("note_ids", nids.Aggregate((a, b) => a + "," + b));
            }
            if (order != null)
            {
                VkAPI.Manager.Params("sort", order.Value);
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
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<Note>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Note(x)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Возвращает заметку по её id.
        /// </summary>
        /// <param name="nid">ID запрашиваемой заметки.</param>
        /// <param name="ownerId">ID владельца заметки (по умолчанию используется id текущего пользователя)</param>
        /// <param name="needWiki">Определяет, требуется ли в ответе wiki-представление заметки (работает, только если запрашиваются заметки текущего пользователя)</param>
        public static async Task<NoteExtended> GetById(int nid, int? ownerId = null, bool? needWiki = null)
        {
            VkAPI.Manager.Method("notes.getById");
            VkAPI.Manager.Params("note_id", nid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (needWiki != null)
            {
                VkAPI.Manager.Params("need_wiki", needWiki.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var node = result.SelectSingleNode("note");
                if (node != null)
                    return new NoteExtended(node);
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список заметок, друзей пользователя.
        /// </summary>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100). По умолчанию выставляется 20.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества заметок.</param>
        public static async Task<ListCount<Note>> GetFriendsNotes(int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("notes.getFriendsNotes");
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
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("//note");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<Note>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Note(x)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Создает новую заметку у текущего пользователя.
        /// </summary>
        /// <param name="title">Заголовок заметки.</param>
        /// <param name="text">Текст заметки.</param>
        /// <param name="privacy">Уровень доступа к заметке. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только пользователь.</param>
        /// <param name="commentPrivacy">Уровень доступа к комментированию заметки. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только пользователь.</param>
        /// <returns>В случае успешного создания заметки метод возвратит идентификатор созданной заметки (nid).</returns>
        public static async Task<int> Add(string title, string text, AccessPrivacy privacy = null,
                              AccessPrivacy commentPrivacy = null)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            VkAPI.Manager.Method("notes.add");
            VkAPI.Manager.Params("title", title);
            VkAPI.Manager.Params("text", text);
            if (privacy != null)
            {
                VkAPI.Manager.Params("privacy", privacy.Value);
            }
            if (commentPrivacy != null)
            {
                VkAPI.Manager.Params("comment_privacy", commentPrivacy.Value);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Редактирует заметку текущего пользователя.
        /// </summary>
        /// <param name="nid">ID редактируемой заметки.</param>
        /// <param name="title">Заголовок заметки.</param>
        /// <param name="text">Текст заметки.</param>
        /// <param name="privacy">Уровень доступа к заметке. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только пользователь.</param>
        /// <param name="commentPrivacy">уровень доступа к комментированию заметки. Значения: 0 – все пользователи, 1 – только друзья, 2 – друзья и друзья друзей, 3 - только пользователь.</param>
        /// <returns>В случае успешного сохранения заметки метод возвратит true. </returns>
        public static async Task<bool> Edit(int nid, string title, string text, AccessPrivacy privacy = null,
                                AccessPrivacy commentPrivacy = null)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            VkAPI.Manager.Method("notes.edit");
            VkAPI.Manager.Params("note_id", nid);
            VkAPI.Manager.Params("title", title);
            VkAPI.Manager.Params("text", text);
            if (privacy != null)
            {
                VkAPI.Manager.Params("privacy", privacy.Value);
            }
            if (commentPrivacy != null)
            {
                VkAPI.Manager.Params("comment_privacy", commentPrivacy.Value);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Удаляет заметку текущего пользователя.
        /// </summary>
        /// <param name="nid">ID удаляемой заметки.</param>
        /// <returns>В случае успешного удаления заметки метод возвратит true. </returns>
        public static async Task<bool> Delete(int nid)
        {
            VkAPI.Manager.Method("notes.delete");
            VkAPI.Manager.Params("note_id", nid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возращает список комментариев к заметке.
        /// </summary>
        /// <param name="nid">ID заметки, комментарии которой нужно вернуть</param>
        /// <param name="ownerId">Идентификатор пользователя (по умолчанию - текущий пользователь).</param>
        /// <param name="order">Сортировка результатов (0 - по дате добавления в порядке возрастания, 1 - по дате добавления в порядке убывания).</param>
        /// <param name="count">Количество комментариев, которое необходимо получить (не более 100). По умолчанию выставляется 20.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества комментариев.</param>
        public static async Task<ListCount<Comment>> GetComments(int nid, int? ownerId = null, SortOrder order = null,
                                                     int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("notes.getComments");
            VkAPI.Manager.Params("note_id", nid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (order != null)
            {
                VkAPI.Manager.Params("sort", order.Value);
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
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<Comment>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Comment(x)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Добавляет новый комментарий к заметке.
        /// </summary>
        /// <param name="nid">ID заметки, в которой нужно создать комментарий</param>
        /// <param name="text">Текст комментария (минимальная длина - 2 символа).</param>
        /// <param name="ownerId">Идентификатор пользователя, владельца заметки (по умолчанию - текущий пользователь).</param>
        /// <param name="replyTo">ID пользователя, ответом на комментарий которого является добавляемый комментарий (не передаётся если комментарий не является ответом).</param>
        /// <returns>Возвращает идентификатор созданного комментария (cid) или код ошибки. </returns>
        public static async Task<int> CreateComment(int nid, string text, int? ownerId = null, int? replyTo = null)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            VkAPI.Manager.Method("notes.createComment");
            VkAPI.Manager.Params("message", text);
            VkAPI.Manager.Params("note_id", nid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (replyTo != null)
            {
                VkAPI.Manager.Params("reply_to", replyTo);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Редактирует указанный комментарий у заметки.
        /// </summary>
        /// <param name="cid">ID комментария, котороый нужно отредактировать</param>
        /// <param name="text">Идентификатор пользователя, владельца заметки (по умолчанию - текущий пользователь).</param>
        /// <param name="ownerId">Новый текст комментария (минимальная длина - 2 символа).</param>
        /// <returns>В случае успешного сохранения комментария метод возвратит true. </returns>
        public static async Task<bool> EditComment(int cid, string text, int? ownerId = null)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            VkAPI.Manager.Method("notes.editComment");
            VkAPI.Manager.Params("message", text);
            VkAPI.Manager.Params("comment_id", cid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Удаляет комментарий.
        /// </summary>
        /// <param name="cid">ID комментария, котороый нужно удалить</param>
        /// <param name="ownerId">Идентификатор пользователя, владельца заметки (по-умолчанию - текущий пользователь).</param>
        /// <returns>В случае успешного удаления комментария метод возвратит true. </returns>
        public static async Task<bool> DeleteComment(int cid, int? ownerId = null)
        {
            VkAPI.Manager.Method("notes.deleteComment");
            VkAPI.Manager.Params("comment_id", cid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Восстанавливает удалённый комментарий.
        /// </summary>
        /// <param name="cid">ID комментария, который нужно восстановить</param>
        /// <param name="ownerId">Идентификатор пользователя, владельца заметки (по умолчанию - текущий пользователь).</param>
        /// <returns>В случае успешного восстановления комментария метод возвратит true. </returns>
        public static async Task<bool> RestoreComment(int cid, int? ownerId = null)
        {
            VkAPI.Manager.Method("notes.restoreComment");
            VkAPI.Manager.Params("comment_id", cid);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }
    }
}