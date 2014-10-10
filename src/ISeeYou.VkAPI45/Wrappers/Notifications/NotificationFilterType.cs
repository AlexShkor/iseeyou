namespace VkAPIAsync.Wrappers.Notifications
{
    /// <summary>
    /// Фильтр уведомлений
    /// </summary>
    public class NotificationFilterType
    {
        public enum NotificationFilterTypeEnum
        {
            /// <summary>
            ///  Записи на стене пользователя
            /// </summary>
            Wall,

            /// <summary>
            /// Упоминания в записях на стене, в комментариях или в обсуждениях
            /// </summary>
            Mentions,

            /// <summary>
            /// Комментарии к записям на стене, фотографиям и видеозаписям 
            /// </summary>
            Comments,

            /// <summary>
            /// Отметки «Мне нравится»
            /// </summary>
            Likes,

            /// <summary>
            /// Скопированные у текущего пользователя записи на стене, фотографии и видеозаписи
            /// </summary>
            Reposts,

            /// <summary>
            /// Новые подписчики
            /// </summary>
            Followers,

            /// <summary>
            /// Принятые заявки в друзья
            /// </summary>
            Friends
        }

        public NotificationFilterType(NotificationFilterTypeEnum filterType)
        {
            Value = filterType.ToString("g").ToLower();
        }

        public string Value { get; private set; }
    }
}