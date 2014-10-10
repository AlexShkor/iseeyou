using System;

namespace VkAPIAsync.Wrappers.Common.AttachmentTypes
{
    /// <summary>
    /// Тип вложения для поста
    /// </summary>
    public class WallAttachmentType
    {
        public enum WallAttachmentTypeEnum
        {
            Video,
            Audio,
            App,
            Graffiti,
            Photo,
            PostedPhoto,
            Doc,
            Page,
            Album,
            PhotosList,
            Note,
            Link,
            Poll
        }

        public WallAttachmentType(WallAttachmentTypeEnum type)
        {
            switch (type)
            {
                case WallAttachmentTypeEnum.Video:
                    Value = "video";
                    break;
                case WallAttachmentTypeEnum.Audio:
                    Value = "audio";
                    break;
                case WallAttachmentTypeEnum.App:
                    Value = "app";
                    break;
                case WallAttachmentTypeEnum.Graffiti:
                    Value = "graffiti";
                    break;
                case WallAttachmentTypeEnum.Photo:
                    Value = "photo";
                    break;
                case WallAttachmentTypeEnum.PostedPhoto:
                    Value = "posted_photo";
                    break;
                case WallAttachmentTypeEnum.Note:
                    Value = "note";
                    break;
                case WallAttachmentTypeEnum.Link:
                    Value = "link";
                    break;
                case WallAttachmentTypeEnum.Poll:
                    Value = "poll";
                    break;
                case WallAttachmentTypeEnum.Album:
                    Value = "album";
                    break;
                case WallAttachmentTypeEnum.Doc:
                    Value = "doc";
                    break;
                case WallAttachmentTypeEnum.PhotosList:
                    Value = "photos_list";
                    break;
                case WallAttachmentTypeEnum.Page:
                    Value = "page";
                    break;
            }
        }

        public string Value { get; private set; }
    }

    /// <summary>
    /// Тип вложения для сообщения
    /// </summary>
    public class MessageAttachmentType
    {
        public enum MessageAttachmentTypeEnum
        {
            Video,
            Audio,
            Photo,
            Doc,
            Wall,
            WallReply
        }

        public MessageAttachmentType(MessageAttachmentTypeEnum type)
        {
            switch (type)
            {
                case MessageAttachmentTypeEnum.Video:
                    Value = "video";
                    break;
                case MessageAttachmentTypeEnum.Audio:
                    Value = "audio";
                    break;
                case MessageAttachmentTypeEnum.Photo:
                    Value = "photo";
                    break;
                case MessageAttachmentTypeEnum.Doc:
                    Value = "doc";
                    break;
                case MessageAttachmentTypeEnum.Wall:
                    Value = "wall";
                    break;
                case MessageAttachmentTypeEnum.WallReply:
                    Value = "wall_reply";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        public string Value { get; private set; }
    }
}