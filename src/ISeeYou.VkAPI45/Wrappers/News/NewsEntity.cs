#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Notes;
using VkAPIAsync.Wrappers.Photos;
using VkAPIAsync.Wrappers.Users;
using VkAPIAsync.Wrappers.Wall;

#endregion

namespace VkAPIAsync.Wrappers.News
{
    /// <summary>
    /// Новость
    /// </summary>
    public class NewsEntity : BaseNewsEntity
    {
        /// <summary>
        /// Друзья
        /// </summary>
        public ListCount<int> Friends { get; set; }

        /// <summary>
        /// Заметки
        /// </summary>
        public ListCount<Note> Notes { get; set; }

        /// <summary>
        /// Метки на фотографиях
        /// </summary>
        public ListCount<Photo> PhotoTags { get; set; }

        /// <summary>
        /// Фотографии
        /// </summary>
        public ListCount<Photo> Photos { get; set; }

        /// <summary>
        /// Массив, содержащий историю репостов для записи. Возвращается только в том случае, если запись является репостом. Каждый из объектов массива, в свою очередь, является объектом-записью стандартного формата
        /// </summary>
        public List<WallEntity> CopyHistory { get; set; }

        public NewsEntity(XmlNode node) : base(node)
        {
            var typeEnum = node.Enum("type", typeof(NewsType.NewsTypeEnum));
            if (typeEnum != null)
            {
                Type = new NewsType((NewsType.NewsTypeEnum)typeEnum);
            }

            SourceId = node.Int("source_id");
            PostId = node.Int("post_id");
            CanEdit = node.Bool("can_edit");
            CanDelete = node.Bool("can_delete");

            var history = node.SelectSingleNode("copy_history");
            if (history != null && history.ChildNodes.Count > 0)
            {
                CopyHistory = history.ChildNodes.Cast<XmlNode>().Where(Node => Node.NodeType == XmlNodeType.Element).Select(x => new WallEntity(x)).ToList();
            }

            if (Type.Value == "photo" | Type.Value == "wall_photo")
            {
                var photoNodes =
                        node.SelectSingleNode("photos/items")
                            .ChildNodes.Cast<XmlNode>()
                            .Where(Node => Node.NodeType == XmlNodeType.Element);
                if (photoNodes.Any())
                {
                    Photos = new ListCount<Photo>(node.Int("photos/count").Value, photoNodes.Select(x => new Photo(x)).ToList());
                }
            }
            switch (Type.Value)
            {
                case "photo_tag":
                    var photoTagNodes =
                        node.SelectSingleNode("photo_tags/items")
                            .ChildNodes.Cast<XmlNode>()
                            .Where(Node => Node.NodeType == XmlNodeType.Element);
                    if (photoTagNodes.Any())
                    {
                        PhotoTags = new ListCount<Photo>(node.Int("photo_tags/count").Value, photoTagNodes.Select(x => new Photo(x)).ToList());
                    }
                    break;
                case "note":
                    var notesNodes =
                        node.SelectSingleNode("notes/items")
                            .ChildNodes.Cast<XmlNode>()
                            .Where(Node => Node.NodeType == XmlNodeType.Element);
                    if (notesNodes.Any())
                    {
                        Notes = new ListCount<Note>(node.Int("notes/count").Value, notesNodes.Select(x => new Note(x)).ToList());
                    }
                    break;
                case "friend":
                    var friendsNodes =
                        node.SelectSingleNode("friends/items")
                            .ChildNodes.Cast<XmlNode>()
                            .Where(Node => Node.NodeType == XmlNodeType.Element && Node.LocalName == "friend");
                    if (friendsNodes.Any())
                    {
                        Friends = new ListCount<int>(node.Int("friends/count").Value, friendsNodes.Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList());
                    }
                    break;
            }
        }

        /// <summary>
        /// Тип списка новости
        /// </summary>
        public NewsType Type { get; set; }

        /// <summary>
        ///  Идентификатор источника новости (положительный — новость пользователя, отрицательный — новость группы)
        /// </summary>
        public int? SourceId { get; set; }

