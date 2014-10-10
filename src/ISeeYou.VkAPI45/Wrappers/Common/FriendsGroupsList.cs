using System.Collections.Generic;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Users;

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Список идентификаторов пользователей и друзей
    /// </summary>
    public class UsersGroupsIDList
    {
        public ListCount<int> Groups { get; set; }
        public ListCount<int> Users { get; set; }
    }

    /// <summary>
    /// Список пользователей и друзей
    /// </summary>
    public class UsersGroupsList
    {
        public List<Group> Groups { get; set; }
        public List<BaseUser> Users { get; set; }
    }
}
