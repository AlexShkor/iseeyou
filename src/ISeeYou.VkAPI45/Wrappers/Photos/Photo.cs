#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Photos
{
    /// <summary>
    /// Изображение
    /// </summary>
    public class Photo : BaseEntity
    {
        /// <summary>
        /// Идентификатор альбома, в котором находится фотография
        /// </summary>
        public int? AlbumId{ get; set; }

        /// <summary>
        /// Дата добавления
        /// </summary>
        public DateTime? DateCreated{ get; set; }

        /// <summary>
        /// Текст описания фотографии
        /// </summary>
        public string Text{ get; set; }

        /// <summary>
        /// Количество комментариев к фотографии
        /// </summary>
        public int? CommentsCount{ get; set; }

        /// <summary>
        /// Количество отметок Мне нравится и информация о том, поставил ли лайк текущий пользователь
        /// </summary>
        public LikesInfo LikesInfo{ get; set; }

        /// <summary>
        /// Количество отметок на фотографии
        /// </summary>
        public int? TagsCount{ get; set; }

        /// <summary>
        /// Может ли текущий пользователь комментировать фото
        /// </summary>
        public bool? CanComment {get;set;}

        /// <summary>
        /// Ширина оригинала фотографии в пикселах
        /// </summary>
        public int? Width{get;set;}

        /// <summary>
        /// Высота оригинала фотографии в пикселах
        /// </summary>
        public int? Height{get;set;}

        /// <summary>
        /// Url копии фотографии с максимальным размером 75x75px. 
        /// </summary>
        public string Photo75{get;set;}

        /// <summary>
        /// Url копии фотографии с максимальным размером 130x130px. 
        /// </summary>
        public string Photo130{get;set;}

        /// <summary>
        /// Url копии фотографии с максимальным размером 604x604px. 
        /// </summary>
        public string Photo604{get;set;}

        /// <summary>
        /// Url копии фотографии с максимальным размером 807x807px. 
        /// </summary>
        public string Photo807{get;set;}

        /// <summary>
        /// Url копии фотографии с максимальным размером 1280x1280px. 
        /// </summary>
        public string Photo1280{get;set;}

        /// <summary>
        /// Url копии фотографии с максимальным размером 2560x2560px. 
        /// </summary>
        public string Photo2560{get;set;}

        /// <summary>
        /// Размеры
        /// </summary>
        public List<PhotoSize> Sizes { get; set; }

        public string AccessKey { get; set; }

        public Photo(XmlNode node)
        {
            Id = node.Int("id");
            AlbumId = node.Int("album_id");
            OwnerId = node.Int("owner_id");
            CanComment = node.Bool("can_comment");
           
            Photo75 = node.String("photo_75");
            Photo130 = node.String("photo_130");
            Photo604 = node.String("photo_604");
            Photo807 = node.String("photo_807");
            Photo1280 = node.String("photo_1280");
            Photo2560 = node.String("photo_2560");

            var sizes = node.SelectSingleNode("sizes");
            if (sizes != null)
            {
                Sizes = sizes.ChildNodes.OfType<XmlNode>().Select(x => new PhotoSize(x)).ToList();
            }

            Text = node.String("text");
            DateCreated = node.DateTimeFromUnixTime("created");
            CommentsCount = node.Int("comments/count");
            TagsCount = node.Int("tags/count");

            var likesNode = node.SelectSingleNode("likes");
            if (likesNode != null)
            {
                LikesInfo = new LikesInfo(likesNode);
            }
            Width = node.Int("width");
            Height = node.Int("height");

            AccessKey = node.String("access_key");
        }

        public override string ToString()
        {
            return Text;
        }
    }

    /// <summary>
    ///  Фотография, на которой есть непросмотренные отметки
    /// </summary>
    public class PhotoTagInfo : Photo
    {
        /// <summary>
        /// Идентификатор пользователя, сделавшего отметку
        /// </summary>
        public int? PlacerId;

        /// <summary>
        /// Дата создания отметки
        /// </summary>
        public DateTime? TagCreated;

        /// <summary>
        /// Идентификатор отметки
        /// </summary>
        public int? TagId;

        public PhotoTagInfo(XmlNode node) : base(node)
        {
            TagId = node.Int("tag_id");
            PlacerId = node.Int("placer_id");
            TagCreated = node.DateTimeFromUnixTime("tag_created");
        }
    }
}