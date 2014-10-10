namespace VkAPIAsync.Wrappers.Users
{
    public class UsersSortOrder
    {
        public enum UsersSortOrderEnum
        {
            ByDate = 0,
            ByMutualFriendsCount = 1
        }

        public UsersSortOrder(UsersSortOrderEnum order)
        {
            Value = (int) order;
        }

        public int Value { get; private set; }
    }
}