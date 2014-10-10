#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    /// Счетчики пользователя
    /// </summary>
    public class UserCounters : BaseEntity
    {
        #region Constructor

        public UserCounters(XmlNode node)
        {
            if (node == null)
                return;

            Albums = node.Int(UserCountersFields.Albums);
            Videos = node.Int(UserCountersFields.Videos);
            Audios = node.Int(UserCountersFields.Audios);
            Photos = node.Int(UserCountersFields.Photos);
            Notes = node.Int(UserCountersFields.Notes);
            Friends = node.Int(UserCountersFields.Friends);
            Groups = node.Int(UserCountersFields.Groups);
            OnlineFriends = node.Int(UserCountersFields.OnlineFriends);
            MutualFriends = node.Int(UserCountersFields.MutualFriends);
            UserVideos = node.Int(UserCountersFields.UserVideos);
            Followers = node.Int(UserCountersFields.Followers);
            UserPhotos = node.Int(UserCountersFields.UserPhotos);
            Subscriptions = node.Int(UserCountersFields.Subscriptions);
        }

        #endregion //Constructor

        #region Properties

        /// <summary>
        ///     Количество фотоальбомов
        /// </summary>
        public int? Albums { get; set; }

        /// <summary>
        ///     Количество видеозаписей
        /// </summary>
        public int? Videos { get; set; }

        /// <summary>
        ///     Количество аудиозаписей
        /// </summary>
        public int? Audios { get; set; }

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
        ///     Количество друзей онлайн
        /// </summary>
        public int? OnlineFriends { get; set; }

        /// <summary>
        ///     Количество общих друзей
        /// </summary>
        public int? MutualFriends { get; set; }

        /// <summary>
        ///     Количество видеозаписей с пользователем
        /// </summary>
        public int? UserVideos { get; set; }

        /// <summary>
        ///     Количество подписчиков
        /// </summary>
        public int? Followers { get; set; }

        /// <summary>
        ///     Количество фотографий с пользователем
        /// </summary>
        public int? UserPhotos { get; set; }

        /// <summary>
        ///     Количество подписок (только пользователи)
        /// </summary>
        public int? Subscriptions { get; set; }

        /// <summary>
        /// Количество фотографий
        /// </summary>
        public int? Photos { get; set; }

        #endregion //Properties
    }

    public static class UserCountersFields
    {
        public static readonly string Albums = "albums";
        public static readonly string Videos = "videos";
        public static readonly string Audios = "audios";
        public static readonly string Notes = "notes";
        public static readonly string Friends = "friends";
        public static readonly string Groups = "groups";
        public static readonly string OnlineFriends = "online_friends";
        public static readonly string MutualFriends = "mutual_friends";
        public static readonly string UserVideos = "user_videos";
        public static readonly string Followers = "followers";
        public static readonly string UserPhotos = "user_photos";
        public static readonly string Subscriptions = "subscriptions";
        public static readonly string Photos = "photos";
    }
}