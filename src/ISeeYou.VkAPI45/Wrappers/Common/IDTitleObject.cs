#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Сущность, которая включает в себя два свойства - Id, Title
    /// </summary>
    public class IdTitleObject
    {
        public IdTitleObject(XmlNode node, string idNodeName = "id", string titleNodeName = "title")
        {
            Id = node.Int(idNodeName);
            Title = node.String(titleNodeName);
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Title) ? base.ToString() : Title;
        }
    }
}