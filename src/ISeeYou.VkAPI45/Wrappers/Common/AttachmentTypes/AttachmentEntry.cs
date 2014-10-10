namespace VkAPIAsync.Wrappers.Common.AttachmentTypes
{
    /// <summary>
    /// Вложение с обьектом
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AttachmentEntity<T> : AttachmentData
    {
        /// <summary>
        /// Сущность
        /// </summary>
        public T Entity { get; set; }
    }
}