using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Photos
{
    /// <summary>
    /// Приватность
    /// </summary>
    public class Privacy
    {
        public Privacy(XmlNode node)
        {
            Type = AccessPrivacyType.Parse(node.String("type"));

            var lists = node.SelectNodes("/lists/*");
            if (lists != null)
            {
                Lists = lists.OfType<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
            }
            var excLists = node.SelectNodes("/except_lists/*");
            if (excLists != null)
            {
                ExceptLists = excLists.OfType<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
            }
            var users = node.SelectNodes("/users/*");
            if (users != null)
            {
                Users = users.OfType<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
            }
            var excUsers = node.SelectNodes("/except_users/*");
            if (excUsers != null)
            {
                ExceptUsers = users.OfType<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
            }
        }

        /// <summary>
        /// Тип приватности
        /// </summary>
        public AccessPrivacyType Type { get; set; }

        /// <summary>
        ///  Массив из пользовательских списков, которые добавляются к пользователям принадлежащим к типу в поле type
        /// </summary>
        public List<int> Lists { get; set; }

        /// <summary>
        ///  Массив из пользовательских списков, которые исключаются из пользователей принадлежащих к типу в поле type
        /// </summary>
        public List<int> ExceptLists { get; set; }

        /// <summary>
        /// Массив из идентификаторов пользователей, которые добавляются к пользователям принадлежащим к типу в поле type
        /// </summary>
        public List<int> Users { get; set; }

        /// <summary>
        /// Массив из идентификаторов пользователей, которые исключаются из пользователей принадлежащих к типу в поле type
        /// </summary>
        public List<int> ExceptUsers { get; set; }
    }

    /// <summary>
    ///  Тип приватности
    /// </summary>
    public class AccessPrivacyType
    {
        public enum AccessPrivacyTypeEnum
        {
            /// <summary>
            /// Ни один пользователь
            /// </summary>
            Nobody,
            /// <summary>
            /// Все пользователи
            /// </summary>
            All,
            /// <summary>
            /// Только друзья
            /// </summary>
            Friends,
            /// <summary>
            /// Друзья и друзья друзей
            /// </summary>
            FriendsOfFriends,
            /// <summary>
            /// Определенный список пользователей, переданный в поле users
            /// </summary>
            Users
        }

        public AccessPrivacyType(AccessPrivacyTypeEnum privacy)
        {
             switch(privacy)
             {
                 case AccessPrivacyTypeEnum.All:
                     StringValue = "all";
                     break;
                 case AccessPrivacyTypeEnum.Friends:
                     StringValue = "friends";
                     break;
                 case AccessPrivacyTypeEnum.FriendsOfFriends:
                     StringValue = "friends_of_friends";
                     break;
                 case AccessPrivacyTypeEnum.Nobody:
                     StringValue = "nobody";
                     break;
                 case AccessPrivacyTypeEnum.Users:
                     StringValue = "users";
                     break;
             }
        }

        public static AccessPrivacyType Parse(string value)
        {
            switch(value)
            {
                case "all":
                    return new AccessPrivacyType(AccessPrivacyTypeEnum.All);
                case "friends":
                    return new AccessPrivacyType(AccessPrivacyTypeEnum.Friends);
                case "friends_of_friends":
                    return new AccessPrivacyType(AccessPrivacyTypeEnum.FriendsOfFriends);
                case "nobody":
                    return new AccessPrivacyType(AccessPrivacyTypeEnum.Nobody);
                case "users":
                    return new AccessPrivacyType(AccessPrivacyTypeEnum.Users);
                default:
                    throw new ArgumentException();
            }
        }

        public string StringValue {get;set;}

        public override string ToString()
        {
            return StringValue;
        }
    }
}
