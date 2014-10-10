namespace VkAPIAsync.Wrappers.Photos
{
    public class AccessPrivacy
    {
        public enum AccessPrivacyEnum
        {
            /// <summary>
            /// Все пользователи
            /// </summary>
            All = 0,
            /// <summary>
            /// Только друзья
            /// </summary>
            OnlyFriends = 1,
            /// <summary>
            /// Друзья и друзья друзей
            /// </summary>
            FriendsOfFriends = 2,
            /// <summary>
            /// Только я
            /// </summary>
            OnlyMe = 3
        }

        public AccessPrivacy(AccessPrivacyEnum privacy)
        {
            Value = (int) privacy;
        }

        public int Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class AccessGroupPrivacy
    {
        public enum AccessGroupPrivacyEnum
        {
            /// <summary>
            /// Все пользователи
            /// </summary>
            All = 0,
            /// <summary>
            /// Только участники группы
            /// </summary>
            OnlyMembers = 1,
        }

        public AccessGroupPrivacy(AccessGroupPrivacyEnum privacy)
        {
            Value = (int) privacy;
        }

        public int Value { get; private set; }
    }

    public class AccessWikiPrivacy
    {
        public enum AccessWikiPrivacyEnum
        {
            All = 2,
            OnlyMembers = 1,
            AdminsOnly = 0
        }

        public AccessWikiPrivacy(AccessWikiPrivacyEnum privacy)
        {
            Value = (int) privacy;
        }

        public int Value { get; private set; }
    }
}