#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Audios
{
    /// <summary>
    /// Аудиозапись
    /// </summary>
    public class Audio : BaseEntity
    {
        public Audio(XmlNode node)
        {
            Id = node.Int("id");
            OwnerId = node.Int("owner_id");
            Duration = node.Int("duration");
            Artist = node.String("artist");
            Title = node.String("title");
            Url = node.String("url");
            LyricsId = node.Int("lyrics_id");
            GenreId = node.Int("genre_id");
            AlbumId = node.Int("album_id");
        }

        /// <summary>
        /// Длительность аудиозаписи в секундах
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Название композиции
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Ссылка на mp3
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Идентификатор текста аудиозаписи (если доступно)
        /// </summary>
        public int? LyricsId { get; set; }

        /// <summary>
        /// Идентификатор альбома, в котором находится аудиозапись (если присвоен)
        /// </summary>
        public int? AlbumId { get; set; }

        /// <summary>
        /// Идентификатор жанра
        /// </summary>
        public int? GenreId { get; set; }

        public override string ToString()
        {
            return Artist + " - " + Title;
        }

        public override bool Equals(object obj)
        {
            if (obj is Audio)
            {
                var a = obj as Audio;
                return Title == a.Title &&
                       Artist == a.Artist &&
                       Duration == a.Duration;
            }
            return false;
        }
    }
}