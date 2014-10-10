#region Using

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Widgets
{
    /// <summary>
    /// Информация страниц
    /// </summary>
    public class PagesInfo
    {
        public PagesInfo(XmlNode node)
        {
            Count = node.Int("count");
            var pages = node.SelectNodes("pages/page");
            if (pages != null && pages.Count > 0)
            {
                Pages = pages.Cast<XmlNode>().Select(x => new Page(x)).ToList();
            }
        }

        /// <summary>
        /// Количество страниц
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Список страниц
        /// </summary>
        public List<Page> Pages { get; set; }
    }
}