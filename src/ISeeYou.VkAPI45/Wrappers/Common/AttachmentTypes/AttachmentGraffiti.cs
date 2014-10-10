namespace VkAPIAsync.Wrappers.Common.AttachmentTypes
{
    /// <summary>
    /// Вложенное граффити
    /// </summary>
    internal class AttachmentGraffiti : AttachmentData
    {
        /// <summary>
        /// Идентификатор граффити
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Идентификатор владельца
        /// </summary>
        public int? OwnerId { get; set; }

        /// <summary>
        /// Адрес изображения для предпросмотра
        /// </summary>
        public string Photo200 { get; set; }

        /// <summary>
        /// Адрес полноразмерного изображения
        /// </summary>
        public string Photo586 { get; set; }
    }
}