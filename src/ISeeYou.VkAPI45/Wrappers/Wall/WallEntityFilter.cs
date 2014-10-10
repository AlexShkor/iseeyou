namespace VkAPIAsync.Wrappers.Wall
{
    public class WallEntityFilter
    {
        /// <summary>
        ///     Определяет, какие типы сообщений на стене необходимо получить.
        /// </summary>
        public enum WallEntityFilterEnum
        {
            /// <summary>
            ///     Сообщения на стене от ее владельца
            /// </summary>
            Owner,

            /// <summary>
            ///     Сообщения на стене не от ее владельца
            /// </summary>
            Others,

            /// <summary>
            ///     Все сообщения на стене
            /// </summary>
            All,

            /// <summary>
            ///  Отложенные записи
            /// </summary>
            Postponed,

            /// <summary>
            ///      Предложенные записи на стене сообщества
            /// </summary>
            Suggested
        }

        public WallEntityFilter(WallEntityFilterEnum filter)
        {
            StringValue = filter.ToString("G").ToLower();
        }

        public string StringValue { get; private set; }
    }
}