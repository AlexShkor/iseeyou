#region Using

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Users;
using VkAPIAsync.Wrappers.Wall;

#endregion

namespace VkAPIAsync.Wrappers.Widgets
{
    /// <summary>
    /// Комментарий к странице, оставленный через Виджет комментариев
    /// </summary>
    public class WidgetPost : WallEntity
    {
        public WidgetPost(XmlNode node) : base(node)
        {
            var nodes = node.SelectNodes("comments/replies");
            if (nodes != null && nodes.Count > 0)
            {
                Replies = nodes.Cast<XmlNode>().Select(x => new Comment(x)).ToList();
            }

            var userNode = node.SelectSingleNode("user");
            if (userNode != null)
            {
                User = new User(userNode);
            }
        }

        /// <summary>
        /// Список комментариев второго уровня
        /// </summary>
        public List<Comment> Replies { get; set; }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        public User User { get; set; }
    }
}