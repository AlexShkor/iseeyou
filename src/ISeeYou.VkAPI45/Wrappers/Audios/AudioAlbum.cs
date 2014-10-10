#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Audios
{
    /// <summary>
    /// Аудиоальбом
    /// </summary>
    public class AudioAlbum : BaseEntity
    {
        public AudioAlbum(XmlNode node)
        {
            Id = node.Int("id");
            OwnerId = node.Int("owner_id");
            Title = node.String("title");
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}