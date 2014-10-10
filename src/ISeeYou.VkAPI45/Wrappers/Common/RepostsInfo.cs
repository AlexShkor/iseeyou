#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Информация о репостах
    /// </summary>
    public class RepostsInfo
    {
        public RepostsInfo(XmlNode node)
        {
            if (node == null) return;

            Count = node.Int("count");
            UserReposted = node.Bool("user_reposted");
        }

        /// <summary>
        /// Число пользователей, скопировавших запись
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Наличие репоста от текущего пользователя
        /// </summary>
        public bool? UserReposted { get; set; }
    }
}