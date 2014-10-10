#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Audios;
using VkAPIAsync.Wrappers.Common.AttachmentTypes;
using VkAPIAsync.Wrappers.Docs;
using VkAPIAsync.Wrappers.Notes;
using VkAPIAsync.Wrappers.Pages;
using VkAPIAsync.Wrappers.Photos;
using VkAPIAsync.Wrappers.Videos;
using VkAPIAsync.Wrappers.Wall;

#endregion

namespace VkAPIAsync.Wrappers.Common.Factories
{
    /// <summary>
    /// Класс для парсинга вложений
    /// </summary>
    public static class Attachments
    {
        /// <summary>
        /// Извлекает информацию о вложении
        /// </summary>
        /// <param name="type">Тип вложения</param>
        /// <param name="node">Элемент DOM, который сожержит информацию в XML-представлении</param>
        public static AttachmentData GetAttachment(WallAttachmentType type, XmlNode node)
        {
            switch (type.Value)
            {
                case "app":
                {
                    var a = new AttachmentApplication
                    {
                        Id = node.Int("id"),
                        Name = node.String("name"),
                        Photo130 = node.String("photo_130"),
                        Photo604 = node.String("photo_604")
                    };
                    return a;
                }
                case "audio":
                {
                    var a = new AttachmentEntity<Audio> {Entity = new Audio(node)};
                    return a;
                }
                case "graffiti":
                {
                    return new AttachmentGraffiti
                    {
                        Id = node.Int("id"),
                        OwnerId = node.Int("owner_id"),
                        Photo200 = node.String("photo_200"),
                        Photo586 = node.String("photo_586")
                    };
                }
                case "note":
                {
                    return new AttachmentEntity<Note> {Entity = new Note(node)};
                }
                case "photo":
                {
                    return new AttachmentEntity<Photo> {Entity = new Photo(node)};
                }
                case "posted_photo":
                {
                    return new AttachmentPhoto
                    {
                        Id = node.Int("id"),
                        OwnerId = node.Int("owner_id"),
                        Photo130 = node.String("photo_130"),
                        Photo604 = node.String("photo_604")
                    };
                }
                case "poll":
                {
                   return new AttachmentPoll
                    {
                        Id = node.Int("id"),
                        Question = node.String("question")
                    };
                }
                case "video":
                {
                    return new AttachmentEntity<Video> {Entity = new Video(node)};
                }
                case "link":
                {
                    return new AttachmentLink
                    {
                        Url = node.String("url"),
                        Title = node.String("title"),
                        Description = node.String("description"),
                        ImageSource = node.String("image_src"),
                        PreviewPage = node.String("preview_page")
                    };
                }
                case "doc":
                {
                    return new AttachmentEntity<Document> {Entity = new Document(node)};
                }
                case "page":
                {
                    return new AttachmentEntity<Page> { Entity = new Page(node) };
                }
                case "album":
                {
                    return new AttachmentAlbum()
                    {
                        Id = node.Int("id"),
                        OwnerId = node.Int("owner_id"),
                        Size = node.Int("size"),
                        Thumb = new Photo(node.SelectSingleNode("thumb")),
                        Created = node.DateTimeFromUnixTime("date_created"),
                        Updated = node.DateTimeFromUnixTime("updated"),
                        Description = node.String("description"),
                        Title = node.String("title")
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// Извлекает информацию о вложении
        /// </summary>
        /// <param name="type">Тип вложения</param>
        /// <param name="node">Элемент DOM, который сожержит информацию в XML-представлении</param>
        public static AttachmentData GetAttachment(MessageAttachmentType type, XmlNode node)
        {
            switch (type.Value)
            {
                case "video":
                {
                    return new AttachmentEntity<Video> { Entity = new Video(node) };
                }
                case "photo":
                {
                    return new AttachmentEntity<Photo> { Entity = new Photo(node) };
                }
                case "audio":
                {
                    return new AttachmentEntity<Audio> { Entity = new Audio(node) };
                }
                case "doc":
                {
                    return new AttachmentEntity<Document> { Entity = new Document(node) };
                }
                case "wall":
                {
                    return new AttachmentEntity<WallEntity> { Entity = new WallEntity(node) };
                }
                case "wall_reply":
                {
                    return new AttachmentEntity<Comment> { Entity = new Comment(node) };
                }
            }
            return null;
        }
    }
}