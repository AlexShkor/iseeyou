using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Users;

namespace VkAPIAsync.Wrappers.Polls
{
    /// <summary>
    /// Список пользователей, которые ответили на опрос
    /// </summary>
    public class AnswerUsers
    {
        public AnswerUsers(XmlNode node)
        {
            AnswerId = node.Int("answer_id");
            var usersNodes = node.SelectNodes("users/items/*");
            if (usersNodes != null && usersNodes.Count > 0)
            {
                Users = new ListCount<User>(node.Int("count").Value, usersNodes.Cast<XmlNode>().Select(x => new User(x)).ToList());
            }
        }

        /// <summary>
        /// Идентификатор ответа
        /// </summary>
        public int? AnswerId { get; set; }

        /// <summary>
        /// Пользователи
        /// </summary>
        public ListCount<User> Users { get; set; }
    }
}
