namespace VkAPIAsync.Wrappers.Wall
{
    /// <summary>
    /// Причина жалобы
    /// </summary>
    public class ReportReason
    {
        public enum ReportReasonEnum
        {
            /// <summary>
            /// Спам
            /// </summary>
            Spam = 0,
            /// <summary>
            /// Детская порнография
            /// </summary>
            Pornography = 1,
            /// <summary>
            /// Экстремизм
            /// </summary>
            Extremism = 2,
            /// <summary>
            /// Насилие
            /// </summary>
            Violence = 3,
            /// <summary>
            /// Пропаганда наркотиков
            /// </summary>
            DrugPropaganda = 4,
            /// <summary>
            /// Материал для взрослых
            /// </summary>
            AdultMaterial = 5,
            /// <summary>
            /// Оскорбление
            /// </summary>
            Affront = 6
        }

        public ReportReason(ReportReasonEnum e)
        {
            Value = (int)e;
        }

        public int Value { get; private set; }
    }
}
