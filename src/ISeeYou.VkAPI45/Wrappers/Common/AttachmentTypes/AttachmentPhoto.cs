namespace VkAPIAsync.Wrappers.Common.AttachmentTypes
{
    /// <summary>
    /// Напрямую загруженая фотография
    /// </summary>
    public class AttachmentPhoto : AttachmentData
    {
        /// <summary>
        /// Идентификатор фотографии
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Адрес изображения для предпросмотра
        /// </summary>
        public string Photo130 { get; set; }

        /// <summary>
        /// Адрес полноразмерного изображения
        /// </summary>
        public string Photo604 { get; set; }
    }
}