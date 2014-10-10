using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Account
{
    /// <summary>
    /// Счетчики аккаунта
    /// </summary>
    public class AccountCounters
    {
        #region Constructor

        public AccountCounters(XmlNode node)
        {
            Messages = node.Int("messages");
            Videos = node.Int("videos");
            Photos = node.Int("photos");
            Notes = node.Int("notes");
            Friends = node.Int("friends");
            Groups = node.Int("groups");
            Gifts = node.Int("gifts");
            Events = node.Int("events");
            Notifications = node.Int("notifications");
        }

        #endregion //Constructor

        #region Properties

        /// <summary>
        ///     Количество новых сообщений
        /// </summary>
        public int? Messages { get; set; }

        /// <summary>
        ///     Количество видеозаписей
        /// </summary>
        public int? Videos { get; set; }

        /// <summary>
        ///     Количество фотографий
        /// </summary>
        public int? Photos { get; set; }

        /// <summary>
        ///     Количество заметок
        /// </summary>
        public int? Notes { get; set; }

        /// <summary>
        ///     Количество друзей
        /// </summary>
        public int? Friends { get; set; }

        /// <summary>
        ///     Количество сообществ
        /// </summary>
        public int? Groups { get; set; }

        /// <summary>
        ///     Количество подарков
        /// </summary>
        public int? Gifts { get; set; }

        /// <summary>
        ///     Количество событий
        /// </summary>
        public int? Events { get; set; }

        /// <summary>
        ///     Количество уведомлений
        /// </summary>
        public int? Notifications { get; set; }


        #endregion //Properties
    }
}
