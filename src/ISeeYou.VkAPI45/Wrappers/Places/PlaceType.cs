#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Тип места
    /// </summary>
    public class PlaceType
    {
        public PlaceType(XmlNode node)
        {
            Id = node.Int("id");
            Title = node.String("title");
            IconUrl = node.String("icon");
        }

        /// <summary>
        /// Идентификатор типа
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Название типа
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Иконка типа
        /// </summary>
        public string IconUrl { get; set; }
    }
}