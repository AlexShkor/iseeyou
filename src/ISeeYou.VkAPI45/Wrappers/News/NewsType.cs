namespace VkAPIAsync.Wrappers.News
{
    /// <summary>
    /// Фильтр новостной ленты
    /// </summary>
    public class NewsType
    {
        public enum NewsTypeEnum
        {
            /// <summary>
            /// Новые записи со стен
            /// </summary>
            Post,

            /// <summary>
            /// Новые фотографии
            /// </summary>
            Photo,

            /// <summary>
            /// Новые отметки на фотографиях
            /// </summary>
            PhotoTag,

            /// <summary>
            /// Новые фотографии на стенах
            /// </summary>
            WallPhoto,

            /// <summary>
            /// Новые друзья
            /// </summary>
            Friend,

            /// <summary>
            /// Новые заметки
            /// </summary>
            Note
        }

        public NewsType(NewsTypeEnum type)
        {
            switch (type)
            {
                case NewsTypeEnum.Post:
                    Value = "post";
                    break;
                case NewsTypeEnum.Photo:
                    Value = "photo";
                    break;
                case NewsTypeEnum.PhotoTag:
                    Value = "photo_tag";
                    break;
                case NewsTypeEnum.WallPhoto:
                    Value = "wall_photo";
                    break;
                case NewsTypeEnum.Friend:
                    Value = "friend";
                    break;
                case NewsTypeEnum.Note:
                    Value = "note";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}