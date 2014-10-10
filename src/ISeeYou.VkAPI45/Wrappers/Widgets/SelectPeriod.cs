namespace VkAPIAsync.Wrappers.Widgets
{
    /// <summary>
    /// Период выборки
    /// </summary>
    public class SelectPeriod
    {
        public enum SelectPeriodEnum
        {
            /// <summary>
            /// День
            /// </summary>
            Day,
            /// <summary>
            /// Неделя
            /// </summary>
            Week,
            /// <summary>
            /// Месяц
            /// </summary>
            Month,
            /// <summary>
            /// Все время
            /// </summary>
            AllTime
        }

        public SelectPeriod(SelectPeriodEnum period)
        {
            switch (period)
            {
                case SelectPeriodEnum.Day:
                    Value = "day";
                    break;
                case SelectPeriodEnum.Week:
                    Value = "week";
                    break;
                case SelectPeriodEnum.Month:
                    Value = "month";
                    break;
                case SelectPeriodEnum.AllTime:
                    Value = "alltime";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}