using System;
using VkAPIAsync.Wrappers.Photos;

namespace VkAPIAsync.Wrappers.Common.AttachmentTypes
{
    /// <summary>
    /// Вложенный фотоальбом
    /// </summary>
    public class AttachmentAlbum : AttachmentData
    {
        /// <summary>
        /// Идентификатор альбома
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Обложка альбома
        /// </summary>
        public Photo Thumb { get; set; }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Название альбома
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание альбома
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Количество фотографий в альбоме
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// Дата создания альбома
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Дата последнего обновления альбома
        /// </summary>
        public DateTime? Updated { get; set; }
    }
}