#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Docs
{
    /// <summary>
    /// Документ
    /// </summary>
    public class Document : BaseEntity
    {
        public Document(XmlNode node)
        {
            Id = node.Int("id");
            OwnerId = node.Int("owner_id");
            Title = node.String("title");
            Size = node.Int("size");
            Extension = node.String("ext");
            Url = node.String("url");
            Photo100 = node.String("photo_100");
            Photo130 = node.String("photo_130");
            AccessKey = node.String("access_key");
        }

        /// <summary>
        /// Название документа
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Размер в байтах
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// Расширение документа
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Ссылка для загрузки
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Адрес изображения с размером 100x75px (если файл графический)
        /// </summary>
        public string Photo100 { get; set; }

        /// <summary>
        /// Адрес изображения с размером 130x100px (если файл графический)
        /// </summary>
        public string Photo130 { get; set; }

        public string AccessKey { get; set; }
    }
}