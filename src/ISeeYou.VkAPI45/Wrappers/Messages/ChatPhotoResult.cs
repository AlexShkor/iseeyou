using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// Информация о операциях с фотографией чата
    /// </summary>
    public class ChatPhotoResult
    {
        public ChatPhotoResult(XmlNode node)
        {
            MessageId = node.Int("message_id");
            var chatNode = node.SelectSingleNode("chat");
            if (chatNode != null)
            {
                Chat = new ChatInfo(chatNode);
            }
        }

        /// <summary>
        /// Идентификатор отправленного системного сообщения
        /// </summary>
        public int? MessageId { get; set; }

        /// <summary>
        /// Объект мультидиалога
        /// </summary>
        public ChatInfo Chat { get; set; }
    }
}
