#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Информация о комментариях
    /// </summary>
    public class CommentsInfo
    {
        /// <summary>
        /// Может ли текущий пользователь комментировать
        /// </summary>
        public bool? CanComment;

        /// <summary>
        /// Кличество комментариев
        /// </summary>
        public int? Count;

        public CommentsInfo(XmlNode node)
        {
            if (node == null) return;

            CanComment = node.Bool("can_comment");
            Count = node.Int("count");
        }
    }
}