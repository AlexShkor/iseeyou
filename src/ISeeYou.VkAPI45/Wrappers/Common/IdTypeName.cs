using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Сущность, содержит свойства Id, Type, Name
    /// </summary>
    public class IdTypeName
    {
        public IdTypeName(System.Xml.XmlNode node)
        {
            Id = node.Int("id");
            Name = node.String("name");
            Type = node.String("type");
        }

        /// <summary>
        /// Идентификатор обьекта
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Имя, название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public string Type { get; set; }
    }
}
