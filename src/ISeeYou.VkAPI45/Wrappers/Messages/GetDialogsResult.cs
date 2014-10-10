using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// Результат выполнения метода messages.getDialogs
    /// </summary>
    public class GetDialogsResult
    {
        public GetDialogsResult(XmlNode node)
        {
            var dialogs = node.SelectNodes("items/*");
            if (dialogs.Count > 0)
            {
                Dialogs = new ListCount<DialogInfo>(node.Int("count").Value, dialogs.Cast<XmlNode>().Select(x => new DialogInfo(x)).ToList());
            }
            UnreadDialogs = node.Short("unread_dialogs");
            if (UnreadDialogs == null)
                UnreadDialogs = 0;
            Messages.UpdateCounters(UnreadDialogs.Value, MessageType.Dialogs);
        }

        /// <summary>
        /// Список диалогов
        /// </summary>
        public ListCount<DialogInfo> Dialogs { get; set; }

        /// <summary>
        /// Количество непрочитанных входящих сообщений
        /// </summary>
        public short? UnreadDialogs { get; set; }
    }

    /// <summary>
    /// Информация о диалоге
    /// </summary>
    public class DialogInfo
    {
        public DialogInfo(XmlNode node)
        {
            Unread = node.Short("unread");
            if (Unread == -1)
            {
                Unread = 0;
            }

            var messageNode = node.SelectSingleNode("message");
            if (messageNode != null)
            {
                Message = new Message(messageNode);
            }
        }

        /// <summary>
        /// Количество непрочитанных сообщений
        /// </summary>
        public short? Unread { get; set; }

        /// <summary>
        /// Последнее сообщение
        /// </summary>
        public Message Message { get; set; }
    }
}
