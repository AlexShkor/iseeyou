namespace VkAPIAsync.Wrappers.Board
{
    /// <summary>
    /// Порядок сортировки тем
    /// </summary>
    public class TopicsSortOrder
    {
        public enum TopicsSortOrderEnum
        {
            /// <summary>
            ///  По убыванию даты обновления
            /// </summary>
            UpdateDateDesc = 1,
            /// <summary>
            /// По убыванию даты создания
            /// </summary>
            CreateDateDesc = 2,
            /// <summary>
            ///  По возрастанию даты обновления
            /// </summary>
            UpdateDateAsc = -1,
            /// <summary>
            ///  По возрастанию даты создания
            /// </summary>
            CreateDateAsc = -2
        }

        public TopicsSortOrder(TopicsSortOrderEnum order)
        {
            Value = (int) order;
        }

        public int Value { get; private set; }
    }
}