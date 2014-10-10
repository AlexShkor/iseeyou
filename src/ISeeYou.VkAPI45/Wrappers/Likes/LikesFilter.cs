namespace VkAPIAsync.Wrappers.Likes
{
    public class LikesFilter
    {
        /// <summary>
        ///     Указывает, следует ли вернуть всех пользователей, добавивших объект в список "Мне нравится" или только тех, которые рассказали о нем друзьям.
        /// </summary>
        public enum LikesFilterEnum
        {
            /// <summary>
            ///     Возвращать всех пользователей
            /// </summary>
            Likes,

            /// <summary>
            ///     Возвращать только пользователей, рассказавших об объекте друзьям
            /// </summary>
            Copies
        }

        public LikesFilter(LikesFilterEnum type)
        {
            switch (type)
            {
                case LikesFilterEnum.Likes:
                    Value = "likes";
                    break;
                case LikesFilterEnum.Copies:
                    Value = "copies";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}