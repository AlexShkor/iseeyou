namespace VkAPIAsync.Wrappers.Common.AttachmentTypes
{
    /// <summary>
    /// Вложенная ссылка
    /// </summary>
    public class AttachmentLink : AttachmentData
    {
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
        /// Ссылка на изображение
        /// </summary>
        public string ImageSource { get; set; }

        /// <summary>
        /// Идентификатр wiki страницы с контентом для предпросмотра содержимого страницы, который может быть получен используя метод pages.get. Идентификатор возвращается в формате "owner_id_page_id"
        /// </summary>
        public string PreviewPage { get; set; }
    }
}