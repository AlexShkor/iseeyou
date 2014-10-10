#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Базовая сущность группы
    /// </summary>
    public class BaseGroup : BaseEntity
    {
        public BaseGroup(XmlNode x)
        {
            Id = x.Int("id");
            Name = x.String("name");
            ScreenName = x.String("screen_name");
            IsClosed = x.Short("is_closed");
            Deactivated = x.String("deactivated");
            IsAdmin = x.Bool("is_admin");
            if (IsAdmin.HasValue && IsAdmin.Value)
            {
                AdminLevel = new AdminLevel((Wrappers.Groups.AdminLevel.AdminLevelEnum)x.Int("admin_level"));
            }
            IsMember = x.Bool("is_member");
            Photo = x.String("photo_50");
            PhotoMedium = x.String("photo_100");
            PhotoBig = x.String("photo_200");
            Type = new GroupType((GroupType.GroupTypeEnum)x.Enum("type", typeof(GroupType.GroupTypeEnum)));
        }

        /// <summary>
        ///     Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Тип сообщества
        /// </summary>
        public GroupType Type { get; set; }

        /// <summary>
        ///     Короткое название
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        ///     Является ли сообщество закрытым. Возможные значения:
        ///     0 — открытое;
        ///     1 — закрытое;
        ///     2 — частное.
        /// </summary>
        public short? IsClosed { get; set; }

        /// <summary>
        /// Возвращается в случае, если сообщество удалено или заблокировано:
        /// deleted — удалено;
        /// banned — заблокировано;
        /// </summary>
        public string Deactivated { get; set; }

        /// <summary>
        ///     Флаг, является ли пользователь адмиистратором
        /// </summary>
        public bool? IsAdmin { get; set; }

        /// <summary>
        /// Полномочия текущего пользователя
        /// </summary>
        public AdminLevel AdminLevel { get; set; }

        /// <summary>
        ///     Флаг, является ли пользователь участником группы
        /// </summary>
        public bool? IsMember { get; set; }

        /// <summary>
        ///     Ссылка на изображение
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        ///     Ссылка на изображение среднего размера
        /// </summary>
        public string PhotoMedium { get; set; }

        /// <summary>
        ///     Ссылка на изображение большого размера
        /// </summary>
        public string PhotoBig { get; set; }
    }

    /// <summary>
    /// Информация о сообществе с доп. полем InvitedBy
    /// </summary>
    public class GroupInvitedBy : BaseGroup
    {
        public GroupInvitedBy(XmlNode x) : base(x)
        {
            InvitedBy = x.Int("invited_by");
        }

        /// <summary>
        /// Идентификатор пользователя, который отправил приглашение
        /// </summary>
        public int? InvitedBy { get; set; }
    }

    /// <summary>
    /// Сообщество
    /// </summary>
    public class Group : BaseGroup
    {
        public Group(XmlNode x) : base(x)
        {
            Activity = x.String(GroupFields.Activity);
            City = x.Int(GroupFields.City);
            Country = x.Int(GroupFields.Country);
            var placeNode = x.SelectSingleNode(GroupFields.Place);
            if (placeNode != null) Place = new Place(placeNode);
            Description = x.String(GroupFields.Description);
            WikiPage = x.String(GroupFields.Description);
            MembersCount = x.Int(GroupFields.MembersCount);
            CanPost = x.Bool(GroupFields.CanPost);
            StartDate = x.DateTimeFromUnixTime(GroupFields.StartDate);
            EndDate = x.DateTimeFromUnixTime(GroupFields.EndDate);
            CanSeeAllPosts = x.Bool(GroupFields.CanSeeAllPosts);
            CanUploadDoc = x.Bool(GroupFields.CanUploadDoc);
            CanCreateTopic = x.Bool(GroupFields.CanCreateTopic);
            Status = x.String(GroupFields.Status);
            Contacts = x.String(GroupFields.Contacts);
            Links = x.String(GroupFields.Links);
            FixedPost = x.Int(GroupFields.FixedPost);
            Site = x.String(GroupFields.Site);
            Verified = x.Bool(GroupFields.Verified);

            var countersNode = x.SelectSingleNode(GroupFields.Counters);
            if (countersNode != null)
                Counters = new GroupCounters(countersNode);
        }

        /// <summary>
        /// Идентификатор города, указанного в информации о сообществе. 
        /// </summary>
        public int? City { get; set; }

        /// <summary>
        /// Идентификатор страны, указанной в информации о сообществе. 
        /// </summary>
        public int? Country { get; set; }

        /// <summary>
        /// Место, указанное в информации о сообществе
        /// </summary>
        public Place Place { get; set; }

        /// <summary>
        /// Текст описания сообщества
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Название главной вики-страницы сообщества
        /// </summary>
        public string WikiPage { get; set; }

        /// <summary>
        /// Количество участников сообщества
        /// </summary>
        public int? MembersCount { get; set; }

        /// <summary>
        /// Время начала встречи
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Время окончания встречи
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Информация о том, может ли текущий пользователь оставлять записи на стене сообщества.
        /// </summary>
        public bool? CanPost { get; set; }

        /// <summary>
        /// Информация о том, разрешено видеть чужие записи на стене группы. 
        /// </summary>
        public bool? CanSeeAllPosts { get; set; }

        /// <summary>
        /// Информация о том, может ли текущий пользователь создать тему обсуждения в группе, используя метод board.addTopic.
        /// </summary>
        public bool? CanCreateTopic { get; set; }

        /// <summary>
        /// Информация о том, может ли текущий пользователь загружать документы в группу.
        /// </summary>
        public bool? CanUploadDoc { get; set; }

        /// <summary>
        /// Строка состояния публичной страницы. У групп возвращается строковое значение, открыта ли группа или нет, а у событий дата начала. 
        /// </summary>
        public string Activity { get; set; }

        /// <summary>
        /// Статус сообщества. Возвращается строка, содержащая текст статуса, расположенного на странице сообщества под его названием. 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Информация из блока контактов публичной страницы.
        /// </summary>
        public string Contacts { get; set; }

        /// <summary>
        /// Информация из блока ссылок сообщества.
        /// </summary>
        public string Links { get; set; }

        /// <summary>
        /// Идентификатор post_id закрепленного поста сообщества. Сам пост можно получить, используя wall.getById, передав в поле posts – {group_id}_{post_id}.
        /// </summary>
        public int? FixedPost { get; set; }

        /// <summary>
        /// Адрес сайта из поля «веб-сайт» в описании сообщества.
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// Возвращает информацию о том, является ли сообщество верифицированным.
        /// </summary>
        public bool? Verified { get; set; }

        /// <summary>
        /// Объект counters, содержащий счётчики сообщества, может включать любой набор из следующих полей: photos, albums, audios, videos, topics, docs.
        /// </summary>
        public GroupCounters Counters { get; set; }
    }
}