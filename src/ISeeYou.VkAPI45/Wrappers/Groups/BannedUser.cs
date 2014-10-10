using System.Xml;
using VkAPIAsync.Wrappers.Users;

namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Пользователь, которого забанили
    /// </summary>
    public class BannedUser : User
    {
        public BannedUser(XmlNode node)
            : base(node)
        {
            var info = node.SelectSingleNode("ban_info");
            if (info != null)
                BanInfo = new BanInfo(info);
        }

        /// <summary>
        /// Информация о бане пользователя
        /// </summary>
        public BanInfo BanInfo { get; set; }
    }
}
