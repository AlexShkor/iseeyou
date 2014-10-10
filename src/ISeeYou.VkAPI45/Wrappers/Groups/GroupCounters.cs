#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Счетчики группы
    /// </summary>
    public class GroupCounters
    {
        #region Constructor

        public GroupCounters(XmlNode node)
        {
            if (node == null)
                return;

            Albums = node.Int(GroupCountersFields.Albums);
            Videos = node.Int(GroupCountersFields.Videos);
            Audios = node.Int(GroupCountersFields.Audios);
            Photos = node.Int(GroupCountersFields.Photos);
            Topics = node.Int(GroupCountersFields.Topics);
            Docs = node.Int(GroupCountersFields.Docs);
        }

        #endregion //Constructor

        #region Properties

        /// <summary>
        ///     Количество фотоальбомов
        /// </summary>
        public int? Albums { get; set; }

        /// <summary>
        ///     Количество видеозаписей
        /// </summary>
        public int? Videos { get; set; }

        /// <summary>
        ///     Количество аудиозаписей
        /// </summary>
        public int? Audios { get; set; }

        /// <summary>
        ///     Количество фотографий
        /// </summary>
        public int? Photos { get; set; }

        /// <summary>
        /// Количество обсуждений
        /// </summary>
        public int? Topics { get; set; }

        /// <summary>
        /// Количество документов
        /// </summary>
        public int? Docs { get; set; }

        #endregion //Properties
    }

    public static class GroupCountersFields
    {
        public static readonly string Albums = "albums";
        public static readonly string Videos = "videos";
        public static readonly string Audios = "audios";
        public static readonly string Photos = "photos";
        public static readonly string Topics = "topics";
        public static readonly string Docs = "docs";
    }
}