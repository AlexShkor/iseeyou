#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Photos;

#endregion

namespace VkAPIAsync.Wrappers.Widgets
{
    /// <summary>
    /// Страница
    /// </summary>
    public class Page
    {
        public Page(XmlNode node)
        {
            Id = node.Int("id");
            Title = node.String("title");
            Description = node.String("description");
            Photo = new Photo(node.SelectSingleNode("photo"));
            Url = node.String("url");
            LikesCount = node.Int("likes/count");
            CommentsCount = node.Int("comments/count");
            Date = node.DateTimeFromUnixTime("date");
            PageId = node.Int("page_id");
        }

        /// <summary>
        /// Идентификатор страницы в системе
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///  Заголовок страницы (берется из мета-тегов на странице или задается параметром pageTitle при инициализации)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Краткое описание страницы (берется из мета-тегов на странице или задается параметром pageDescription при инициализации)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///  Объект, содержащий фотографию-миниатюру страницы (берется из мета-тегов на странице или задается параметром pageImage при инициализации)
        /// </summary>
        public Photo Photo { get; set; }

        /// <summary>
        /// Абсолютный адрес страницы
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Количество лайков
        /// </summary>
        public int? LikesCount { get; set; }

        /// <summary>
        /// Количество комментариев
        /// </summary>
        public int? CommentsCount { get; set; }

        /// <summary>
        ///  Дата первого обращения к виджетам на странице
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///  Внутренний идентификатор страницы в приложении/на сайте (в случае, если при инициализации виджетов использовался параметр page_id)
        /// </summary>
        public int? PageId { get; set; }
    }
}