namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    ///     �������� ���������
    /// </summary>
    public class UserMaritalStatus
    {
        public enum UserMaritalStatusEnum
        {
            /// <summary>
            /// �� �������
            /// </summary>
            NA = 0,

            /// <summary>
            ///  �� �����
            /// </summary>
            Single = 1,
            /// <summary>
            /// �����������
            /// </summary>
            Meets = 2,
            /// <summary>
            /// ���������
            /// </summary>
            Engaged = 3,
            /// <summary>
            /// �����
            /// </summary>
            Maried = 4,
            /// <summary>
            /// � �������� ������
            /// </summary>
            InSearch = 5,
            /// <summary>
            /// �� ������
            /// </summary>
            Complicated = 6,
            /// <summary>
            /// ������
            /// </summary>
            InLove = 7
        }

        public UserMaritalStatus(UserMaritalStatusEnum sex)
        {
            Value = (int) sex;
        }

        public int Value { get; private set; }
    }
}