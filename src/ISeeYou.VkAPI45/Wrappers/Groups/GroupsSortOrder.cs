namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Порядок сортировки групп
    /// </summary>
    public class GroupsSortOrder
    {
        public enum GroupsSortOrderEnum
        {
            /// <summary>
            /// По количеству пользователей
            /// </summary>
            ByUserCount = 0,
            /// <summary>
            ///  По скорости роста
            /// </summary>
            BySpeedOfGrowth = 1,
            /// <summary>
            ///  По отношению дневной посещаемости ко количеству пользователей
            /// </summary>
            ByTrafficUsersRelation = 2,
            /// <summary>
            /// По отношению количества лайков к количеству пользователей
            /// </summary>
            ByLikesUsersRelation = 3,
            /// <summary>
            ///  По отношению количества комментариев к количеству пользователей
            /// </summary>
            ByCommentsUserRelation = 4,
            /// <summary>
            ///  По отношению количества записей в обсуждениях к количеству пользователей
            /// </summary>
            ByDiscussionsUsersRelations = 5
        }

        public GroupsSortOrder(GroupsSortOrderEnum order)
        {
            Value = (int) order;
        }

        public int Value { get; private set; }
    }
}