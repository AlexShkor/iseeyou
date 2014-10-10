namespace VkAPIAsync.Wrappers.News
{
    /// <summary>
    /// Тип комментария для отписки
    /// </summary>
    public class CommentType
    {
        public enum CommentTypeEnum
        {
            /// <summary>
            /// Запись на стене пользователя или группы
            /// </summary>
            Post,

            /// <summary>
            /// Фотография
            /// </summary>
            Photo,

            /// <summary>
            ///  Видеозапись
            /// </summary>
            Video,

            /// <summary>
            /// Обсуждение
            /// </summary>
            Topic,

            /// <summary>
            /// Заметка
            /// </summary>
            Note
        }

        public CommentType(CommentTypeEnum e)
        {
            StringValue = e.ToString("G").ToLower();
        }

        public string StringValue { get; set; }
    }
}
