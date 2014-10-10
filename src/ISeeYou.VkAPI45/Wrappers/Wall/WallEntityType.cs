namespace VkAPIAsync.Wrappers.Wall
{
    /// <summary>
    /// Тип записи
    /// </summary>
    public class WallEntityType
    {
        
        public enum WallEntityTypeEnum
        {
            /// <summary>
            /// Пост
            /// </summary>
            Post,
            /// <summary>
            /// Копия поста
            /// </summary>
            Copy,
            /// <summary>
            /// Ответ
            /// </summary>
            Reply,
            /// <summary>
            /// Отложенная запись
            /// </summary>
            Postpone,
            /// <summary>
            /// Предложенная запись
            /// </summary>
            Suggest
        }

        public WallEntityType(WallEntityTypeEnum type)
        {
            StringValue = type.ToString("G").ToLower();
        }

        public string StringValue { get; private set; }
    }
}