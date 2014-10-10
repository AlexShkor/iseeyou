#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Photos
{
    /// <summary>
    /// Альбом
    /// </summary>
    public class PhotoAlbum : BaseEntity
    {
        /// <summary>
        /// Дата создания альбома в формате unixtime (не приходит для системных альбомов)
        /// </summary>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Дата последнего обновления альбома в формате unixtime (не приходит для системных альбомов)
        /// </summary>
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Описание альбома (не приходит для системных альбомов)
        /// </summary>
        public string Description{ get; set; }

        /// <summary>
        ///  Настройки приватности для альбома в формате настроек приватности; (не приходит для системных альбомов)
        /// </summary>
        public Privacy PrivacyView{ get; set; }
        
        /// <summary>
        /// Настройки приватности для альбома в формате настроек приватности; (не приходит для системных альбомов)
        /// </summary>
        public Privacy PrivacyComment { get; set; }

        /// <summary>
        ///  Количество фотографий в альбоме
        /// </summary>
        public int? Size{ get; set; }

        /// <summary>
        ///  Идентификатор фотографии, которая является обложкой
        /// </summary>
        public int? ThumbnailId{ get; set; }

        /// <summary>
        /// Ссылка на изображение обложки альбома (если был указан параметр need_covers).
        /// </summary>
        public string ThumbnailSource{ get; set; }

        /// <summary>
        ///  Название альбома
        /// </summary>
        public string Title{ get; set; }

        /// <summary>
        /// true, если текущий пользователь может загружать фотографии в альбом (при запросе информации об альбомах сообщества)
        /// </summary>
        public bool? CanUpload { get; set; }

        public PhotoAlbum(XmlNode node)
        {
            if (node == null)
                return;

            Id = node.Int("id");
            ThumbnailId = node.Int("thumb_id");
            OwnerId = node.Int("owner_id");
            Title = node.String("title");
            Description = node.String("description");
            DateCreated = node.DateTimeFromUnixTime("created");
            DateUpdated = node.DateTimeFromUnixTime("updated");
            Size = node.Int("size");
            CanUpload = node.Bool("can_upload");
            PrivacyView = new Privacy(node.SelectSingleNode("privacy_view"));
            PrivacyComment = new Privacy(node.SelectSingleNode("privacy_comment"));
            ThumbnailSource = node.String("thumb_source");
        }

        public override string ToString()
        {
            return Title;
        }
    }
}