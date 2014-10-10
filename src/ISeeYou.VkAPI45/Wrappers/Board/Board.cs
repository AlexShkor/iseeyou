#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Board
{
    /// <summary>
    /// Обсуждения
    /// </summary>
    public static class Board
    {
        /// <summary>
        ///     Возвращает список тем в обсуждениях указанной группы.
        /// </summary>
        /// <param name="gid">ID группы, список тем которой необходимо получить.</param>
        /// <param name="tids">Список идентификаторов тем, которые необходимо получить (не более 100). По умолчанию возвращаются все темы. Если указан данный параметр, игнорируются параметры order, offset и count (возвращаются все запрошенные темы в указанном порядке).</param>
        /// <param name="order">
        ///     Порядок, в котором необходимо вернуть список тем. Возможные значения:
        ///     1 - по убыванию даты обновления,
        ///     2 - по убыванию даты создания,
        ///     -1 - по возрастанию даты обновления,
        ///     -2 - по возрастанию даты создания.
        ///     По умолчанию темы возвращаются в порядке, установленном администратором группы. "Прилепленные" темы при любой сортировке возвращаются первыми в списке.
        /// </param>
        /// <param name="extended">Если указать в качестве этого параметра 1, то будет возвращена информация о пользователях, являющихся создателями тем или оставившими в них последнее сообщение. По умолчанию 0.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества тем.</param>
        /// <param name="count">Количество тем, которое необходимо получить (но не более 100). По умолчанию 40.</param>
        /// <param name="flags">
        ///     Набор флагов, определяющий, необходимо ли вернуть вместе с информацией о темах текст первых и последних сообщений в них. Является суммой флагов:
        ///     1 - вернуть первое сообщение в каждой теме (поле first_comment),
        ///     2 - вернуть последнее сообщение в каждой теме (поле last_comment). По умолчанию 0 (не возвращать текст сообщений).
        /// </param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать первое и последнее сообщение. Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию 90).</param>
        public static async Task<TopicsInfo> GetTopics(int gid, IEnumerable<int> tids = null, TopicsSortOrder order = null,
                                           bool? extended = null, int? offset = null, int? count = null,
                                           ReturnPreviewFlags flags = null, int? previewLength = null)
        {
            VkAPI.Manager.Method("board.getTopics");
            VkAPI.Manager.Params("group_id", gid);
            if (tids != null)
            {
                VkAPI.Manager.Params("topic_ids", tids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (order != null)
            {
                VkAPI.Manager.Params("order", order.Value);
            }
            if (flags != null)
            {
                VkAPI.Manager.Params("preview", flags.Value);
            }
            if (extended != null)
            {
                VkAPI.Manager.Params("extended", extended.Value ? 1 : 0);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (previewLength != null)
            {
                VkAPI.Manager.Params("preview_length", previewLength.Value);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new TopicsInfo(result) : null;
        }

        /// <summary>
        ///     Удаляет тему в обсуждениях группы.
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо удалить тему.</param>
        /// <param name="tid">ID удаляемой темы</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> DeleteTopic(int gid, int tid)
        {
            VkAPI.Manager.Method("board.deleteTopic");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("topic_id", tid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает список сообщений в указанной теме.
        /// </summary>
        /// <param name="gid">ID группы, к обсуждениям которой относится указанная тема.</param>
        /// <param name="tid">ID темы в группе</param>
        /// <param name="extended">Если указать в качестве этого параметра 1, то будет возвращена информация о пользователях, являющихся авторами сообщений. По умолчанию 0.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100). По умолчанию 20.</param>
        /// <param name="needLikes">true — будет возвращено дополнительное поле likes. По умолчанию поле likes не возвращается. </param>
        /// <param name="order">порядок сортировки комментариев:
        /// asc — хронологический
        /// desc — антихронологический
        /// </param>
        public static async Task<TopicCommentsInfo> GetComments(int gid, int tid, bool? extended = null, int? offset = null,
                                                    int? count = null, bool? needLikes = null, SortOrder order = null)
        {
            VkAPI.Manager.Method("board.getComments");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("topic_id", tid);
            if (extended != null)
            {
                VkAPI.Manager.Params("extended", extended.Value ? 1 : 0);
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
                VkAPI.Manager.Params("need_likes", needLikes.Value ? 1:0);
            }
            if (order != null)
            {
                VkAPI.Manager.Params("sort", order.StringValue);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new TopicCommentsInfo(result) : null;
        }

        /// <summary>
        ///     Добавляет новое сообщение в теме группы.
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо создать новое сообщение.</param>
        /// <param name="tid">ID темы, в которой необходимо оставить новое сообщение.</param>
        /// <param name="text">Текст нового сообщения в теме. Параметр является опциональным только если указан параметр attachments.</param>
        /// <param name="attachments">Список объектов, приложенных к сообщению и разделённых символом ",". </param>
        /// <param name="fromGroup">true — сообщение будет опубликовано от имени группы, false — сообщение будет опубликовано от имени пользователя (по умолчанию). </param>
        /// <returns>В случае успеха возвращает идентификатор созданного сообщения. </returns>
        public static async Task<int> AddComment(int gid, int tid, string text = null, IEnumerable<WallAttachment> attachments = null, bool? fromGroup = null)
        {
            if (text == null & attachments == null)
            {
                throw new ArgumentException(
                    "Невозможно выполнить метод, если не указан хотя бы один с параметров: text, attachments");
            }

            VkAPI.Manager.Method("board.addComment");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("topic_id", tid);
            if (text != null)
            {
                VkAPI.Manager.Params("text", text);
            }
            if (fromGroup != null)
            {
                VkAPI.Manager.Params("from_group", fromGroup.Value ? 1 : 0);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Редактирует одно из сообщений в теме группы.
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо изменить сообщение.</param>
        /// <param name="tid">ID темы, в которой необходимо изменить сообщение.</param>
        /// <param name="cid">ID сообщения, которое необходимо изменить.</param>
        /// <param name="text">Новый текст сообщения. Параметр является опциональным только если указан параметр attachments.</param>
        /// <param name="attachments">Список объектов, приложенных к сообщению и разделённых символом ",".</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> EditComment(int gid, int tid, int cid, string text = null,
                                       IEnumerable<string> attachments = null)
        {
            if (text == null & attachments == null)
            {
                throw new ArgumentException(
                    "Невозможно выполнить метод, если не указан хотя бы один с параметров: text, attachments");
            }

            VkAPI.Manager.Method("board.editComment");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("commment_id", cid);
            VkAPI.Manager.Params("topic_id", tid);
            if (text != null)
            {
                VkAPI.Manager.Params("text", text);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Aggregate((a, b) => a + "," + b));
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Удаляет сообщение темы в обсуждениях группы.
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо удалить сообщение.</param>
        /// <param name="tid">ID темы, которой принадлежит удаляемое сообщение</param>
        /// <param name="cid">ID удаляемого сообщения</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> DeleteComment(int gid, int tid, int cid)
        {
            VkAPI.Manager.Method("board.deleteComment");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("comment_id", cid);
            VkAPI.Manager.Params("topic_id", tid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Восстанавливает удаленное сообщение темы в обсуждениях группы.
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо восстановить сообщение.</param>
        /// <param name="tid">ID темы, которой принадлежало удаленное сообщение</param>
        /// <param name="cid">ID удаленного сообщения</param>
        /// <returns>В случае успеха возвращает true</returns>
        public static async Task<bool> RestoreComment(int gid, int tid, int cid)
        {
            VkAPI.Manager.Method("board.restoreComment");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("comment_id", cid);
            VkAPI.Manager.Params("topic_id", tid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Создает новую тему в списке обсуждений группы.
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо создать новую тему.</param>
        /// <param name="title">Заголовок создаваемой темы.</param>
        /// <param name="text">Текст первого сообщения в теме.</param>
        /// <param name="attachments">Список объектов, приложенных к записи и разделённых символом ",".</param>
        /// <param name="fromGroup">true — тема будет создана от имени группы, false — тема будет создана от имени пользователя (по умолчанию). </param>
        /// <returns>В случае успеха возвращает идентификатор созданной темы. </returns>
        public static async Task<int> AddTopic(int gid, string title, string text, bool? fromGroup = null, IEnumerable<WallAttachment> attachments = null)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            VkAPI.Manager.Method("board.addTopic");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("text", text);
            VkAPI.Manager.Params("title", title);
            if (fromGroup != null)
            {
                VkAPI.Manager.Params("from_group", fromGroup.Value ? 1 : 0);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachments", attachments.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Закрывает тему в списке обсуждений группы (в такой теме невозможно оставлять новые сообщения).
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо закрыть тему.</param>
        /// <param name="tid">ID темы, которую необходимо закрыть.</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> CloseTopic(int gid, int tid)
        {
            VkAPI.Manager.Method("board.closeTopic");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("topic_id", tid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Открывает ранее закрытую тему (в ней станет возможно оставлять новые сообщения).
        /// </summary>
        /// <param name="gid">Идентификатор сообщества, в котором размещено обсуждение.</param>
        /// <param name="tid">Идентификатор обсуждения.</param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> OpenTopic(int gid, int tid)
        {
            VkAPI.Manager.Method("board.openTopic");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("topic_id", tid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Закрепляет тему в списке обсуждений группы (такая тема при любой сортировке выводится выше остальных).
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо закрыть тему.</param>
        /// <param name="tid">ID темы, которую необходимо закрыть.</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> FixTopic(int gid, int tid)
        {
            VkAPI.Manager.Method("board.fixTopic");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("topic_id", tid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Отменяет прикрепление темы в списке обсуждений группы (тема будет выводиться согласно выбранной сортировке).
        /// </summary>
        /// <param name="gid">ID группы, в обсуждениях которой необходимо закрыть тему.</param>
        /// <param name="tid">ID темы, которую необходимо закрыть.</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> UnfixTopic(int gid, int tid)
        {
            VkAPI.Manager.Method("board.unfixTopic");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("topic_id", tid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }
    }
}