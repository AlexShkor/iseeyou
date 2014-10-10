namespace VkAPIAsync.Wrappers.Widgets
{
    /// <summary>
    /// Порядок сортировки страниц
    /// </summary>
    public class PagesSortOrder
    {
        public enum PageSortOrderEnum
        {
            /// <summary>
            /// По дате
            /// </summary>
            Date,
            /// <summary>
            /// По количеству комментариев
            /// </summary>
            Comments,
            /// <summary>
            /// По количеству лайков
            /// </summary>
            Likes,
            /// <summary>
            /// По количеству лайков друзей пользователя
            /// </summary>
            FriendLikes
        }

        public PagesSortOrder(PageSortOrderEnum order)
        {
            switch (order)
            {
                case PageSortOrderEnum.Date:
                    Value = "date";
                    break;
                case PageSortOrderEnum.Comments:
                    Value = "comments";
                    break;
                case PageSortOrderEnum.Likes:
                    Value = "likes";
                    break;
                case PageSortOrderEnum.FriendLikes:
                    Value = "friend_likes";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}