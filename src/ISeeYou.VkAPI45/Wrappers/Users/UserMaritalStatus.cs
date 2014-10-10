namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    ///     Семейное положение
    /// </summary>
    public class UserMaritalStatus
    {
        public enum UserMaritalStatusEnum
        {
            /// <summary>
            /// Не указано
            /// </summary>
            NA = 0,

            /// <summary>
            ///  Не женат
            /// </summary>
            Single = 1,
            /// <summary>
            /// Встречается
            /// </summary>
            Meets = 2,
            /// <summary>
            /// Помолвлен
            /// </summary>
            Engaged = 3,
            /// <summary>
            /// Женат
            /// </summary>
            Maried = 4,
            /// <summary>
            /// В активном поиске
            /// </summary>
            InSearch = 5,
            /// <summary>
            /// Всё сложно
            /// </summary>
            Complicated = 6,
            /// <summary>
            /// Влюблён
            /// </summary>
            InLove = 7
        }

        public UserMaritalStatus(UserMaritalStatusEnum sex)
        {
            Value = (int) sex;
        }

        public int Value { get; private set; }
    }
}