namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Радису поиска мест
    /// </summary>
    public class SearchRadius
    {
        public enum SearchRadiusEnum
        {
            /// <summary>
            /// 300 метров
            /// </summary>
            M300 = 1,

            /// <summary>
            /// 2400 метров
            /// </summary>
            M2400 = 2,

            /// <summary>
            /// 18 километров
            /// </summary>
            Km18 = 3,

            /// <summary>
            /// 150 километров
            /// </summary>
            Km150 = 4
        }

        public SearchRadius(SearchRadiusEnum radius)
        {
            Value = (int) radius;
        }

        public int Value { get; set; }
    }
}