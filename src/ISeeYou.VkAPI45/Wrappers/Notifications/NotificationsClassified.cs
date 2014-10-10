#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Notifications
{
    /// <summary>
    /// Уведомления
    /// </summary>
    public class NotificationsClassified
    {
        private List<BaseGroup> _groupes;
        private List<Notification> _items;
        private List<BaseUser> _profiles;

        public NotificationsClassified(XmlNode x)
        {
            LastViewed = x.DateTimeFromUnixTime("last_viewed");
            NewFrom = x.String("news_from");
            NewOffset = x.Int("new_offset");

            //Notifications
            var items =
                x.SelectSingleNode("items")
                 .ChildNodes.Cast<XmlNode>()
                 .Where(item => item.NodeType == XmlNodeType.Element && item.LocalName == "item");
            var nodes = items as List<XmlNode> ?? items.ToList();
            if (nodes.Any())
            {
                foreach (var item in nodes)
                {
                    var type = (NotificationType) item.Enum(item.String("type"), typeof (NotificationType));
                    switch (type)
                    {
                        case NotificationType.Follow:
                            Items.Add(new UserNotification(item));
                            break;
                        case NotificationType.FriendAccepted:
                            Items.Add(new UserNotification(item));
                            break;
                        case NotificationType.Mention:
                            Items.Add(new PostNotification(item));
                            break;
                        case NotificationType.MentionComments:
                            Items.Add(new PostCommentNotification(item));
                            break;
                        case NotificationType.Wall:
                            Items.Add(new PostNotification(item));
                            break;
                        case NotificationType.CommentPost:
                            Items.Add(new PostCommentNotification(item));
                            break;
                        case NotificationType.CommentPhoto:
                            Items.Add(new PhotoCommentNotification(item));
                            break;
                        case NotificationType.CommentVideo:
                            Items.Add(new VideoCommentNotification(item));
                            break;
                        case NotificationType.ReplyComment:
                            Items.Add(new CommentsNotification(item));
                            break;
                        case NotificationType.ReplyTopic:
                            Items.Add(new TopicCommentNotification(item));
                            break;
                        case NotificationType.LikePost:
                            Items.Add(new PostUserNotification(item));
                            break;
                        case NotificationType.LikeComment:
                            Items.Add(new CommentUserNotification(item));
                            break;
                        case NotificationType.LikePhoto:
                            Items.Add(new PhotoUserNotification(item));
                            break;
                        case NotificationType.LikeVideo:
                            Items.Add(new VideoUserNotification(item));
                            break;
                        case NotificationType.CopyPost:
                            Items.Add(new PostCopyNotification(item));
                            break;
                        case NotificationType.CopyPhoto:
                            Items.Add(new PhotoCopyNotification(item));
                            break;
                        case NotificationType.CopyVideo:
                            Items.Add(new VideoCopyNotification(item));
                            break;
                        case NotificationType.ReplyCommentPhoto:
                            Items.Add(new CommentsNotification(item));
                            break;
                        case NotificationType.ReplyCommentVideo:
                            Items.Add(new CommentsNotification(item));
                            break;
                        case NotificationType.LikeCommentPhoto:
                            Items.Add(new CommentUserNotification(item));
                            break;
                        case NotificationType.LikeCommentVideo:
                            Items.Add(new CommentUserNotification(item));
                            break;
                        case NotificationType.LikeCommentTopic:
                            Items.Add(new CommentUserNotification(item));
                            break;
                        case NotificationType.MentionCommentPhoto:
                            Items.Add(new PhotoCommentNotification(item));
                            break;
                        case NotificationType.MentionCommentVideo:
                            Items.Add(new VideoCommentNotification(item));
                            break;
                    }
                }
            }

            //Users
            var profiles =
                x.SelectSingleNode("profiles")
                 .ChildNodes.Cast<XmlNode>()
                 .Where(item => item.NodeType == XmlNodeType.Element && item.LocalName == "user");
            nodes = profiles as List<XmlNode> ?? profiles.ToList();
            if (nodes.Any())
            {
                foreach (var profile in nodes)
                    Profiles.Add(new BaseUser(profile));
            }

            //Groups
            var groups =
                x.SelectSingleNode("groups")
                 .ChildNodes.Cast<XmlNode>()
                 .Where(item => item.NodeType == XmlNodeType.Element && item.LocalName == "group");
            nodes = groups as List<XmlNode> ?? groups.ToList();
            if (nodes.Any())
            {
                foreach (var group in nodes)
                    Groupes.Add(new BaseGroup(group));
            }
        }

        /// <summary>
        /// Список оповещений для текущего пользователя
        /// </summary>
        public List<Notification> Items
        {
            get { return _items ?? (_items = new List<Notification>()); }
            set { _items = value; }
        }

        /// <summary>
        /// Содержит информацию о пользователях, которые находятся в списке оповещений
        /// </summary>
        public List<BaseUser> Profiles
        {
            get { return _profiles ?? (_profiles = new List<BaseUser>()); }
            set { _profiles = value; }
        }

        /// <summary>
        /// Содержит информацию о сообществах, которые находятся в списке оповещений
        /// </summary>
        public List<BaseGroup> Groupes
        {
            get { return _groupes ?? (_groupes = new List<BaseGroup>()); }
            set { _groupes = value; }
        }

        /// <summary>
        /// Содержит время последнего просмотра пользователем раздела оповещений 
        /// </summary>
        public DateTime? LastViewed { get; set; }

        /// <summary>
        /// Новый строковый идентификатор отступа
        /// </summary>
        public string NewFrom { get; set; }

        /// <summary>
        /// Новое смещение
        /// </summary>
        public int? NewOffset { get; set; }
    }
}