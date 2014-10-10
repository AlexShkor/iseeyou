namespace VkAPIAsync.Wrappers.Common.AttachmentTypes
{
    /// <summary>
    /// Вложенный опрос
    /// </summary>
    public class AttachmentPoll : AttachmentData
    {
        /// <summary>
        /// Идентификатор опроса для получения информации о нем через метод polls.getById
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///  Вопрос, заданный в голосовании
        /// </summary>
        public string Question { get; set; }
    }
}