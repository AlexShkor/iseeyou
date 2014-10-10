#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// Тип сообщения
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Входящее
        /// </summary>
        Incoming = 0,

        /// <summary>
        /// Исходящее
        /// </summary>
        Outgoing = 1,

        /// <summary>
        /// Спам
        /// </summary>
        Spam = 2,

        /// <summary>
        /// История
        /// </summary>
        History,

        /// <summary>
        /// С диалогов
        /// </summary>
        Dialogs
    }

    /// <summary>
    /// Состояние сообщения
    /// </summary>
    public enum MessageState
    {
        /// <summary>
        /// Прочитанное
        /// </summary>
        Read = 1,

        /// <summary>
        /// Непрочитанное
        /// </summary>
        Unread = 0
    }

    /// <summary>
    /// Фильтр сообщений
    /// </summary>
    public enum MessageFilter
    {
        /// <summary>
        /// Только непрочитанные. С версии 5.10 игнорируется.
        /// </summary>
        OnlyUnread = 1,

        /// <summary>
        /// Не из чата. С версии 5.10 игнорируется.
        /// </summary>
        NotFromChat = 2,

        /// <summary>
        /// Только от друзей. С версии 5.10 игнорируется.
        /// </summary>
        OnlyFromFriends = 4,

        /// <summary>
        /// Важные сообщения
        /// </summary>
        ImportantMessages = 8
    }

    /// <summary>
    /// Тип отправки сообщения
    /// </summary>
    public enum SendMessageType
    {
        /// <summary>
        /// Обычное сообщение
        /// </summary>
        StandardMessage = 0,

        /// <summary>
        /// Из чата
        /// </summary>
        FromChat = 1
    }

    /// <summary>
    /// Сообщение из мультидиалога
    /// </summary>
    public class DialogMessage : Message
    {
        public DialogMessage(XmlNode node) :base(node)
        {
            ChatId = node.Int("chat_id");
            ChatActive = node.String("chat_active").Split(char.Parse(",")).ToList();
            UsersCount = node.Int("users_count");
            AdminId = node.Int("admin_id");
            Photo50 = node.String("photo_50");
            Photo100 = node.String("photo_100");
            Photo200 = node.String("photo_200");
        }

        /// <summary>
        /// Идентификатор беседы
        /// </summary>
        public int? ChatId { get; set; }

        /// <summary>
        /// Идентификаторы участников беседы
        /// </summary>
        public List<string> ChatActive { get; set; }

        /// <summary>
        /// Количество участников беседы
        /// </summary>
        public int? UsersCount { get; set; }

        /// <summary>
        /// Идентификатор создателя беседы
        /// </summary>
        public int? AdminId { get; set; }

        /// <summary>
        /// url копии фотографии беседы шириной 50px
        /// </summary>
        public string Photo50 { get; set; }

        /// <summary>
        /// url копии фотографии беседы шириной 100px
        /// </summary>
        public string Photo100 { get; set; }

        /// <summary>
        /// url копии фотографии беседы шириной 200px
        /// </summary>
        public string Photo200 { get; set; }
    }

    /// <summary>
    /// Пересланное сообщение
    /// </summary>
    public class ForwardedMessage
    {
        public ForwardedMessage(XmlNode node)
        {
            UserId = node.Int("user_id");
            Date = node.DateTimeFromUnixTime("date");
            Title = node.String("title");
            Body = node.String("body");

            var attachments = node.SelectNodes("attachments/attachment");
            if (attachments != null && attachments.Count > 0)
            {
                Attachments = attachments.Cast<XmlNode>().Select(x => new MessageAttachment(x)).ToList();
            }

            var fwdMessages = node.SelectNodes("fwd_messages/message");
            if (fwdMessages != null && fwdMessages.Count > 0)
            {
                ForwardedMessages = fwdMessages.Cast<XmlNode>().Select(x => new ForwardedMessage(x)).ToList();
            }

            Deleted = node.Bool("deleted");
            Emoji = node.Bool("emoji");
            Important = node.Bool("important");
        }

        /// <summary>
        /// Список медиа-вложений
        /// </summary>
        public List<MessageAttachment> Attachments { get; set; }

        /// <summary>
        /// Список пересланных сообщений
        /// </summary>
        public List<ForwardedMessage> ForwardedMessages { get; set; }

        /// <summary>
        /// Удалено ли сообщение
        /// </summary>
        public bool? Deleted { get; set; }

        /// <summary>
        /// Содержатся ли в сообщении emoji-смайлы
        /// </summary>
        public bool? Emoji { get; set; }

        /// <summary>
        /// Является ли сообщение важным
        /// </summary>
        public bool? Important { get; set; }

        /// <summary>
        ///    Текст сообщения
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///    Дата отправки сообщения
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///     Заголовок сообщения или беседы
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Идентификатор автора сообщения (для исходящего сообщения — идентификатор получателя)
        /// </summary>
        public int? UserId { get; set; }
    }

    /// <summary>
    /// Сообщение
    /// </summary>
    public class Message : ForwardedMessage
    {
        public Message(XmlNode node, bool isHistoryMessage = false) : base(node)
        {
            Id = node.Int("id");
            ReadState = (MessageState) node.Int("read_state");
            Type = (MessageType) node.Int("out");
            if (isHistoryMessage)
            {
                UserId = node.Int("from_id");
            }
        }

        /// <summary>
        /// Идентификатор сообщения (не возвращается для пересланных сообщений)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Статус прочтения
        /// </summary>
        public MessageState ReadState { get; set; }

        /// <summary>
        /// Тип сообщения
        /// </summary>
        public MessageType Type { get; set; }
    }
}