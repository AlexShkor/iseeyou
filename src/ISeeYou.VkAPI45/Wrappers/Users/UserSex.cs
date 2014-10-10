namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    ///     Пол пользователя
    /// </summary>
    public class UserSex
    {
        public enum UserSexEnum
        {
            Male = 2,
            Female = 1,
            Unknown = 0
        }

        public UserSex(UserSexEnum sex)
        {
            Value = (int) sex;
        }

        public int Value { get; private set; }
    }
}