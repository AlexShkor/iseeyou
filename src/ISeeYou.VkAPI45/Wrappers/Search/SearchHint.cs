using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Users;

namespace VkAPIAsync.Wrappers.Search
{
    /// <summary>
    /// Подсказка при поиске
    /// </summary>
    public class SearchHint
    {
        public SearchHint(XmlNode node)
        {
            Type = (HintType) node.Enum("type", typeof(HintType));
            Sections = node.String("sections");
            Description = node.String("description");
        }

        /// <summary>
        /// Тип подсказки
        /// </summary>
        public HintType Type { get; set; }

        /// <summary>
        /// Тип объекта (для сообществ — 'groups', 'events' или 'publics', для профилей — 'correspondents', 'people', 'friends', или 'mutual_friends')
        /// </summary>
        public string Sections { get; set; }

        /// <summary>
        /// Описание объекта (для сообществ — тип и число участников, например, 'Group, 269,136 members', для профилей друзей или пользователями, которые не являются возможными друзьями — название университета или город, для профиля текущего пользователя — 'That's you', для профилей возможных друзей — 'N mutual friends').
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Поисковая подсказка о пользователе
    /// </summary>
    public class UserSearchHint : SearchHint
    {
        public UserSearchHint(XmlNode node) : base(node)
        {
            var profile = node.SelectSingleNode("profile");
            if (profile != null)
            {
                User = new BaseUser(profile);
            }
        }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        public BaseUser User { get; set; }
    }

    /// <summary>
    /// Поискаовая подсказка о группе
    /// </summary>
    public class GroupSearchHint : SearchHint
    {
        public GroupSearchHint(XmlNode node) : base(node)
        {
            var n = node.SelectSingleNode("group");
            if (n != null)
            {
                Group = new BaseGroup(n);
            }
        }

        /// <summary>
        /// Информация о группе
        /// </summary>
        public BaseGroup Group { get; set; }
    }

    /// <summary>
    /// Тип подсказки
    /// </summary>
    public enum HintType
    {
        /// <summary>
        /// Группа
        /// </summary>
        Group,

        /// <summary>
        /// Пользователь
        /// </summary>
        Profile
    }
}
