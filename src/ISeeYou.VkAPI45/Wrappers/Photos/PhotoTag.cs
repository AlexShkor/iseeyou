#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Photos
{
    /// <summary>
    /// Отметка на фотографии
    /// </summary>
    public class PhotoTag
    {
        /// <summary>
        /// Дата добавления отметки
        /// </summary>
        public DateTime? Date;

        /// <summary>
        /// Илентификатор
        /// </summary>
        public int? Id;

        /// <summary>
        ///  Идентификатор пользователя, сделавшего отметку
        /// </summary>
        public int? PlacerId;

        /// <summary>
        /// Название отметки
        /// </summary>
        public string TaggedName;

        /// <summary>
        /// Идентификатор пользователя, которому соответствует отметкам
        /// </summary>
        public int? UserId;

        /// <summary>
        /// Статус отметки (true — подтвержденная, false — неподтвержденная)
        /// </summary>
        public bool? Viewed;

        /// <summary>
        /// Координата Х первой точки
        /// </summary>
        public float? X;

        /// <summary>
        /// Координата Х второй точки
        /// </summary>
        public float? X2;

        /// <summary>
        /// Координата Y первой точки
        /// </summary>
        public float? Y;

        /// <summary>
        /// Координата Y второй точки
        /// </summary>
        public float? Y2;

        public PhotoTag(XmlNode node)
        {
            Id = node.Int("tag_id");
            UserId = node.Int("uid");
            PlacerId = node.Int("placer_id");
            Date = node.DateTimeFromUnixTime("date");
            X = node.Float("x");
            Y = node.Float("y");
            X2 = node.Float("x2");
            Y2 = node.Float("y2");
            Viewed = node.Bool("viewed");
        }
    }
}