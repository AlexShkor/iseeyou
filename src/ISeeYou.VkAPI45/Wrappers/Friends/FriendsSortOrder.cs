namespace VkAPIAsync.Wrappers.Friends
{
    public class FriendsSortOrder
    {
        public enum FriendsSortOrderEnum
        {
            ByDate = 0,
            ByMutualFriendsCount = 1
        }

        public FriendsSortOrder(FriendsSortOrderEnum order)
        {
            Value = (int) order;
        }

        public int Value { get; private set; }
    }
}