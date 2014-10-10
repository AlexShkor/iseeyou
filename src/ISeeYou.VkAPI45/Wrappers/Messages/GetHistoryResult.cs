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
    /// Результат выполнения метода messages.getHistory
    /// </summary>
    public class GetHistoryResult
    {
        public GetHistoryResult(XmlNode node)
        {
            Items = new ListCount<Message>(node.Int("count").Value, Messages.BuildMessagesList(node, MessageType.History, "message"));
            Unread = node.Int("unread");
            Skipped = node.Int("skipped");
        }

        /// <summary>
        /// Количество пропущенных сообщений (реальное значение offset, которое использовалось для получения интервала истории).
        /// </summary>
        public int? Skipped { get; set; }

        /// <summary>
        /// Количество непрочитанных входящих сообщений
        /// </summary>
        public int? Unread { get; set; }

        /// <summary>
        /// Список сообщений
        /// </summary>
        public ListCount<Message> Items { get; set; }
    }
}
