#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Тема
    /// </summary>
    public class Topic : BaseEntity
    {
        /// <summary>
        /// Количество комментариев
        /// </summary>
        public int? CommentsCount;

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime?  Created;

        /// <summary>
        /// Идентификатор пользователя, который создал тему
        /// </summary>
        public int? CreatedBy;

        /// <summary>
        /// true, если тема закрыта
        /// </summary>
        public bool? IsClosed;

        /// <summary>
        /// true, если тема закреплена
        /// </summary>
        public bool? IsFixed;

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title;

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime?  Updated;

        /// <summary>
        /// Идентификатор пользователя, который обновил тему
        /// </summary>
        public int? UpdatedBy;

        public Topic(XmlNode node)
        {
            Id = node.Int("id");
            OwnerId = node.Int("owner_id");
            Title = node.String("title");
            Created = node.DateTimeFromUnixTime("created");
            CreatedBy = node.Int("created_by");
            Updated = node.DateTimeFromUnixTime("updated");
            UpdatedBy = node.Int("updated_by");
            IsClosed = node.Bool("is_closed");
            IsFixed = node.Bool("is_fixed");
            CommentsCount = node.Int("comments");
        }
    }
}