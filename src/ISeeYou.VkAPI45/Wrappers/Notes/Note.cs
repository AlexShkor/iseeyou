#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Photos;

#endregion

namespace VkAPIAsync.Wrappers.Notes
{
    /// <summary>
    /// Заметка
    /// </summary>
    public class Note : BaseEntity
    {
        public Note(XmlNode node)
        {
            Id = node.Int("id");
            OwnerId = node.Int("user_id");
            Title = node.String("title");
            Text = node.String("text");
            Date = node.DateTimeFromUnixTime("date");
            CommentsCount = node.Int("comments");
            ReadCommentsCount = node.Int("read_comments");
            Url = node.String("view_url");
        }

        /// <summary>
        /// Заголовок заметки
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст заметки
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания заметки 
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Количество комментариев
        /// </summary>
        public int? CommentsCount { get; set; }

        /// <summary>
        /// Количество прочитанных комментариев (только при запросе информации о заметке текущего пользователя
        /// </summary>
        public int? ReadCommentsCount { get; set; }

        /// <summary>
        /// Адрес страницы для отображения заметки.
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// Расширенная заметка
    /// </summary>
    public class NoteExtended : Note
    {
        /// <summary>
        /// Уровень доступа к комментированию заметки
        /// </summary>
        public AccessPrivacy.AccessPrivacyEnum CommentPrivacy;

        /// <summary>
        /// Уровень доступа к заметке
        /// </summary>
        public AccessPrivacy.AccessPrivacyEnum Privacy;

        public NoteExtended(XmlNode node) : base(node)
        {
            TextWiki = node.String("text_wiki");
            var privacyEnum = node.Enum("privacy", typeof(AccessPrivacy.AccessPrivacyEnum));
            if (privacyEnum != null)
            {
                Privacy = (AccessPrivacy.AccessPrivacyEnum)privacyEnum;
            }
            var commentPrivacyEnum = node.Enum("comment_privacy", typeof(AccessPrivacy.AccessPrivacyEnum));
            if (privacyEnum != null)
            {
                CommentPrivacy = (AccessPrivacy.AccessPrivacyEnum)commentPrivacyEnum;
            }
            CanComment = node.Bool("can_comment");
        }

        /// <summary>
        /// Вики-представление заметки
        /// </summary>
        public string TextWiki { get; set; }

        /// <summary>
        /// Может ли текущий пользователь комментировать заметку
        /// </summary>
        public bool? CanComment { get; set; }
    }
}