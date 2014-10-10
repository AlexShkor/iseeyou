#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Photos;
using VkAPIAsync.Wrappers.Users;
using VkAPIAsync.Wrappers.Videos;
using VkAPIAsync.Wrappers.Wall;

#endregion

namespace VkAPIAsync.Wrappers.Notifications
{
    /// <summary>
    /// Базовое уведомление
    /// </summary>
    public abstract class Notification
    {
        /// <summary>
        /// Дата уведомления
        /// </summary>
        public DateTime? Date;

        /// <summary>
        /// Ответ
        /// </summary>
        public ReplyInfo Reply;

        /// <summary>
        /// Тип уведомления
        /// </summary>
        public NotificationType Type;

        protected Notification(XmlNode node)
        {
            Type = (NotificationType) node.Enum(node.String("type"), typeof (NotificationType));
            Date = node.DateTimeFromUnixTime("date");
            Reply = new ReplyInfo(node.SelectSingleNode("reply"));
        }
    }

    /// <summary>
    /// Уведомление, связанное с каким-либо пользователем
    /// </summary>
    public class UserNotification : Notification
    {
        public List<User> Feedback;

        public UserNotification(XmlNode node) : base(node)
        {
            var nodes = node.SelectNodes("feedback");
            if (nodes != null && nodes.Count > 0)
                Feedback = nodes.Cast<XmlNode>().Select(x => new User(x)).ToList();
        }
    }

    /// <summary>
    /// Уведомление, связанное с постом
    /// </summary>
    public class PostNotification : Notification
    {
        public WallEntity Feedback;

        public PostNotification(XmlNode node)
            : base(node)
        {
            var post = node.SelectSingleNode("feedback");
            if (post != null)
                Feedback = new WallEntity(post);
        }
    }

    /// <summary>
    /// Уведомление, связанное с комментарием к посту
    /// </summary>
    public class PostCommentNotification : Notification
    {
        public Comment Feedback;
        public WallEntity Parent;

        public PostCommentNotification(XmlNode node)
            : base(node)
        {
            var post = node.SelectSingleNode("parent");
            if (post != null)
                Parent = new WallEntity(post);
            var comment = node.SelectSingleNode("feedback");
            if (comment != null)
                Feedback = new Comment(comment);
        }
    }

    /// <summary>
    /// Уведомление, связанное с комментарием к фотографии
    /// </summary>
    public class PhotoCommentNotification : Notification
    {
        public Comment Feedback;
        public Photo Parent;

        public PhotoCommentNotification(XmlNode node)
            : base(node)
        {
            var photo = node.SelectSingleNode("parent");
            if (photo != null)
                Parent = new Photo(photo);
            var comment = node.SelectSingleNode("feedback");
            if (comment != null)
                Feedback = new Comment(comment);
        }
    }

    /// <summary>
    /// Уведомление, связанное с комментарием к видеозаписи
    /// </summary>
    public class VideoCommentNotification : Notification
    {
        public Comment Feedback;
        public Video Parent;

        public VideoCommentNotification(XmlNode node)
            : base(node)
        {
            var video = node.SelectSingleNode("parent");
            if (video != null)
                Parent = new Video(video);
            var comment = node.SelectSingleNode("feedback");
            if (comment != null)
                Feedback = new Comment(comment);
        }
    }

    /// <summary>
    /// Уведомление, связанное с несколькими комментариями
    /// </summary>
    public class CommentsNotification : Notification
    {
        public Comment Feedback;
        public Comment Parent;

        public CommentsNotification(XmlNode node)
            : base(node)
        {
            var comment1 = node.SelectSingleNode("parent");
            if (comment1 != null)
                Parent = new Comment(comment1);
            var comment = node.SelectSingleNode("feedback");
            if (comment != null)
                Feedback = new Comment(comment);
        }
    }

    /// <summary>
    /// Уведомление, связанное с комментарием к теме
    /// </summary>
    public class TopicCommentNotification : Notification
    {
        public Comment Feedback;
        public Topic Parent;

        public TopicCommentNotification(XmlNode node)
            : base(node)
        {
            var topic = node.SelectSingleNode("parent");
            if (topic != null)
                Parent = new Topic(topic);
            var comment = node.SelectSingleNode("feedback");
            if (comment != null)
                Feedback = new Comment(comment);
        }
    }

    /// <summary>
    /// Уведомление, связанное с постом и с пользователем
    /// </summary>
    public class PostUserNotification : Notification
    {
        public User Feedback;
        public WallEntity Parent;

        public PostUserNotification(XmlNode node)
            : base(node)
        {
            var post = node.SelectSingleNode("parent");
            if (post != null)
                Parent = new WallEntity(post);
            var user = node.SelectSingleNode("feedback");
            if (user != null)
                Feedback = new User(user);
        }
    }

    /// <summary>
    /// Уведомление, связанное с комментарием и пользователем
    /// </summary>
    public class CommentUserNotification : Notification
    {
        public User Feedback;
        public Comment Parent;

        public CommentUserNotification(XmlNode node)
            : base(node)
        {
            var comment = node.SelectSingleNode("parent");
            if (comment != null)
                Parent = new Comment(comment);
            var user = node.SelectSingleNode("feedback");
            if (user != null)
                Feedback = new User(user);
        }
    }

    /// <summary>
    /// Уведомление, связанное с фотографией и пользователем
    /// </summary>
    public class PhotoUserNotification : Notification
    {
        public User Feedback;
        public Photo Parent;

