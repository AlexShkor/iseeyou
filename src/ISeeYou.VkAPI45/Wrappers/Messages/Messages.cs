#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// Сообщения
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// Количество сообщений в истории
        /// </summary>
        public static short HistoryMessagesCount { get; set; }

        /// <summary>
        /// Количествов входящих сообщений
        /// </summary>
        public static short IncomingMessagesCount { get; set; }

        /// <summary>
        /// Количество исходящих сообщений
        /// </summary>
        public static short OutgoingMessagesCount { get; set; }

        /// <summary>
        /// Количествов непрочитанных диалогов
        /// </summary>
        public static short UnreadDialogs { get; set; }

        /// <summary>
        /// Обновление счетчиков
        /// </summary>
        /// <param name="msgCount">Количство сообщений</param>
        /// <param name="type">Тип сообщений</param>
        internal static void UpdateCounters(short msgCount, MessageType type)
        {
            if (type == MessageType.History)
            {
                HistoryMessagesCount = msgCount;
            }
            if (type == MessageType.Dialogs)
            {
                UnreadDialogs = msgCount;
            }
            else
            {
                switch (type)
                {
                    case MessageType.Incoming:
                        IncomingMessagesCount = msgCount;
                        break;
                    case MessageType.Outgoing:
                        OutgoingMessagesCount = msgCount;
                        break;
                }
            }
        }

        /// <summary>
        ///     Возвращает список входящих либо исходящих личных сообщений текущего пользователя
        /// </summary>
        /// <param name="type">Если этот параметр равен 1, сервер вернет исходящие сообщения.</param>
        /// <param name="filter">Фильтр возвращаемых сообщений: 1 - только непрочитанные; 2 - не из чата; 4 - сообщения от друзей. Если установлен флаг "4", то флаги "1" и "2" не учитываются</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100)</param>
        /// <param name="previewLen">Количество символов, по которому нужно обрезать сообщение. Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются). Обратите внимание что сообщения обрезаются по словам</param>
        /// <param name="timeOffset">Максимальное время, прошедшее с момента отправки сообщения до текущего момента в секундах. 0, если Вы хотите получить сообщения любой давности</param>
        /// <param name="lastMessageId">Идентификатор сообщения, полученного перед тем, которое нужно вернуть последним (при условии, что после него было получено не более count сообщений, иначе необходимо использовать с параметром offset)</param>
        public static async Task<ListCount<Message>> Get(MessageType type, MessageFilter? filter = null, int? offset = null,
                                        int? count = null, int? lastMessageId = null,
                                        int? previewLen = null, int? timeOffset = null)
        {
            VkAPI.Manager.Method("messages.get");
            VkAPI.Manager.Params("out", (int) type);
            if (filter != null)
            {
                VkAPI.Manager.Params("filters", (int) filter);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (previewLen != null)
            {
                VkAPI.Manager.Params("preview_length", previewLen);
            }
            if (timeOffset != null)
            {
                VkAPI.Manager.Params("time_offset", timeOffset);
            }
            if (lastMessageId != null)
            {
                VkAPI.Manager.Params("last_message_id", lastMessageId);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes ? null : new ListCount<Message>(resp.Int("count").Value, BuildMessagesList(resp, type));
            }
            return null;
        }

        /// <summary>
        ///     Возвращает сообщения по их ID.
        /// </summary>
        /// <param name="mids">ID сообщений, которые необходимо вернуть, разделенные запятыми (не более 100).</param>
        /// <param name="previewLength">Количество слов, по которому нужно обрезать сообщение. Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются).</param>
        public static async Task<ListCount<Message>> GetById(IEnumerable<int> mids, int? previewLength = null)
        {
            VkAPI.Manager.Method("messages.getById");
            VkAPI.Manager.Params("message_ids", mids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            if (previewLength != null)
            {
                VkAPI.Manager.Params("preview_length", previewLength);
            }
            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.HasChildNodes
                           ? null
                           : new ListCount<Message>(resp.Int("count").Value, BuildMessagesList(resp, MessageType.Dialogs));
            }
            return null;
        }

        /// <summary>
        /// Формирует список сообщений, при этом обновляя счетчики
        /// </summary>
        /// <param name="x">Xml-елемент с ответом</param>
        /// <param name="messageType">Тип сообщений</param>
        /// <param name="nodeName">Название Xml-елемента, дочерные елементы которого - сообщения</param>
        internal static List<Message> BuildMessagesList(XmlNode x, MessageType messageType, string nodeName = null)
        {
            var count = x.Short("count");
            if (count.HasValue)
            {
                UpdateCounters(count.Value, messageType);
            }
            var nodes = x.SelectNodes("items/" + (string.IsNullOrWhiteSpace(nodeName) ? "*" : nodeName));
            if (nodes != null && nodes.Count > 0)
            {
                return x.SelectNodes("items/" + (string.IsNullOrWhiteSpace(nodeName) ? "*" : nodeName)).Cast<XmlNode>().Select(y => new Message(y)).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список диалогов текущего пользователя.
        /// </summary>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества диалогов.</param>
        /// <param name="count">Количество диалогов, которое необходимо получить (но не более 200).</param>
        /// <param name="previewLen">Количество символов, по которому нужно обрезать сообщение. Укажите 0, если Вы не хотите обрезать сообщение. (по умолчанию сообщения не обрезаются).</param>
        /// <param name="unread">Значение true означает, что нужно вернуть только диалоги в которых есть непрочитанные входящие сообщения. По умолчанию false. </param>
        public static async Task<GetDialogsResult> GetDialogs(bool? unread = null, int? offset = null,
                                               int? count = null, int? previewLen = null)
        {
            VkAPI.Manager.Method("messages.getDialogs");
            if (unread != null)
            {
                VkAPI.Manager.Params("unread", unread.Value ? 1 : 0);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (previewLen != null)
            {
                VkAPI.Manager.Params("preview_length", previewLen);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return new GetDialogsResult(resp);
            }
            return null;
        }

        /// <summary>
        ///     Возвращает историю сообщений для указанного пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, историю переписки с которым необходимо вернуть. Является необязательным параметром в случае с истории сообщений в беседе.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений, должен быть больше, равно 0, если не передан параметр start_message_id, и должен быть менее или равно 0, если передан. </param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 200).</param>
        /// <param name="startMid">Если значение > 0, то это идентификатор сообщения, начиная с которого нужно вернуть историю переписки, если же передано значение -1, то к значению параметра offset прибавляется количество входящих непрочитанных сообщений в конце диалога </param>
        /// <param name="rev">true – возвращать сообщения в хронологическом порядке. false – возвращать сообщения в обратном хронологическом порядке (по умолчанию), недоступен при переданном start_message_id. </param>
        /// <param name="chatId">Идентификатор диалога, историю сообщений которого необходимо получить. </param>
        /// <reremarks>
        /// Параметр start_message_id вместе с offset менее или равно 0 и count больше 0 позволяет получить интервал истории сообщений вокруг данного сообщения или вокруг начала отрезка непрочитанных входящих сообщений. 
        /// Для start_message_id > 0 к значению параметра offset прибавляется количество сообщений, чей идентификатор строго больше данного start_message_id (при offset равном 0 вернутся сообщения начиная с данного включительно и более старые, count штук). 
        /// Для start_message_id = -1 поведение такое же, как при start_message_id равном последнему сообщению в истории переписки, не являющемуся входящим непрочитанным (при отсутствии входящих непрочитанных в этом диалоге это совпадает с отсутствием параметра start_message_id), то есть к значению offset прибавляется количество входящих непрочитанных сообщений в конце истории.
        /// </reremarks>
        public static async Task<GetHistoryResult> GetHistory(int userId, int? chatId = null, int? offset = null,
                                               int? count = null, int? startMid = null, bool? rev = null)
        {
            VkAPI.Manager.Method("messages.getHistory");
            VkAPI.Manager.Params("user_id", userId);

            if (chatId != null)
            {
                VkAPI.Manager.Params("chat_id", chatId);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (startMid != null)
            {
                VkAPI.Manager.Params("start_message_id", startMid);
            }
            if (rev != null)
            {
                VkAPI.Manager.Params("rev", rev.Value ? 1 : 0);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return new GetHistoryResult(resp);
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список найденных диалогов текущего пользователя по введенной строке поиска.
        /// </summary>
        /// <param name="query">Подстрока, по которой будет производиться поиск.</param>
        /// <param name="limit"></param>
        /// <param name="fields">Поля профилей собеседников, которые необходимо вернуть.</param>
        public static async Task<List<DialogSearchItem>> SearchDialogs(string query, int? limit = null, IEnumerable<string> fields = null)
        {
            VkAPI.Manager.Method("messages.searchDialogs");
            VkAPI.Manager.Params("q", query);
            if (limit != null)
            {
                VkAPI.Manager.Params("limit", limit);
            }
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + "," + b));
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                var nodes = resp.SelectNodes("item");
                if (nodes != null && nodes.Count > 0)
                {
                    return
                        nodes.Cast<XmlNode>()
                             .Select(y => new DialogSearchItem(y))
                             .ToList();
                }
            }
            return null;
        }


        /// <summary>
        ///     Возвращает список найденных личных сообщений текущего пользователя по введенной строке поиска.
        /// </summary>
        /// <param name="query">Подстрока, по которой будет производиться поиск.</param>
        /// <param name="previewLength">Количество символов, по которому нужно обрезать сообщение. Укажите '0', если Вы не хотите обрезать сообщение</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений из списка найденных.</param>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100).</param>
        public static async Task<ListCount<Message>> Search(string query, int? previewLength = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("messages.search");
            VkAPI.Manager.Params("q", query);
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
                VkAPI.Manager.Params("preview_length", previewLength);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : new ListCount<Message>(resp.Int("count").Value,BuildMessagesList(resp, MessageType.Dialogs));
            }
            return null;
        }

        /// <summary>
        ///     Посылает сообщение.
        /// </summary>
        /// <param name="userId">ID пользователя (по умолчанию - текущий пользователь).</param>
        /// <param name="chatId">ID беседы, к которой будет относиться сообщение</param>
        /// <param name="message">Текст личного cообщения (является обязательным, если не задан параметр attachment)</param>
        /// <param name="title">Заголовок сообщения.</param>
        /// <param name="type">0 - обычное сообщение, 1 - сообщение из чата. (по умолчанию 0)</param>
        /// <param name="attachments">Медиа-приложения к личному сообщению, перечисленные через запятую.</param>
        /// <param name="forwardMessages">Идентификаторы пересылаемых сообщений, перечисленные через запятую. </param>
        /// <param name="latitude">Latitude, широта при добавлении местоположения.</param>
        /// <param name="longitude">Longitude, долгота при добавлении местоположения.</param>
        /// <param name="domain">Короткий адрес пользователя (например, illarionov)</param>
        /// <param name="userIds">Идентификаторы получателей сообщения (при необходимости создать новую беседу)</param>
        /// <param name="guid">Уникальный строковой идентификатор, предназначенный для предотвращения повторной отправки одинакового сообщения.</param>
        public static async Task<int> Send(int? userId, int? chatId, string domain = null, IEnumerable<int> userIds = null,
                               string message = null, string title = null, SendMessageType? type = null,
                               IEnumerable<MessageAttachment> attachments = null, IEnumerable<int> forwardMessages = null,
                               int? latitude = null, int? longitude = null, int? guid = null)
        {
            if (userId == null & chatId == null & userIds != null)
            {
                throw new ArgumentException(
                    "Невозможно выполнить метод, если не указан хотя бы один с параметров: user_id, chat_id, user_ids");
            }

            if (message != null & attachments != null)
            {
                throw new ArgumentException(
                    "Невозможно выполнить метод с одновременно указанными параметрами: message, attachments");
            }
            if (message == null & attachments == null)
            {
                throw new ArgumentException(
                    "Невозможно выполнить метод, если не указан хотя бы один с параметров: message, attachments");
            }

            VkAPI.Manager.Method("messages.send");
            if (userId != null)
            {
                VkAPI.Manager.Params("user_id", userId);
            }
            if (chatId != null)
            {
                VkAPI.Manager.Params("chat_id", chatId);
            }
            if (domain != null)
            {
                VkAPI.Manager.Params("domain", domain);
            }
            if (userIds != null)
            {
                VkAPI.Manager.Params("user_ids",
                                     userIds.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (message != null)
            {
                VkAPI.Manager.Params("message", message);
            }
            if (forwardMessages != null)
            {
                VkAPI.Manager.Params("forward_messages",
                                     forwardMessages.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (latitude != null)
            {
                VkAPI.Manager.Params("lat", latitude);
            }
            if (longitude != null)
            {
                VkAPI.Manager.Params("long", longitude);
            }
            if (attachments != null)
            {
                VkAPI.Manager.Params("attachment",
                                     attachments.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (guid != null)
            {
                VkAPI.Manager.Params("guid", guid);
            }
            if (title != null)
            {
                VkAPI.Manager.Params("title", title);
            }
            if (type != null)
            {
                VkAPI.Manager.Params("type", (int) type);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && resp.IntVal().HasValue ? resp.IntVal().Value : -1;
        }

        /// <summary>
        ///     Удаляет сообщение.
        /// </summary>
        /// <param name="mids">Список идентификаторов сообщений, разделённых через запятую.</param>
        /// <returns>Возвращает true в случае успешного удаления или код ошибки. </returns>
        public static async Task<bool> Delete(IEnumerable<int> mids)
        {
            if (mids == null)
            {
                throw new ArgumentNullException("mids");
            }

            VkAPI.Manager.Method("messages.delete");
            VkAPI.Manager.Params("message_ids", mids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        /// Восстанавливает удаленное сообщение
        /// </summary>
        /// <param name="mid">Идентификатор сообщения, которое нужно восстановить</param>
        /// <returns>После успешного выполнения возвращает true</returns>
        public static async Task<bool> Restore(int mid)
        {
            VkAPI.Manager.Method("messages.restore");
            VkAPI.Manager.Params("message_id", mid);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Удаляет все личные сообщения в диалоге.
        /// </summary>
        /// <param name="userId">ID пользователя.</param>
        /// <param name="offset">Начиная с какого сообщения нужно удалить переписку. (По умолчанию удаляются все сообщения начиная с первого).</param>
        /// <param name="count">Как много сообщений нужно удалить. Обратите внимание что на метод наложено ограничение, за один вызов нельзя удалить больше 10000 сообщений, поэтому если сообщений в переписке больше - метод нужно вызывать несколько раз.</param>
        /// <returns>Возвращает true в случае успешного удаления. </returns>
        public static async Task<bool> DeleteDialog(int? uid = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("messages.deleteDialog");
            if (uid != null)
            {
                VkAPI.Manager.Params("user_id", uid);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Помечает сообщения, как непрочитанные. С версии 5.10 не поддерживается
        /// </summary>
        /// <param name="mids">Cписок идентификаторов сообщений, разделенных запятой.</param>
        /// <returns>Возвращает true в случае успешной установки</returns>
        public static async Task<bool> MarkAsNew(IEnumerable<int> mids)
        {
            VkAPI.Manager.Method("messages.markAsNew");
            VkAPI.Manager.Params("message_ids", mids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Помечает сообщения, как прочитанные.
        /// </summary>
        /// <param name="mids">Список идентификаторов сообщений, разделенных запятой</param>
        /// <returns>Возвращает true в случае успешной установки</returns>
        public static async Task<bool> MarkAsRead(IEnumerable<int> mids)
        {
            VkAPI.Manager.Method("messages.markAsRead");
            VkAPI.Manager.Params("message_ids", mids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        /// Помечает сообщения как важные либо снимает отметку
        /// </summary>
        /// <param name="mids">Список идентификаторов сообщений, которые необходимо пометить</param>
        /// <param name="important">true, если сообщения необходимо пометить, как важные;
        /// false, если необходимо снять пометку</param>
        /// <returns></returns>
        public static async Task<bool> MarkAsImportant(IEnumerable<int> mids, bool important = true)
        {
            VkAPI.Manager.Method("messages.markAsImportant");
            VkAPI.Manager.Params("message_ids", mids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            VkAPI.Manager.Params("important", important);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Изменяет статус набора текста пользователем в диалоге.
        /// </summary>
        /// <param name="typing">typing - пользователь начал набирать текст</param>
        /// <param name="userId">ID пользователя (по умолчанию - текущий пользователь).</param>
        /// <returns>Возвращает true, если метод был успешно выполнен. </returns>
        public static async Task<bool> SetActivity(bool typing, int? uid = null)
        {
            VkAPI.Manager.Method("messages.setActivity");
            VkAPI.Manager.Params("type", typing ? "typing" : "");
            VkAPI.Manager.Params("uid", uid);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Возвращает текущий статус и дату последней активности указанного пользователя.
        /// </summary>
        /// <param name="userId">ID пользователя, для которого нужно получить время активности.</param>
        public static async Task<ActivityInfo> GetLastActivity(int uid)
        {
            VkAPI.Manager.Method("messages.getLastActivity");
            VkAPI.Manager.Params("user_id", uid);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes ? null : new ActivityInfo(resp);
            }
            return null;
        }

        /// <summary>
        /// Возвращает информацию о беседе
        /// </summary>
        /// <param name="chatId">Идентификатор беседы</param>
        /// <param name="chatIds">Список идентификаторов бесед</param>
        /// <param name="fields">Список дополнительных полей профилей, которые необходимо вернуть</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя. Возможные значения: именительный – nom, родительный – gen, дательный – dat, винительный – acc, творительный – ins, предложный – abl. По умолчанию nom</param>
        public static async Task<List<ChatInfo>> GetChat(int? chatId = null, IEnumerable<int> chatIds = null, IEnumerable<string> fields = null, string nameCase = null)
        {
            VkAPI.Manager.Method("messages.getChat");
            if (chatId != null)
            {
                VkAPI.Manager.Params("chat_id", chatId);
            }
            if (chatIds != null)
            {
                VkAPI.Manager.Params("chat_ids", chatIds.Select(x => x.ToString()).Aggregate((a,b) => a + "," + b));
            }
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + "," + b));
            }
            if (nameCase != null)
            {
                VkAPI.Manager.Params("name_case", nameCase);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = resp.SelectNodes("//chat");
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => new ChatInfo(x)).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Создаёт беседу с несколькими участниками.
        /// </summary>
        /// <param name="uids">Список идентификаторов друзей текущего пользователя с которыми необходимо создать беседу.</param>
        /// <param name="title">Название мультидиалога.</param>
        /// <returns>Возвращает идентификатор созданного чата в случае успешного выполнения данного метода.</returns>
        public static async Task<int> CreateChat(IEnumerable<int> uids, string title)
        {
            if (uids == null)
            {
                throw new ArgumentNullException("uids");
            }
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            VkAPI.Manager.Method("messages.createChat");
            VkAPI.Manager.Params("title", title);
            VkAPI.Manager.Params("user_ids", uids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && resp.IntVal().HasValue ? resp.IntVal().Value : -1;
        }

        /// <summary>
        ///     Изменяет название беседы.
        /// </summary>
        /// <param name="chatId">Идентификатор чата</param>
        /// <param name="title">Название беседы.</param>
        public static async Task<bool> EditChat(int chatId, string title)
        {
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }

            VkAPI.Manager.Method("messages.createChat");
            VkAPI.Manager.Params("title", title);
            VkAPI.Manager.Params("chat_id", chatId);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes ? false : resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        /// Позволяет получить список пользователей мультидиалога по его id.
        /// </summary>
        /// <param name="chatId">Идентификатор беседы</param>
        /// <param name="chatIds">Идентификаторы бесед</param>
        /// <param name="fields">Список дополнительных полей профилей, которые необходимо вернуть</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя. Возможные значения: именительный – nom, родительный – gen, дательный – dat, винительный – acc, творительный – ins, предложный – abl. По умолчанию nom. </param>
        public static async Task<List<ChatUser>> GetChatUsers(int chatId, IEnumerable<int> chatIds = null, IEnumerable<string> fields = null, string nameCase = null)
        {
            VkAPI.Manager.Method("messages.getChatUsers");
            if (chatId != null)
            {
                VkAPI.Manager.Params("chat_id", chatId);
            }
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + "," + b));
            }
            if (chatIds != null)
            {
                VkAPI.Manager.Params("chat_ids", chatIds.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }
            if (nameCase != null)
            {
                VkAPI.Manager.Params("name_case", nameCase);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                var nodes = resp.SelectNodes("user");
                if (nodes != null && nodes.Count > 0)
                {
                    return
                        nodes.Cast<XmlNode>()
                             .Select(y => new ChatUser(y))
                             .ToList();
                }
                nodes = resp.SelectNodes("user_id");
                if (nodes != null && nodes.Count > 0)
                {
                    return nodes.Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(y => new ChatUser(y.Value)).ToList();
                }
            }
            return null;
        }

        /// <summary>
        ///     Добавляет в мультидиалог нового пользователя.
        /// </summary>
        /// <param name="chatId">ID беседы, в которую необходимо добавить пользователя</param>
        /// <param name="userId">ID беседы, в которую необходимо добавить пользователя</param>
        /// <returns>После успешного выполнения возвращает true </returns>
        public static async Task<bool> AddChatUser(int chatId, int uid)
        {
            VkAPI.Manager.Method("messages.addChatUser");
            VkAPI.Manager.Params("chat_id", chatId);
            VkAPI.Manager.Params("user_id", uid);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Исключает из мультидиалога пользователя, если текущий пользователь был создателем беседы либо пригласил исключаемого пользователя. Также может быть использован для выхода текущего пользователя из беседы, в которой он состоит.
        /// </summary>
        /// <param name="chatId">ID беседы, из которой необходимо удалить пользователя.</param>
        /// <param name="userId">ID пользователя.</param>
        /// <returns>Возвращает true или код ошибки. </returns>
        public static async Task<bool> RemoveChatUser(int chatId, int uid)
        {
            VkAPI.Manager.Method("messages.removeChatUser");
            VkAPI.Manager.Params("chat_id", chatId);
            VkAPI.Manager.Params("user_id", uid);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        ///     Возвращает данные, необходимые для подключения к Long Poll серверу. Long Poll подключение позволит Вам моментально узнавать о приходе новых сообщений и других событий.
        /// </summary>
        /// <returns>
        ///     Возвращает объект, который содержит поля key, server, ts.
        ///     Используя эти данные, Вы можете подключиться к серверу быстрых сообщений для мгновенного получения приходящих сообщений и других событий.
        /// </returns>
        public static async Task<LongPollServerConnectionInfo> GetLongPollServer(bool? useSSL = null, bool? needPts = null)
        {
            VkAPI.Manager.Method("messages.getLongPollServer");
            if (useSSL != null)
            {
                VkAPI.Manager.Params("use_ssl", useSSL.Value ? 1 : 0);
            }
            if (needPts != null)
            {
                VkAPI.Manager.Params("need_pts", needPts.Value ? 1 : 0);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : new LongPollServerConnectionInfo(resp);
            }
            return null;
        }

        /// <summary>
        /// Возвращает обновления в личных сообщениях пользователя.
        /// Для ускорения работы с личными сообщениями может быть полезно кешировать уже загруженные ранее сообщения на мобильном устройстве / ПК пользователя, чтобы не получать их повторно при каждом обращении. Этот метод помогает осуществить синхронизацию локальной копии списка сообщений с актуальной версией.
        /// </summary>
        /// <param name="ts">Последнее значение параметра ts, полученное от Long Poll сервера или с помощью метода messages.getLongPollServer</param>
        /// <param name="pts"></param>
        /// <param name="previewLength"></param>
        /// <param name="onlines"></param>
        /// <param name="eventsLimit"></param>
        /// <param name="messagesLimit"></param>
        /// <param name="maxMsgId"> Максимальный идентификатор сообщения среди уже имеющихся в локальной копии. Необходимо учитывать как сообщения, полученные через методы API (например messages.getDialogs, messages.getHistory), так и данные, полученные из Long Poll сервера (события с кодом 4).</param>
        /// <returns>Возвращает объект, который содержит поля history и messages. </returns>
        public static async Task<LongPollServerHistory> GetLongPollServerHistory(int ts, int pts, int? previewLength = null, bool? onlines = null, 
                                                                     int? eventsLimit = null, int? messagesLimit = null, int? maxMsgId = null)
        {
            VkAPI.Manager.Method("messages.getLongPollServer");
            VkAPI.Manager.Params("ts", ts);
            VkAPI.Manager.Params("pts", pts);

            if (previewLength != null)
            {
                VkAPI.Manager.Params("preview_length", previewLength);
            }
            if (onlines != null)
            {
                VkAPI.Manager.Params("onlines", onlines.Value ? 1 : 0);
            }
            if (eventsLimit != null)
            {
                VkAPI.Manager.Params("events_limit", eventsLimit);
            }
            if (messagesLimit != null)
            {
                VkAPI.Manager.Params("msgs_limit", messagesLimit);
            }
            if (maxMsgId != null)
            {
                VkAPI.Manager.Params("max_msg_id", maxMsgId);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : new LongPollServerHistory(resp);
            }
            return null;
        }

        /// <summary>
        /// Позволяет установить фотографию мультидиалога, загруженную с помощью метода photos.getChatUploadServer.
        /// </summary>
        /// <param name="file">Содержимое поля response из ответа специального upload сервера, полученного в результате загрузки изображения на адрес, полученный методом photos.getChatUploadServer. </param>
        public static async Task<ChatPhotoResult> SetChatPhoto(string file)
        {
            VkAPI.Manager.Method("messages.setChatPhoto");
            VkAPI.Manager.Params("file", file);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : new ChatPhotoResult(resp);
            }
            return null;
        }

        /// <summary>
        /// Позволяет удалить фотографию мультидиалога
        /// </summary>
        /// <param name="chatId">Идентификатор беседы</param>
        public static async Task<ChatPhotoResult> DeleteChatPhoto(int chatId)
        {
            VkAPI.Manager.Method("messages.deleteChatPhoto");
            VkAPI.Manager.Params("chat_id", chatId);

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : new ChatPhotoResult(resp);
            }
            return null;
        }
    }
}