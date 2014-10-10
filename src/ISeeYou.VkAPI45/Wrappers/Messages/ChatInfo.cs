#region Using

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// Информация о мультидиалоге
    /// </summary>
    public class ChatInfo
    {
        public ChatInfo(XmlNode node)
        {
            ChatId = node.Int("chat_id");
            Title = node.String("title");
            Type = node.String("type");
            
            var userNodes = node.SelectNodes("users/*");
            if (userNodes != null && userNodes.Count > 0)
            {
                if (userNodes[0].ChildNodes.Count == 0)
                {
                    Users = userNodes.Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => new User(x.Value)).ToList();
                }
                else
                {
                    Users = userNodes.Cast<XmlNode>().Select(x => new User(x)).ToList();
                }
            }
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int? ChatId { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Список профилей (если указано поле fields)
        /// </summary>
        public List<User> Users { get; set; }
    }
}