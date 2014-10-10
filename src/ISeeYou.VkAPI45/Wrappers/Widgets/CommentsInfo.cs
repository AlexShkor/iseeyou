#region Using

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Widgets
{
    /// <summary>
    /// Информация о комментарие
    /// </summary>
    public class CommentsInfo
    {
        public CommentsInfo(XmlNode node)
        {
            Count = node.Int("count");
            var postsNodes = node.SelectNodes("posts/post");
            if (postsNodes != null && postsNodes.Count > 0)
            {
                Posts = postsNodes.Cast<XmlNode>().Select(x => new WidgetPost(x)).ToList();
            }
        }

        /// <summary>
        /// Общее количество комментариев первого уровня к странице
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Список комментариев первого уровня
        /// </summary>
        public List<WidgetPost> Posts { get; set; }
    }
}