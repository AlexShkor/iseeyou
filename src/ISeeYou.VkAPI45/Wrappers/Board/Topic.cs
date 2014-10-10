#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Board
{
    /// <summary>
    /// Тема
    /// </summary>
    public class Topic
    {
        public Topic(XmlNode node)
        {
            Id = node.Int("id");
            Title = node.String("title");
            Created = node.DateTimeFromUnixTime("created");
            CreatedBy = node.Int("created_by");
            Updated = node.DateTimeFromUnixTime("updated");
            UpdatedBy = node.Int("updated_by");
            IsClosed = node.Bool("is_closed");
            IsFixed = node.Bool("is_fixed");
            CommentsCount = node.Int("comments");
            FirstComment = node.Int("first_comment");
            LastComment = node.Int("last_comment");
        }

        /// <summary>
        /// Идентификатор темы
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        ///  Идентификатор пользователя, создавшего тему
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Дата последнего сообщения
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        ///  Идентификатор пользователя, оставившего последнее сообщение
        /// </summary>
        public int? UpdatedBy { get; set; }

        /// <summary>
        /// true, если тема является закрытой
        /// </summary>
        public bool? IsClosed { get; set; }

        /// <summary>
        /// true, если тема является закрепленной
        /// </summary>
        public bool? IsFixed { get; set; }

        /// <summary>
        ///  Число сообщений в теме
        /// </summary>
        public int? CommentsCount { get; set; }

        /// <summary>
        ///  (только если в поле preview указан флаг 1) — текст первого сообщения
        /// </summary>
        public int? FirstComment { get; set; }

        /// <summary>
        /// (только если в поле preview указан флаг 2) — текст последнего сообщения
        /// </summary>
        public int? LastComment { get; set; }
    }
}