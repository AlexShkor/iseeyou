#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Fave
{
    /// <summary>
    /// Информация о ссылке
    /// </summary>
    public class LinkInfo
    {
        public LinkInfo(XmlNode node)
        {
            Url = node.String("url");
            Title = node.String("title");
            Description = node.String("description");
            ImageSource = node.String("image_50");
            ImageMiddle = node.String("image_100");
        }

        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ссылка на изображение 50x50
        /// </summary>
        public string ImageSource { get; set; }

        /// <summary>
        /// Ссылка на изображение 100x100
        /// </summary>
        public string ImageMiddle { get; set; }
    }
}