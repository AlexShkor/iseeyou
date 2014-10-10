#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Videos
{
    /// <summary>
    /// Отметка на видео
    /// </summary>
    public class VideoTag
    {
        public VideoTag(XmlNode node)
        {
            TagId = node.Int("tag_id");
            Uid = node.Int("user_id");
            PlacerId = node.Int("placer_int");
            TaggedName = node.String("tagged_name");
            Viewed = node.Bool("viewed");
            Date = node.DateTimeFromUnixTime("date");
        }

        /// <summary>
        /// Идентификатор отметки
        /// </summary>
        public int? TagId { get; set; }

        /// <summary>
        ///  Идентификатор пользователя, которому соответствует отметка
        /// </summary>
        public int? Uid { get; set; }

        /// <summary>
        /// Идентификатор пользователя, сделавшего отметку
        /// </summary>
        public int? PlacerId { get; set; }

        /// <summary>
        /// Название отметки
        /// </summary>
        public string TaggedName { get; set; }

        /// <summary>
        /// Статус отметки (1 — подтвержденная, 0 — неподтвержденная)
        /// </summary>
        public bool? Viewed { get; set; }

        /// <summary>
        /// Дата добавления отметки
        /// </summary>
        public DateTime? Date { get; set; }
    }
}