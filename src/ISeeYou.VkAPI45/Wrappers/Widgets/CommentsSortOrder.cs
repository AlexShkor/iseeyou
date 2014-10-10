namespace VkAPIAsync.Wrappers.Widgets
{
    /// <summary>
    /// Порядок сортировки комментариев
    /// </summary>
    public class CommentsSortOrder
    {
        public enum CommentsSortOrderEnum
        {
            /// <summary>
            /// По дате
            /// </summary>
            Date,
            /// <summary>
            /// По количеству лайков
            /// </summary>
            Likes,
            /// <summary>
            /// По дате последнего комментария
            /// </summary>
            LastComment
        }

        public CommentsSortOrder(CommentsSortOrderEnum order)
        {
            switch (order)
            {
                case CommentsSortOrderEnum.Date:
                    Value = "date";
                    break;
                case CommentsSortOrderEnum.Likes:
                    Value = "likes";
                    break;
                case CommentsSortOrderEnum.LastComment:
                    Value = "last_comment";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}