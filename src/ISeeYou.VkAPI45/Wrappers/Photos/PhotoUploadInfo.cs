#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Photos
{
    /// <summary>
    /// Информация о выгрузке фото
    /// </summary>
    public class PhotoUploadInfo
    {
        /// <summary>
        /// Идентификатор альбома, в который будет загружена фотография
        /// </summary>
        public int? AlbumId;

        /// <summary>
        /// Адрес для загрузки фотографий
        /// </summary>
        public string Url;

        /// <summary>
        /// Идентификатор пользователя, от чьего имени будет загружено фото.
        /// </summary>
        public int? UserId;

        public PhotoUploadInfo(XmlNode node)
        {
            AlbumId = node.Int("album_id");
            Url = node.String("upload_url");
            UserId = node.Int("user_id");
        }
    }
}