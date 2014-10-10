namespace VkAPIAsync.Wrappers.Users
{
    public class FriendsResultOrder
    {
        public enum FriendsResultOrderEnum
        {
            /// <summary>
            /// Сортировать по имени (работает только при передаче параметра fields)
            /// </summary>
            ByName,
            /// <summary>
            /// Сортировать по рейтингу
            /// </summary>
            ByRating,
            /// <summary>
            /// Возвращает друзей в случайном порядке. 
            /// </summary>
            Random,
            /// <summary>
            /// Возвращает выше тех друзей, у которых установлены мобильные приложения. 
            /// </summary>
            Mobile
        }

        public FriendsResultOrder(FriendsResultOrderEnum order)
        {
            switch (order)
            {
                case FriendsResultOrderEnum.ByName:
                    Value = "name";
                    break;
                case FriendsResultOrderEnum.ByRating:
                    Value = "hints";
                    break;
                case FriendsResultOrderEnum.Random:
                    Value = "random";
                    break;
                case FriendsResultOrderEnum.Mobile:
                    Value = "mobile";
                    break;
            }
        }

        public string Value { get; private set; }
    }

    public class RandomUsersResultOrder
    {
        public enum RandomUsersResultOrderEnum
        {
            /// <summary>
            /// Сортировать в случайном порядке
            /// </summary>
            Random,
            /// <summary>
            /// Сортировать по рейтингу
            /// </summary>
            ByRating
        }

        public RandomUsersResultOrder(RandomUsersResultOrderEnum order)
        {
            switch (order)
            {
                case RandomUsersResultOrderEnum.Random:
                    Value = "random";
                    break;
                case RandomUsersResultOrderEnum.ByRating:
                    Value = "hints";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}