        public PhotoUserNotification(XmlNode node)
            : base(node)
        {
            var photo = node.SelectSingleNode("parent");
            if (photo != null)
                Parent = new Photo(photo);
            var user = node.SelectSingleNode("feedback");
            if (user != null)
                Feedback = new User(user);
        }
    }

    /// <summary>
    /// Уведомление, связанное с видеозаписью и пользователем
    /// </summary>
    public class VideoUserNotification : Notification
    {
        public User Feedback;
        public Video Parent;

        public VideoUserNotification(XmlNode node)
            : base(node)
        {
            var video = node.SelectSingleNode("parent");
            if (video != null)
                Parent = new Video(video);
            var user = node.SelectSingleNode("feedback");
            if (user != null)
                Feedback = new User(user);
        }
    }

    /// <summary>
    /// Уведомление о репосте поста
    /// </summary>
    public class PostCopyNotification : Notification
    {
        public RepostsInfo Feedback;
        public WallEntity Parent;

        public PostCopyNotification(XmlNode node)
            : base(node)
        {
            var post = node.SelectSingleNode("parent");
            if (post != null)
                Parent = new WallEntity(post);
            var copy = node.SelectSingleNode("feedback");
            if (copy != null)
                Feedback = new RepostsInfo(copy);
        }
    }

    /// <summary>
    /// Уведомление о репосту фотографии
    /// </summary>
    public class PhotoCopyNotification : Notification
    {
        public RepostsInfo Feedback;
        public Photo Parent;

        public PhotoCopyNotification(XmlNode node)
            : base(node)
        {
            var post = node.SelectSingleNode("parent");
            if (post != null)
                Parent = new Photo(post);
            var copy = node.SelectSingleNode("feedback");
            if (copy != null)
                Feedback = new RepostsInfo(copy);
        }
    }
    
    /// <summary>
    /// Уведомление о репосте видеозаписи
    /// </summary>
    public class VideoCopyNotification : Notification
    {
        public RepostsInfo Feedback;
        public Video Parent;

        public VideoCopyNotification(XmlNode node)
            : base(node)
        {
            var post = node.SelectSingleNode("parent");
            if (post != null)
                Parent = new Video(post);
            var copy = node.SelectSingleNode("feedback");
            if (copy != null)
                Feedback = new RepostsInfo(copy);
        }
    }

    /// <summary>
    /// Тип уведомления
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// У пользователя появился один или несколько новых подписчиков.
        /// </summary>
        Follow,

        /// <summary>
        /// Заявка в друзья, отправленная пользователем, была принята.
        /// </summary>
        FriendAccepted,

        /// <summary>
        /// Была создана запись на чужой стене, содержащая упоминание пользователя.
        /// </summary>
        Mention,

        /// <summary>
        /// Был оставлен комментарий, содержащий упоминание пользователя.
        /// </summary>
        MentionComments,

        /// <summary>
        /// Была добавлена запись на стене пользователя.
        /// </summary>
        Wall,

        /// <summary>
        /// Был добавлен новый комментарий к записи, созданной пользователем.
        /// </summary>
        CommentPost,

        /// <summary>
        /// Был добавлен новый комментарий к фотографии пользователя.
        /// </summary>
        CommentPhoto,

        /// <summary>
        /// Был добавлен новый комментарий к видеозаписи пользователя.
        /// </summary>
        CommentVideo,

        /// <summary>
        /// Был добавлен новый ответ на комментарий пользователя.
        /// </summary>
        ReplyComment,

        /// <summary>
        /// Был добавлен новый ответ на комментарий пользователя к фотографии.
        /// </summary>
        ReplyCommentPhoto,

        /// <summary>
        /// Был добавлен новый ответ на комментарий пользователя к видеозаписи.
        /// </summary>
        ReplyCommentVideo,

        /// <summary>
        /// Был добавлен новый ответ пользователю в обсуждении.
        /// </summary>
        ReplyTopic,

        /// <summary>
        /// У записи пользователя появилась одна или несколько новых отметок «Мне нравится».
        /// </summary>
        LikePost,

        /// <summary>
        /// У комментария пользователя появилась одна или несколько новых отметок «Мне нравится».
        /// </summary>
        LikeComment,

        /// <summary>
        /// У фотографии пользователя появилась одна или несколько новых отметок «Мне нравится».
        /// </summary>
        LikePhoto,

        /// <summary>
        /// У видеозаписи пользователя появилась одна или несколько новых отметок «Мне нравится».
        /// </summary>
        LikeVideo,

        /// <summary>
        /// У комментария пользователя к видеозаписи появилась одна или несколько новых отметок «Мне нравится».
        /// </summary>
        LikeCommentVideo,

        /// <summary>
        /// У комментария пользователя к фотографии появилась одна или несколько новых отметок «Мне нравится».
        /// </summary>
        LikeCommentPhoto,

        /// <summary>
        /// У комментария пользователя в обсуждении появилась одна или несколько новых отметок «Мне нравится».
        /// </summary>
        LikeCommentTopic,

        /// <summary>
        /// Один или несколько пользователей скопировали запись пользователя
        /// </summary>
        CopyPost,

        /// <summary>
        /// Один или несколько пользователей скопировали фотографию пользователя.
        /// </summary>
        CopyPhoto,

        /// <summary>
        /// Один или несколько пользователей скопировали видеозапись пользователя.
        /// </summary>
        CopyVideo,

        /// <summary>
        /// Под видео был оставлен комментарий, содержащий упоминание пользователя.
        /// </summary>
        MentionCommentVideo,

        /// <summary>
        /// Под фотографией был оставлен комментарий, содержащий упоминание пользователя.
        /// </summary>
        MentionCommentPhoto
    }
}