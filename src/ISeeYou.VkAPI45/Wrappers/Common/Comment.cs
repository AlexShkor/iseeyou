#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Комментарий
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Список вложений
        /// </summary>
        public List<WallAttachment> Attachments;

        /// <summary>
        /// Дата написания
        /// </summary>
        public DateTime? Date;

        /// <summary>
        /// Идентификатор комментария
        /// </summary>
        public int? Id;

        /// <summary>
        /// Информация о лайках
        /// </summary>
        public LikesInfo Likes;

        /// <summary>
        /// Идентификатор комментария, ответом на который является текущий комментарий
        /// </summary>
        public int? ReplyToComment;

        /// <summary>
        /// Идентификатор пользователя, который написал комментарий, ответом на который является текущий комментарий
        /// </summary>
        public int? ReplyToUser;

        /// <summary>
        /// Текст
        /// </summary>
        public string Text;

        /// <summary>
        /// Идентификатор пользователя, который написал этот комментарий
        /// </summary>
        public int? UserId;

        public Comment(XmlNode node)
        {
            Id = node.Int("id");
            Date = node.DateTimeFromUnixTime("date");
            ReplyToComment = node.Int("reply_to_comment");
            ReplyToUser = node.Int("reply_to_user");
            Text = node.String("message");
            UserId = node.Int("from_id");
            var likes = node.SelectSingleNode("likes");
            if (likes != null)
                Likes = new LikesInfo(likes);
            var attachments = node.SelectNodes("attachments/attachment");
            if (attachments != null && attachments.Count > 0)
            {
                Attachments = attachments.Cast<XmlNode>().Select(x => new WallAttachment(x)).ToList();
            }
        }
    }
}