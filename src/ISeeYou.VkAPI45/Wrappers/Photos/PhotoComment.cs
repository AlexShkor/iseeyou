#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Photos
{
    /// <summary>
    /// Комментарий к фотографии
    /// </summary>
    public class PhotoComment : Comment
    {
        /// <summary>
        /// Идентификатор фотографии
        /// </summary>
        public int? PhotoId;

        public PhotoComment(XmlNode node) : base(node)
        {
            PhotoId = node.Int("pid");
        }
    }
}