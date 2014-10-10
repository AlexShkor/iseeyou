#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Audios;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    /// Минимальный набор данных о пользователе
    /// </summary>
    public class BaseUser
    {
        public BaseUser(int? id)
        {
            Id = id;
        }

        public BaseUser(XmlNode node)
        {
            Id = node.Int("id");
            FirstName = node.String("first_name");
            LastName = node.String("last_name");

            var sex = node.Int(UserFields.Sex);
            if (sex.HasValue)
            {
                Sex = (UserSex.UserSexEnum)sex;
            }       

            Hidden = node.Bool("hidden");
            Deactivated = node.String("deactivated");

            Status = node.String(UserFields.Status);
            var statusAudioNode = node.SelectSingleNode("status_audio");
            if (statusAudioNode != null)
            {
                StatusAudio = new Audio(statusAudioNode);
            }
        }

        public BaseUser()
        {

        }

        /// <summary>
        ///     Id пользователя
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///     Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Пол
        /// </summary>
        public UserSex.UserSexEnum Sex { get; set; }

        /// <summary>
        /// Если пользователь в настройках приватности скрыл свою страницу от неавторизованных в ВК посетителей, то true
        /// </summary>
        public bool? Hidden { get; set; }

        /// <summary>
        /// Поле, содержащее строку "deleted" или "banned" в случае, если запрашиваемый аккаунт заблокирован или удален владельцем. 
        /// </summary>
        public string Deactivated { get; set; }

        /// <summary>
        /// Статус пользователя
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Аудиозапись, которую слушает пользователь
        /// </summary>
        public Audio StatusAudio { get; set; }
    }

    /// <summary>
    /// Пользователь ВК
    /// </summary>
    public class User : BaseUser
    {
        public User(int? id) : base(id)
        {
            
        }

        public User(XmlNode node) : base(node)
        {
            //Base
            Domain = node.String(UserFields.Domain);
            BirthDate = node.String(UserFields.BirthDate);
            Nickname = node.String(UserFields.Nickname);
            ScreenName = node.String(UserFields.ScreenName);
            Timezone = node.Short(UserFields.Timezone);
            Blacklisted = node.Bool(UserFields.Blacklisted);
            Hometown = node.String(UserFields.Hometown);
            FollowersCount = node.Int(UserFields.FollowersCount);
            CommonCount = node.Int(UserFields.CommonCount);
            if (node.SelectSingleNode(UserFields.City) != null)
            {
                City = new IdTitleObject(node.SelectSingleNode(UserFields.City));
            }
            if (node.SelectSingleNode(UserFields.Country) != null)
            {
                Country = new IdTitleObject(node.SelectSingleNode(UserFields.Country));
            }

            //Photos
            Photo50 = node.String(UserFields.Photo50);
            Photo100 = node.String(UserFields.Photo100);
            Photo200 = node.String(UserFields.Photo200);
            Photo200Orig = node.String(UserFields.Photo200Orig);
            Photo400Orig = node.String(UserFields.Photo400Orig);
            PhotoMax = node.String(UserFields.PhotoMax);
            PhotoMaxOrig = node.String(UserFields.PhotoMaxOrig);

            //Status
            LastSeen = node.DateTimeFromUnixTime("last_seen/time");
            Online = node.Bool(UserFields.Online);
            OnlineMobile = node.Bool("online_mobile");
            OnlineApp = node.Int("online_app");
            if (node.Int("relation") > 0)
                Relation = (UserMaritalStatus.UserMaritalStatusEnum)node.Int("relation");

            //Education
            University = node.Int("university");
            UniversityName = node.String("university_name");
            Faculty = node.Int("faculty");
            FacultyName = node.String("faculty_name");
            Graduation = node.Int("graduation");
            var universities = node.SelectNodes("universities/*");
            if (universities.Count > 0)
            {
                Universities = universities.Cast<XmlNode>().Select(x => new University(x)).ToList();
            }
            var schools = node.SelectNodes("schools/*");
            if (schools.Count > 0)
            {
                Schools = schools.Cast<XmlNode>().Select(x => new School(x)).ToList();
            }

            //Contacts
            Lists = Friends.Friends.BuildFriendsList(node.SelectNodes("lists/list/item").OfType<XmlNode>());
            HomePhone = node.String("home_phone");
            MobilePhone = node.String("mobile_phone");
            HasMobile = node.Bool("has_mobile");
            Site = node.String(UserFields.Site);

            //Posibilities
            CanPost = node.Bool(UserFields.CanPost);
            CanSeeAllPosts = node.Bool(UserFields.CanSeeAllPosts);
            CanSeeAudio = node.Bool(UserFields.CanSeeAudio);
            CanWritePrivateMessage = node.Bool(UserFields.CanWritePrivateMessage);
            WallComments = node.Bool(UserFields.WallComments);

            //Connections
            Skype = node.String("skype");
            Facebook = node.String("facebook");
            Twitter = node.String("twitter");
            Instagram = node.String("instagram");
            LiveJournal = node.String("live_journal");

            //Personal
            Political = node.Short("personal/political");
            var languagesNodes = node.SelectNodes("personal/langs/*");
            if (languagesNodes.Count > 0)
            {
                Languages = languagesNodes.Cast<XmlNode>().Select(x => x.InnerText).ToList();
            }
            Religion = node.String("personal/religion");
            InspiredBy = node.String("personal/inspired_by");
            PeopleMain = node.Short("personal/people_main");
            LifeMain = node.Short("personal/life_main");
            Alcohol = node.Short("personal/alcohol");
            Smoking = node.Short("personal/smoking");

            //Info
            Activities = node.String(UserFields.Activities);
            Interests = node.String(UserFields.Interests);
            Music = node.String(UserFields.Music);
            Movies = node.String(UserFields.Movies);
            Tv = node.String(UserFields.TV);
            Books = node.String(UserFields.Books);
            Games = node.String(UserFields.Games);
            About = node.String(UserFields.About);
            Quotes = node.String(UserFields.Quotes);

            var occupationNode = node.SelectSingleNode(UserFields.Occupation);
            if (occupationNode != null)
            {
                Occupation = new IdTypeName(occupationNode);
            }

            var relativesNode = node.SelectSingleNode(UserFields.Relatives);
            if (relativesNode != null && relativesNode.HasChildNodes)
            {
                Relatives = relativesNode.ChildNodes.Cast<XmlNode>().Select(x => new IdTypeName(x)).ToList();
            }

            var countersNode = node.SelectSingleNode(UserFields.Counters);
            if (countersNode != null)
            {
                Counters = new UserCounters(countersNode);
            }
        }
        

        public User()
        {
            
        }

        #region Base

        /// <summary>
        ///     Дата рождения
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        ///     Id и Title города, в котором живет пользователь
        /// </summary>
        public IdTitleObject City { get; set; }

        /// <summary>
        ///     Id и Title страны, в которой живет пользователь
        /// </summary>
        public IdTitleObject Country { get; set; }

        /// <summary>
        ///     Пользовательский домен. Например, vladrocklike
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Никнейм (отчество) пользователя.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Короткое имя (поддомен) страницы пользователя.
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// Временная зона пользователя. Возвращается только при запросе информации о текущем пользователе.
        /// </summary>
        public short? Timezone { get; set; }

        /// <summary>
        /// Возвращается true, если текущий пользователь находится в черном списке у запрашиваемого.
        /// </summary>
        public bool? Blacklisted { get; set; }

        /// <summary>
        /// Название родного города пользователя
        /// </summary>
        public string Hometown { get; set; }

        /// <summary>
        /// Количество подписчиков пользователя.
        /// </summary>
        public int? FollowersCount { get; set; }

        /// <summary>
        /// Количество общих друзей с текущим пользователем.
        /// </summary>
        public int? CommonCount { get; set; }

        #endregion

        #region Photo

        /// <summary>
        ///     url квадратной фотографии пользователя, имеющей ширину 50 пикселей
        /// </summary>
        public string Photo50 { get; set; }

        /// <summary>
        ///     url квадратной фотографии пользователя, имеющей ширину 100 пикселей
        /// </summary>
        public string Photo100 { get; set; }

        /// <summary>
        ///     url фотографии пользователя, имеющей ширину 200 пикселей
        /// </summary>
        public string Photo200Orig { get; set; }

        /// <summary>
        /// url фотографии пользователя, имеющей ширину 400 пикселей. Если у пользователя отсутствует фотография такого размера, ответ не будет содержать этого поля.
        /// </summary>
        public string Photo400Orig { get; set; }

        /// <summary>
        ///     url квадратной фотографии пользователя, имеющей ширину 200 пикселей
        /// </summary>
        public string Photo200 { get; set; }

        /// <summary>
        ///     url квадратной фотографии пользователя с максимальной шириной
        /// </summary>
        public string PhotoMax { get; set; }

        /// <summary>
        /// url фотографии пользователя максимального размера. Может быть возвращена фотография, имеющая ширину как 400, так и 200 пикселей
        /// </summary>
        public string PhotoMaxOrig { get; set; }

        #endregion

        #region Status

        /// <summary>
        ///     Пользователь онлайн
        /// </summary>
        public bool? Online { get; set; }

        /// <summary>
        ///     Пользователь онлайн с мобильного телефона
        /// </summary>
        public bool? OnlineMobile { get; set; }

        /// <summary>
        /// Идентификатор приложения, которое используется в данный момент
        /// </summary>
        public int? OnlineApp { get; set; }

        /// <summary>
        /// Время последнего посещения
        /// </summary>
        public DateTime? LastSeen { get; set; }

        /// <summary>
        /// Семейное положение пользователя
        /// </summary>
        public UserMaritalStatus.UserMaritalStatusEnum Relation { get; set; }

        #endregion

        #region Education

        /// <summary>
        ///     Id факультета
        /// </summary>
        public int? Faculty { get; set; }

        /// <summary>
        ///     Название факультета
        /// </summary>
        public string FacultyName { get; set; }

        /// <summary>
        ///     Образование
        /// </summary>
        public int? Graduation { get; set; }

        /// <summary>
        ///     Id университета
        /// </summary>
        public int? University { get; set; }

        /// <summary>
        ///     Название университета
        /// </summary>
        public string UniversityName { get; set; }

        /// <summary>
        /// Список высших учебных заведений, в которых учился текущий пользователь.
        /// </summary>
        public List<University> Universities { get; set; }

        /// <summary>
        /// Список школ, в которых учился пользователь.
        /// </summary>
        public List<School> Schools { get; set; }

        #endregion

        #region Contacts

        /// <summary>
        ///     Мобильный телефон доступен
        /// </summary>
        public bool? HasMobile { get; set; }

        /// <summary>
        ///     Домашний телефон
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        ///     Листы, в которые добавлен пользователь
        /// </summary>
        public List<int> Lists { get; set; }

        /// <summary>
        ///     Мобильный телефон
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// Указанный в профиле сайт пользователя
        /// </summary>
        public string Site { get; set; }

        #endregion

        #region Posibilities

        /// <summary>
        /// Информация о том, разрешено ли оставлять записи на стене у пользователя
        /// </summary>
        public bool? CanPost { get; set; }

        /// <summary>
        /// Информация о том, разрешено ли видеть чужие записи на стене пользователя
        /// </summary>
        public bool? CanSeeAllPosts { get; set; }

        /// <summary>
        /// Информация о том, разрешено ли видеть чужие аудиозаписи на стене пользователя
        /// </summary>
        public bool? CanSeeAudio { get; set; }

        /// <summary>
        /// Информация о том, разрешено ли написание личных сообщений данному пользователю
        /// </summary>
        public bool? CanWritePrivateMessage { get; set; }

        /// <summary>
        /// Доступно ли комментирование стены
        /// </summary>
        public bool? WallComments { get; set; }

        #endregion

        #region Connections

        /// <summary>
        /// Skype пользователя
        /// </summary>
        public string Skype { get; set; }

        /// <summary>
        /// Facebook пользователя
        /// </summary>
        public string Facebook { get; set; }

        /// <summary>
        /// Twitter пользователя
        /// </summary>
        public string Twitter { get; set; }

        /// <summary>
        /// LiveJournal пользователя
        /// </summary>
        public string LiveJournal { get; set; }

        /// <summary>
        /// Instagram пользователя
        /// </summary>
        public string Instagram { get; set; }

        #endregion

        #region  Personal

        /// <summary>
        ///  Политические предпочтения.
        /// </summary>
        public short? Political { get; set; }

        /// <summary>
        ///  Языки.
        /// </summary>
        public List<string> Languages { get; set; }

        /// <summary>
        /// Мировоззрение.
        /// </summary>
        public string Religion { get; set; }

        /// <summary>
        /// Источники вдохновения.
        /// </summary>
        public string InspiredBy { get; set; }

        /// <summary>
        /// Главное в людях.
        /// </summary>
        public short? PeopleMain { get; set; }

        /// <summary>
        /// Главное в жизни.
        /// </summary>
        public short? LifeMain { get; set; }

        /// <summary>
        /// Отношение к курению.
        /// </summary>
        public short? Smoking { get; set; }

        /// <summary>
        /// Отношение к алкоголю.
        /// </summary>
        public short? Alcohol { get; set; }

        #endregion

        #region Info

        /// <summary>
        /// Деятельность.
        /// </summary>
        public string Activities { get; set; }

        /// <summary>
        /// Интересы.
        /// </summary>
        public string Interests { get; set; }

        /// <summary>
        /// Любимая музыка.
        /// </summary>
        public string Music { get; set; }

        /// <summary>
        /// Любимые фильмы.
        /// </summary>
        public string Movies { get; set; }

        /// <summary>
        /// Любимые шоу.
        /// </summary>
        public string Tv { get; set; }

        /// <summary>
        /// Любимые книги.
        /// </summary>
        public string Books { get; set; }

        /// <summary>
        /// Любимые игры.
        /// </summary>
        public string Games { get; set; }

        /// <summary>
        /// "О себе".
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Любимые цитаты.
        /// </summary>
        public string Quotes { get; set; }

        #endregion

        /// <summary>
        /// Информация о текущем роде занятия пользователя
        /// </summary>
        public IdTypeName Occupation { get; set; }

        /// <summary>
        /// Cписок родственников текущего пользователя.
        /// </summary>
        public List<IdTypeName> Relatives { get; set; }

        /// <summary>
        /// Количество различных объектов у пользователя
        /// </summary>
        public UserCounters Counters { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}