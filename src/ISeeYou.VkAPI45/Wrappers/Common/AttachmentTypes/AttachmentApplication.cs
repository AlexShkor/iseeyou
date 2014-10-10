namespace VkAPIAsync.Wrappers.Common.AttachmentTypes
{
    /// <summary>
    /// Контент приложения
    /// </summary>
    internal class AttachmentApplication : AttachmentData
    {
        /// <summary>
        /// Идентификатор приложения
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Название приложения
        /// </summary>
        public string Name { get; set; }

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