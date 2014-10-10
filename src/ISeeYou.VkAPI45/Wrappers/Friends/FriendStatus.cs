#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Friends
{
    /// <summary>
    /// Статус дружбы с пользователем
    /// </summary>
    public class FriendStatus
    {
        public FriendStatus(XmlNode node)
        {
            Uid = node.Int("user_id");
            Status = (FriendStatusEnum) node.Int("friend_status");
            RequestMessage = node.String("request_message");
            RequestReaded = node.SelectSingleNode("read_state") != null ? node.Bool("read_state") : null;
        }

        /// <summary>
        ///     Id пользователя
        /// </summary>
        public int? Uid { get; set; }

        /// <summary>
        ///     Статус дружбы относительно текущего пользователя
        /// </summary>
        public FriendStatusEnum Status { get; set; }

        /// <summary>
        ///     Текст заявки в друзья (если есть)
        /// </summary>
        public string RequestMessage { get; set; }

        /// <summary>
        /// Статус заявки (false — не просмотрена, true — просмотрена), возвращается только если friend_status = 2
        /// </summary>
        public bool?  RequestReaded { get; set; }
    }

    /// <summary>
    ///  Статус дружбы с пользователем
    /// </summary>
    public enum FriendStatusEnum
    {
        /// <summary>
        /// Пользователь не является другом
        /// </summary>
        NotFriend = 0,
        /// <summary>
        /// Отправлена заявка/подписка пользователю
        /// </summary>
        OutRequest = 1,
        /// <summary>
        /// Имеется входящая заявка/подписка от пользователя
        /// </summary>
        InRequest = 2,
        /// <summary>
        ///  Пользователь является другом
        /// </summary>
        Friend = 3
    }
}