        /// <summary>
        ///  Находится в записях со стен и содержит идентификатор записи на стене владельца
        /// </summary>
        public int? PostId { get; set; }

        /// <summary>
        /// Содержит true, если текущий пользователь может редактировать запись
        /// </summary>
        public bool? CanEdit { get; set; }

        /// <summary>
        /// Возвращается, если пользователь может удалить новость, всегда содержит true
        /// </summary>
        public bool? CanDelete { get; set; }
    }

    /// <summary>
    /// Базовая сущность новости
    /// </summary>
    public class BaseNewsEntity : BaseEntity
    {
        public BaseNewsEntity(XmlNode node)
        {
            PostType = node.String("post_type");
            CopyOwnerID = node.Int("copy_owner_id");
            CopyPostID = node.Int("copy_post_id");
            CopyPostDate = node.DateTimeFromUnixTime("copy_post_date");
            Date = node.DateTimeFromUnixTime("date");
            Text = node.String("text");
            FromId = node.Int("from_id");
            
            CommentsInfo = new CommentsInfo(node.SelectSingleNode("comments"));
            LikesInfo = new LikesInfo(node.SelectSingleNode("likes"));
            RepostsInfo = new RepostsInfo(node.SelectSingleNode("reposts"));
            Geo = new GeoInfo(node.SelectSingleNode("geo"));
            
            var attachments = node.SelectSingleNode("attachments");
            if (attachments != null)
            {
                Attachments = new List<WallAttachment>();
                foreach (
                    var attachmentNode in
                        attachments.ChildNodes.Cast<XmlNode>().Where(Node => Node.NodeType == XmlNodeType.Element))
                    Attachments.Add(new WallAttachment(attachmentNode));
            }

            var comments = node.SelectNodes("comments/list/*");
            if (comments.Count > 0)
            {
                Comments = comments.Cast<XmlNode>().Select(x => new Comment(x)).ToList();
            }
        }

        /// <summary>
        /// Находится в записях со стен, если сообщение является копией сообщения с чужой стены, и содержит дату скопированного сообщения
        /// </summary>
        public DateTime? CopyPostDate { get; set; }

        /// <summary>
        /// Время публикации новости
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///  Находится в записях со стен и содержит текст записи
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///  Находится в записях со стен, в которых имеется информация о местоположении, содержит поля
        /// </summary>
        public GeoInfo Geo { get; set; }

        /// <summary>
        /// Находится в записях со стен и содержит информацию о комментариях к записи
        /// </summary>
        public CommentsInfo CommentsInfo { get; set; }

        /// <summary>
        /// Список комментариев к записи в количестве, указанном в методе News.GetComments()
        /// </summary>
        public List<Comment> Comments { get; set; }

        /// <summary>
        /// Находится в записях со стен и содержит информацию о числе людей, которым понравилась данная запись
        /// </summary>
        public LikesInfo LikesInfo { get; set; }

        /// <summary>
        /// Находится в записях со стен и содержит информацию о числе людей, которые скопировали данную запись на свою страницу
        /// </summary>
        public RepostsInfo RepostsInfo { get; set; }

        /// <summary>
        /// Находится в записях со стен и содержит массив объектов, которые прикреплены к текущей новости (фотография, ссылка и т.п.)
        /// </summary>
        public List<WallAttachment> Attachments { get; set; }

        /// <summary>
        /// Находится в записях со стен, содержит тип новости (post или copy)
        /// </summary>
        public string PostType { get; set; }

        /// <summary>
        ///  Находится в записях со стен, если сообщение является копией сообщения с чужой стены, и содержит идентификатор владельца стены, у которого было скопировано сообщение
        /// </summary>
        public int? CopyOwnerID { get; set; }

        /// <summary>
        /// Находится в записях со стен, если сообщение является копией сообщения с чужой стены, и содержит идентификатор скопированного сообщения на стене его владельца
        /// </summary>
        public int? CopyPostID { get; set; }

        /// <summary>
        /// Идентификатор автора записи
        /// </summary>
        /// <remarks>Для метода News.Search</remarks>
        public int? FromId { get; set; }
    }
}