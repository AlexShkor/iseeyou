namespace VkAPIAsync.Wrappers.Likes
{
    public class LikeType
    {
        public enum LikeTypeEnum
        {
            /// <summary>
            /// Пост
            /// </summary>
            Post,

            /// <summary>
            /// Комментарий
            /// </summary>
            Comment,

            /// <summary>
            /// Фото
            /// </summary>
            Photo,

            /// <summary>
            /// Аудио
            /// </summary>
            Audio,

            /// <summary>
            /// Видео
            /// </summary>
            Video,

            /// <summary>
            /// Запись
            /// </summary>
            Note,

            /// <summary>
            /// Комментарий к фотографии
            /// </summary>
            PhotoComment,

            /// <summary>
            ///  Комментарий к видеозаписи
            /// </summary>
            VideoComment,

            /// <summary>
            ///  Комментарий в обсуждении
            /// </summary>
            TopicComment,

            /// <summary>
            /// Страница сайта
            /// </summary>
            SitePage
        }

        public LikeType(LikeTypeEnum type)
        {
            switch (type)
            {
                case LikeTypeEnum.Post:
                    Value = "post";
                    break;
                case LikeTypeEnum.Comment:
                    Value = "comment";
                    break;
                case LikeTypeEnum.Photo:
                    Value = "photo";
                    break;
                case LikeTypeEnum.Audio:
                    Value = "audio";
                    break;
                case LikeTypeEnum.Video:
                    Value = "video";
                    break;
                case LikeTypeEnum.Note:
                    Value = "note";
                    break;
                case LikeTypeEnum.PhotoComment:
                    Value = "photo_comment";
                    break;
                case LikeTypeEnum.TopicComment:
                    Value = "topic_comment";
                    break;
                case LikeTypeEnum.VideoComment:
                    Value = "video_comment";
                    break;
                case LikeTypeEnum.SitePage:
                    Value = "sitepage";
                    break;
            }
        }

        public string Value { get; private set; }
    }
}