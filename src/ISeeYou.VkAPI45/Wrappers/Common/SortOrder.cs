namespace VkAPIAsync.Wrappers.Common
{
    public class SortOrder
    {
        /// <summary>
        ///     Порядок сортировки
        /// </summary>
        public enum SortOrderEnum
        {
            /// <summary>
            ///     В порядке возрастания
            /// </summary>
            Asc = 1,

            /// <summary>
            ///     В порядке убывания
            /// </summary>
            Desc = 0
        }

        public SortOrder(SortOrderEnum order)
        {
            Value = (int) order;
            switch (order)
            {
                case SortOrderEnum.Asc:
                    StringValue = "asc";
                    break;
                case SortOrderEnum.Desc:
                    StringValue = "desc";
                    break;
            }
        }

        public int Value { get; private set; }
        public string StringValue { get; private set; }
    }
}