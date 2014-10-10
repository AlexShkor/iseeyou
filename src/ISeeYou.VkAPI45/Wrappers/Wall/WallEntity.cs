#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Wall
{
    /// <summary>
    /// Объект post, описывающий запись на стене пользователя или сообщества
    /// </summary>
    public class WallEntity
    {
        /// <summary>
        ///     Информация о вложениях записи
        /// </summary>
        public List<WallAttachment> Attachments { get; set; }

        /// <summary>
        ///     Текст записи
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///     Информация о комментариях к записи
        /// </summary>
        public CommentsInfo CommentsInfo { get; set; }

        /// <summary>
        /// Массив, содержащий историю репостов для записи. Возвращается только в том случае, если запись является репостом. Каждый из объектов массива, в свою очередь, является объектом-записью стандартного формата.
        /// </summary>
        public List<WallEntity> CopyHistory { get; set; }

        /// <summary>
        ///     Время публикации записи
        /// </summary>
        public DateTime?  Date{ get; set; }

        /// <summary>
        ///     Идентификатор автора записи.
        /// </summary>
        public int? FromUser{ get; set; }

        /// <summary>
        ///     Информация о местоположении
        /// </summary>
        public GeoInfo GeoInfo{ get; set; }

        /// <summary>
        ///     Идентификатор записи.
        /// </summary>
        public int? Id{ get; set; }

        /// <summary>
        ///     Информация о лайках к записи
        /// </summary>
        public LikesInfo LikesInfo{ get; set; }

        /// <summary>
        ///     Информация о способе размещения записи
        /// </summary>
        public PostSource PostSource{ get; set; }

        /// <summary>
        ///     Информация о репостах записи
        /// </summary>
        public RepostsInfo RepostsInfo{ get; set; }

        /// <summary>
        ///     Тип
        /// </summary>
        public WallEntityType Type { get; set; }

        /// <summary>
        ///     Идентификатор автора, если запись была опубликована от имени сообщества и подписана пользователем{ get; set; }
        /// </summary>
        public int? SignerId { get; set; }

        /// <summary>
        ///     Идентификатор владельца стены, на которой размещена запись.
        /// </summary>
        public int? ToUser { get; set; }

        /// <summary>
        ///     Только для друзей
        /// </summary>
        public bool? FriendsOnly { get; set; }

        /// <summary>
        ///     Идентификатор владельца записи, в ответ на которую была оставлена текущая.
        /// </summary>
        public int? ReplyOwnerId { get; set; }

        /// <summary>
        ///     Идентификатор записи, в ответ на которую была оставлена текущая.
        /// </summary>
        public int? ReplyId { get; set; }

        public WallEntity(XmlNode node)
        {
            Id = node.Int("id");
            Body = node.String("text");
            FromUser = node.Int("from_id");
            ToUser = node.Int("to_id");

            var postTypeEnum = node.Enum("post_type", typeof(WallEntityType.WallEntityTypeEnum));
            if (postTypeEnum != null)
                Type = new WallEntityType((WallEntityType.WallEntityTypeEnum)postTypeEnum);

            Date = node.DateTimeFromUnixTime("date");
            var attachmentsNodes = node.SelectNodes("attachments/attachment");
            if (attachmentsNodes != null && attachmentsNodes.Count > 0)
                Attachments = attachmentsNodes.Cast<XmlNode>().Select(x => new WallAttachment(x)).ToList();

            var historyNodes = node.SelectNodes("copy_history/*");
            if (historyNodes.Count > 0)
            {
                CopyHistory = historyNodes.Cast<XmlNode>().Select(x => new WallEntity(x)).ToList();
            }

            SignerId = node.Int("signer_id");
            GeoInfo = new GeoInfo(node.SelectSingleNode("geo"));
            PostSource = new PostSource(node.SelectSingleNode("post_source"));
            RepostsInfo = new RepostsInfo(node.SelectSingleNode("reposts"));
            LikesInfo = new LikesInfo(node.SelectSingleNode("likes"));
            CommentsInfo = new CommentsInfo(node.SelectSingleNode("comments"));
            FriendsOnly = node.Bool("friends_only");
            ReplyOwnerId = node.Int("reply_owner_id");
            ReplyId = node.Int("reply_post_id");
        }
    }
}