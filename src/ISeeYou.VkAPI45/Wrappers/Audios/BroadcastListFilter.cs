namespace VkAPIAsync.Wrappers.Audios
{
    /// <summary>
    /// Фильтр списка вещаний
    /// </summary>
    public class BroadcastListFilter
    {
        public enum BroadcastListFilterEnum
        {
            /// <summary>
            /// Только друзья
            /// </summary>
            Friends,

            /// <summary>
            /// Только группы
            /// </summary>
            Groups,

            /// <summary>
            /// Все
            /// </summary>
            All
        }

        public BroadcastListFilter(BroadcastListFilterEnum e)
        {
            StringValue = e.ToString("g");
        }

        public string StringValue { get; set; }
    }
}
