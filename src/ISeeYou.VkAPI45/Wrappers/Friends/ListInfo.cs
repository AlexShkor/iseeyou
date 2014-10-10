#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Friends
{
    /// <summary>
    /// Информация о списке пользователей
    /// </summary>
    public class ListInfo
    {
        public ListInfo(XmlNode node)
        {
            Id = node.Int("id");
            Name = node.String("name");
        }

        /// <summary>
        /// Идентификатор списка
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Название списка
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? base.ToString() : Name;
        }
    }
}