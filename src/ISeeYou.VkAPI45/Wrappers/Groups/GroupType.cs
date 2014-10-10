namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Тип сообщества
    /// </summary>
    public class GroupType
    {
        public enum GroupTypeEnum
        {
            /// <summary>
            /// Группа
            /// </summary>
            Group,
            /// <summary>
            /// Публичная страница
            /// </summary>
            Page,
            /// <summary>
            /// Мероприятие
            /// </summary>
            Event
        }

        public GroupType(GroupTypeEnum type)
        {
            StringValue = type.ToString("G").ToLower();
        }

        public string StringValue { get; private set; }
    }
}