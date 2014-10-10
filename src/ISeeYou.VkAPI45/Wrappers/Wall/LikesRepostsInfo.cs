#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Wall
{
    /// <summary>
    /// Информация о лайках и репостах
    /// </summary>
    public class LikesRepostsInfo
    {
        /// <summary>
        /// Количество лайков
        /// </summary>
        public int? Likes;

        /// <summary>
        /// Количество репостов
        /// </summary>
        public int? Reposts;

        public LikesRepostsInfo(XmlNode node)
        {
            Likes = node.Int("likes");
            Reposts = node.Int("reposts");
        }
    }
}