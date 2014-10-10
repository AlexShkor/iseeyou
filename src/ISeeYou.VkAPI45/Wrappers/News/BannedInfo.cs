#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.News
{
    /// <summary>
    /// Результат метода newsfeed.getBanned
    /// </summary>
    public class BannedInfo
    {
        public BannedInfo(XmlNode node)
        {
            var groups = node.SelectNodes("groups/*");
            if (groups != null && groups.Count > 0)
            {
                Groups = groups.Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
            }
            var users = node.SelectNodes("members/*");
            if (users != null && users.Count > 0)
            {
                Users = users.Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
            }
            if (Groups == null | Users == null) throw new Exception("Ошибка при обработке данных");
        }

        /// <summary>
        /// Список идентификаторов сообществ, которые пользователь скрыл из ленты новостей
        /// </summary>
        public List<int> Groups { get; set; }

        /// <summary>
        /// Список идентификаторов друзей, которые пользователь скрыл из ленты новостей
        /// </summary>
        public List<int> Users { get; set; }
    }

    /// <summary>
    /// Расширенный результат метода newsfeed.getBanned
    /// </summary>
    public class BannedInfoExtended
    {
        public BannedInfoExtended(XmlNode node)
        {
            var groups = node.SelectNodes("groups/*");
            if (groups != null && groups.Count > 0)
            {
                Groups = groups.Cast<XmlNode>().Select(x => new Group(x)).ToList();
            }
            var users = node.SelectNodes("profiles/*");
            if (users != null && users.Count > 0)
            {
                Users = users.Cast<XmlNode>().Select(x => new User(x)).ToList();
            }
            if (Groups == null | Users == null) throw new Exception("Ошибка при обработке данных");
        }

        /// <summary>
        /// Список сообществ, которые пользователь скрыл из ленты новостей
        /// </summary>
        public List<Group> Groups { get; set; }

        /// <summary>
        /// Список друзей, которые пользователь скрыл из ленты новостей
        /// </summary>
        public List<User> Users { get; set; }
    }
}