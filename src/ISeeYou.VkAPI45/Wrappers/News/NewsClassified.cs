#region Using

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.News
{
    /// <summary>
    /// Лента новостей
    /// </summary>
    public class NewsClassified
    {
        private List<BaseGroup> _groupes;
        private List<NewsEntity> _items;

        private List<BaseUser> _profiles;

        public NewsClassified(XmlNode x)
        {
            var items =
                x.SelectSingleNode("items")
                 .ChildNodes.Cast<XmlNode>()
                 .Where(item => item.NodeType == XmlNodeType.Element && item.LocalName == "item");
            var nodes = items as List<XmlNode> ?? items.ToList();
            if (nodes.Any())
            {
                foreach (var item in nodes)
                {
                    Items.Add(new NewsEntity(item));
                }
            }

            var profiles =
                x.SelectSingleNode("profiles")
                 .ChildNodes.Cast<XmlNode>()
                 .Where(item => item.NodeType == XmlNodeType.Element && item.LocalName == "user");
            nodes = profiles as List<XmlNode> ?? profiles.ToList();
            if (nodes.Any())
            {
                foreach (var profile in nodes)
                    Profiles.Add(new User(profile));
            }

            var groups =
                x.SelectSingleNode("groups")
                 .ChildNodes.Cast<XmlNode>()
                 .Where(item => item.NodeType == XmlNodeType.Element && item.LocalName == "group");
            nodes = groups as List<XmlNode> ?? groups.ToList();
            if (nodes.Any())
            {
                foreach (var group in nodes)
                    Groupes.Add(new Group(group));
            }

            NextFrom = x.SelectSingleNode("next_from").InnerText;
        }

        /// <summary>
        /// Массив новостей для текущего пользователя
        /// </summary>
        public List<NewsEntity> Items
        {
            get { return _items ?? (_items = new List<NewsEntity>()); }
            set { _items = value; }
        }

        /// <summary>
        /// Информация о пользователях, которые находятся в списке новостей
        /// </summary>
        public List<BaseUser> Profiles
        {
            get { return _profiles ?? (_profiles = new List<BaseUser>()); }
            set { _profiles = value; }
        }

        /// <summary>
        /// Информация о группах, которые находятся в списке новостей
        /// </summary>
        public List<BaseGroup> Groupes
        {
            get { return _groupes ?? (_groupes = new List<BaseGroup>()); }
            set { _groupes = value; }
        }

        /// <summary>
        ///  start_from, приходящий в виде строки в поле next_from для получения следующей страницы.
        /// </summary>
        public string NextFrom { get; set; }
    }
}