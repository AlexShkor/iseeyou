namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Причина бана пользователя
    /// </summary>
    public class BanReason
    {
        public enum BanReasonEnum
        {
            /// <summary>
            /// Другое
            /// </summary>
            Other = 0,
            /// <summary>
            /// Спам
            /// </summary>
            Spam = 1,
            /// <summary>
            /// Оскорбление участников
            /// </summary>
            Insulting = 2,
            /// <summary>
            /// Нецензурные выражения
            /// </summary>
            ObsceneLanguage = 3,
            /// <summary>
            /// Сообщения не по теме
            /// </summary>
            Offtop = 4
        }

        public BanReason(BanReasonEnum order)
        {
            Value = (int) order;
        }

        public int Value { get; private set; }
    }
}
