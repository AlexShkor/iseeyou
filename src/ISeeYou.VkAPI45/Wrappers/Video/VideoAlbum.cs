using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

namespace VkAPIAsync.Wrappers.Videos
{
    /// <summary>
    /// Видеоальбом
    /// </summary>
    public class VideoAlbum : BaseEntity
    {
        public VideoAlbum(XmlNode node)
        {
            Id = node.Int("album_id");
            OwnerId = node.Int("owner_id");
            Title = node.String("title");

            Count = node.Int("count");
            Photo160 = node.String("photo_160");
            Photo320 = node.String("photo_320");
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Количество видеозаписей в альбоме
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Обложка альбома шириной в 160px
        /// </summary>
        public string Photo160 { get; set; }

        // <summary>
        /// Обложка альбома шириной в 320px
        /// </summary>
        public string Photo320 { get; set; }
    }
}
