#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Информация о лайках
    /// </summary>
    public class LikesInfo
    {
        /// <summary>
        /// Может ли текущий пользователь поставить отметку «Мне нравится»
        /// </summary>
        public bool? CanLike;

        /// <summary>
        /// Может ли текущий пользователь сделать репост записи
        /// </summary>
        public bool? CanPublish;

        /// <summary>
        /// Число пользователей, которым понравилась запись
        /// </summary>
        public int? Count;

        /// <summary>
        /// Наличие отметки «Мне нравится» от текущего пользователя
        /// </summary>
        public int? UserLikes;

        public LikesInfo(XmlNode node)
        {
            if (node == null) return;

            CanLike = node.Bool("can_like");
            CanPublish = node.Bool("can_publish");
            Count = node.Int("count");
            UserLikes = node.Int("user_likes");
        }
    }
}