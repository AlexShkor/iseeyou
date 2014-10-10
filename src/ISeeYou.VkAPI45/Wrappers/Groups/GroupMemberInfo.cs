#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Информация о учаснике группы
    /// </summary>
    public class GroupMemberInfo
    {
        public GroupMemberInfo(XmlNode node)
        {
            IsMember = node.Bool("member");
            Request = node.Bool("request");
            Invitation = node.Bool("invitation");
        }

        /// <summary>
        /// Является ли пользователь участником сообщества
        /// </summary>
        public bool? IsMember { get; set; }

        /// <summary>
        /// Есть ли непринятая заявка от пользователя на вступление в группу (такую заявку можно отозвать методом groups.leave);
        /// </summary>
        public bool? Request { get; set; }

        /// <summary>
        /// Приглашён ли пользователь в группу или встречу
        /// </summary>
        public bool? Invitation { get; set; }
    }
}