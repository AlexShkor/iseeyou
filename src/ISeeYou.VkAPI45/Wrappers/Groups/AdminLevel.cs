namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Полномочия текущего пользователя
    /// </summary>
    public class AdminLevel
    {
        public enum AdminLevelEnum
        {
            /// <summary>
            /// Модератор
            /// </summary>
            Moderator = 1,
            /// <summary>
            /// Редактор
            /// </summary>
            Editor = 2,
            /// <summary>
            /// Администратор
            /// </summary>
            Admin = 3
        }

        public AdminLevel(AdminLevelEnum order)
        {
            Value = (int) order;
        }

        public int Value { get; private set; }
    }
}