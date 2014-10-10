namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    /// Тип жалобы на пользователя
    /// </summary>
    public class UserReportType
    {
        public enum UserReportTypeEnum
        {
            /// <summary>
            /// Порнография 
            /// </summary>
            Porn,
            /// <summary>
            /// Рассылка спама 
            /// </summary>
            Spam,
            /// <summary>
            /// Оскорбительное поведение 
            /// </summary>
            Insult,
            /// <summary>
            /// Рекламная страница, засоряющая поиск
            /// </summary>
            Advertisment
        }

        public UserReportType(UserReportTypeEnum e)
        {
            StringValue = e.ToString("G").ToLower();
        }

        public string StringValue { get; private set; }
    }
}
