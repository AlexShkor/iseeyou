#region Using

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Board
{
    /// <summary>
    /// Информация о темах
    /// </summary>
    public class TopicsInfo
    {
        public TopicsInfo(XmlNode node)
        {
            Topics = new ListCount<Topic>(node.Int("count").Value,
                                               node.SelectNodes("//topic")
                                                   .Cast<XmlNode>()
                                                   .Select(x => new Topic(x))
                                                   .ToList());
            DefaultOrder = (TopicsSortOrder.TopicsSortOrderEnum) node.Int("default_order");
            CanAddTopics = node.Bool("can_add_topics");
            var profiles = node.SelectNodes("users/user");
            if (profiles != null && profiles.Count > 0)
            {
                Profiles = profiles.Cast<XmlNode>().Select(x => new User(x)).ToList();
            }
        }

        /// <summary>
        /// Описания каждой из тем 
        /// </summary>
        public ListCount<Topic> Topics { get; set; }

        /// <summary>
        ///  Тип сортировки по умолчанию
        /// </summary>
        public TopicsSortOrder.TopicsSortOrderEnum DefaultOrder { get; set; }

        /// <summary>
        /// true в том случае, если текущий пользователь может создавать новые темы в обсуждениях данной группы
        /// </summary>
        public bool? CanAddTopics { get; set; }

        /// <summary>
        /// В случае передачи параметра extended равным true, поле profiles содержит массив объектов users с информацией о данных пользователей, являющихся создателями тем или оставившими в них последнее сообщение.
        /// </summary>
        public List<User> Profiles { get; set; }
    }
}