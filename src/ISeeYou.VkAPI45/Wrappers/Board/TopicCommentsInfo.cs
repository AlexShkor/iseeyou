#region Using

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Polls;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Board
{
    /// <summary>
    /// Информация о комментариях темы
    /// </summary>
    public class TopicCommentsInfo
    {
        public TopicCommentsInfo(XmlNode node)
        {
            Comments = new ListCount<Comment>(node.Int("count").Value,
                                                   node.SelectNodes("items/*")
                                                       .Cast<XmlNode>()
                                                       .Select(x => new Comment(x))
                                                       .ToList());
            var poll = node.SelectSingleNode("poll");
            if (poll != null)
            {
                Poll = new Poll(poll);
            }

            var profiles = node.SelectNodes("profiles/user");
            if (profiles != null)
            {
                Profiles = profiles.Cast<XmlNode>().Select(x => new User(x)).ToList();
            }
        }

        /// <summary>
        /// Описания каждого сообщения 
        /// </summary>
        public ListCount<Comment> Comments { get; set; }

        /// <summary>
        /// Если к теме был прикреплен опрос, возвращается поле poll
        /// </summary>
        public Poll Poll { get; set; }

        /// <summary>
        /// В случае передачи параметра extended равным 1, поле profiles содержит массив объектов user с информацией о данных пользователей, являющихся авторами сообщений.
        /// </summary>
        public List<User> Profiles { get; set; }
    }
}