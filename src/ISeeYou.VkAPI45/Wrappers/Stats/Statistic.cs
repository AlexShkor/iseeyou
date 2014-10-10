#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Stats
{
    /// <summary>
    /// Статистика
    /// </summary>
    /// <typeparam name="T">Значение демографического показателя</typeparam>
    public class Statistic<T>
    {
        public Statistic(XmlNode node, Delegate valueParser)
        {
            Visitors = node.Int("visitors");
            Value = (T) valueParser.DynamicInvoke(node.String("value"));
            Name = node.String("name");
        }

        /// <summary>
        /// Аудитория для показателя value
        /// </summary>
        public int? Visitors { get; set; }

        /// <summary>
        /// Значение демографического показателя, имеет разные возможные значения для разных показателей
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        ///  Наглядное название значения указанного в value (только для городов)
        /// </summary>
        public string Name { get; set; }
    }
}