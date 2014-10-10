#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// Пользователь, который участвует в чате
    /// </summary>
    public class ChatUser : BaseUser
    {
        public ChatUser(XmlNode node) : base(node)
        {
            InvitedBy = node.Int("invited_by");
        }

        public ChatUser(int userId) : base(userId)
        {
        }

        /// <summary>
        /// Идентификатор пользователя, который пригласил в беседу
        /// </summary>
        public int? InvitedBy { get; set; }
    }
}