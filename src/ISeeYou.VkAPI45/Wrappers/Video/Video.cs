#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Photos;

#endregion

namespace VkAPIAsync.Wrappers.Videos
{
    /// <summary>
    /// Видеозапись
    /// </summary>
    public class Video : BaseEntity
    {
        public Video(XmlNode node)
        {
            Id = node.Int("id");
            OwnerId = node.Int("owner_id");
            Title = node.String("title");
            Description = node.String("description");

            var duration = node.Int("duration");
            if (duration.HasValue)
            {
                Duration = TimeSpan.FromSeconds(duration.Value);
            }

            Link = node.String("link");
            Photo130 = node.String("photo_130");
            Photo320 = node.String("photo_320");
            Photo640 = node.String("photo_640");
            Date = node.DateTimeFromUnixTime("date");
            PlayerLink = node.String("player");
            Comments = node.Int("comments");
            Views = node.Int("views");

            //Privacy
            var pViewNode = node.SelectSingleNode("privacy_view");
            if (pViewNode != null)
            {
                PrivacyView = new Privacy(pViewNode);
            }
            var pCommentNode = node.SelectSingleNode("privacy_comment");
            if (pCommentNode != null)
            {
                PrivacyComment = new Privacy(pCommentNode);
            }

            CanComment = node.Bool("can_comment");
            CanRepost = node.Bool("can_repost");
            Repeat = node.Bool("repeat");

            //Likes
            var likesNode = node.SelectSingleNode("likes");
            if (likesNode != null)
            {
                Likes = new LikesInfo(likesNode);
            }
        }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Длительность ролика в секундах
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Строка, состоящая из ключа video+vid
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// url изображения-обложки ролика с размером 130x98px
        /// </summary>
        public string Photo130 { get; set; }

        /// <summary>
        /// url изображения-обложки ролика с размером 320x240px
        /// </summary>
        public string Photo320 { get; set; }

        /// <summary>
        /// url изображения-обложки ролика с размером 640x480px
        /// </summary>
        public string Photo640 { get; set; }

        /// <summary>
        /// Дата добавления видеозаписи
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Количество просмотров видеозаписи
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// Количество комментариев к видеозаписи
        /// </summary>
        public int? Comments { get; set; }

        /// <summary>
        /// Адрес страницы с плеером, который можно использовать для воспроизведения ролика в браузере. Поддерживается flash и html5, плеер всегда масштабируется по размеру окна.
        /// </summary>
        public string PlayerLink { get; set; }

        /// <summary>
        /// Может ли текущий пользователь оставлять комментарии к ролику
        /// </summary>
        public bool? CanComment { get; set; }

        /// <summary>
        /// Может ли текущий пользователь скопировать ролик с помощью функции «Рассказать друзьям»
        /// </summary>
        public bool? CanRepost { get; set; }

        /// <summary>
        ///  Зацикливание воспроизведения видеозаписи
        /// </summary>
        public bool? Repeat { get; set; }

        /// <summary>
        /// Информация от отметках «Мне нравится»
        /// </summary>
        public LikesInfo Likes { get; set; }

        /// <summary>
        /// Настройки приватности просмотров
        /// </summary>
        public Privacy PrivacyView { get; set; }

        /// <summary>
        /// Настройки приватности комментирования
        /// </summary>
        public Privacy PrivacyComment { get; set; }
    }

    /// <summary>
    /// Видеозапись с отметкой
    /// </summary>
    public class TaggedVideo : Video
    {
        public TaggedVideo(XmlNode x) : base(x)
        {
            TagId = x.Int("tag_id");
            PlacerId = x.Int("placer_id");
            TagCreated = x.DateTimeFromUnixTime("tag_created");
        }

        /// <summary>
        /// Идентификатор пользователя, сделавшего отметку
        /// </summary>
        public int? PlacerId { get; set; }

        /// <summary>
        /// Дата создания отметки
        /// </summary>
        public DateTime? TagCreated { get; set; }

        /// <summary>
        /// Идентификатор отметки
        /// </summary>
        public int? TagId { get; set; }
    }
}