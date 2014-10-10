using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Wall
{
    /// <summary>
    /// Результат выполнения репоста
    /// </summary>
    public class RepostSuccess
    {
        public RepostSuccess(XmlNode node)
        {
            if (node == null)
                return;

            Success = node.Bool("success");
            PostId = node.Int("post_id");
            RepostsCount = node.Int("reposts_count");
            LikesCount = node.Int("likes_count");
        }

        /// <summary>
        /// Репост прошел успешно
        /// </summary>
        public bool? Success { get; set; }

        /// <summary>
        /// Идентификатор созданной записи
        /// </summary>
        public int? PostId { get; set; }

        /// <summary>
        /// Количество репостов объекта с учетом осуществленного
        /// </summary>
        public int? RepostsCount { get; set; }

        /// <summary>
        ///  Число отметок «Мне нравится» у объекта
        /// </summary>
        public int? LikesCount { get; set; }
    }
}
