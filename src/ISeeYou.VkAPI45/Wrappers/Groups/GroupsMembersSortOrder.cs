namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Порядок сортировки участников группы
    /// </summary>
    public class GroupsMembersSortOrder
    {
        public enum GroupsMembersSortOrderEnum
        {
            /// <summary>
            /// В порядке возрастания ID пользователя
            /// </summary>
            IdAsc = 1,
            /// <summary>
            /// В порядке убывания ID пользователя
            /// </summary>
            IdDesc = -1,
            /// <summary>
            /// В хронологическом порядке вступления пользователей в группу
            /// </summary>
            TimeAsc = 2,
            /// <summary>
            /// В антихронологическом порядке вступления пользователей в группу
            /// </summary>
            TimeDesc = -2
        }

        public GroupsMembersSortOrder(GroupsMembersSortOrderEnum order)
        {
            Value = (int) order;
            switch (order)
            {
                case GroupsMembersSortOrderEnum.IdAsc:
                    StringValue = "id_asc";
                    break;
                case GroupsMembersSortOrderEnum.IdDesc:
                    StringValue = "id_desc";
                    break;
                case GroupsMembersSortOrderEnum.TimeAsc:
                    StringValue = "time_asc";
                    break;
                case GroupsMembersSortOrderEnum.TimeDesc:
                    StringValue = "time_desc";
                    break;
            }
        }

        public int Value { get; private set; }
        public string StringValue { get; private set; }
    }
}