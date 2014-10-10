#region Using

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// История LongPoll сервера
    /// </summary>
    public class LongPollServerHistory
    {
        public LongPollServerHistory(XmlNode node)
        {
            var items = node.SelectNodes("history/items");
            if (items != null && items.Count > 0)
            {
                History = new int[items.Count][];
                for (int i = 0; i < items.Count - 1; i++)
                {
                    History[i] = items[i].SelectNodes("item").Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToArray();
                }
            }

            var messages = node.SelectNodes("history/messages/message");
            if (messages != null && messages.Count > 0)
            {
                Messages = messages.Cast<XmlNode>().Select(x => new Message(x)).ToList();
            }
        }

        public int[][] History { get; set; }

        /// <summary>
        /// Список сообщений
        /// </summary>
        public List<Message> Messages { get; set; }
    }
}