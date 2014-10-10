namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    /// Поля
    /// </summary>
    public static class UserFields
    {
        /// <summary>
        /// Все поля
        /// </summary>
        public static readonly string[] All = new[]
            {
                "sex", "bdate", "city", "country", "photo_50", "photo_100", "photo_200", "photo_200_orig",
                "photo_400_orig", "photo_max", "photo_max_orig", "online", "lists", "domain", "has_mobile",
                "contacts", "connections", "site", "education", "universities", "schools", "can_post", 
                "can_see_all_posts", "can_see_audio", "can_write_private_message", "status", "last_seen",
                "common_count", "relation", "relatives", "counters", "verified", "followers_count", "blacklisted",
                "wall_comments", "timezone", "screen_name", "personal", "activities", "interests", "music",
                "movies", "tv", "books", "games", "about", "quotes", "home_town", "occupation", "nickname"
            };

        public static readonly string[] Main = new[]
            {
                "sex", "bdate", "city", "country", "photo_200", "online", "domain", "nickname",
                "contacts", "site", "education", "can_post", "can_see_all_posts", "can_see_audio", 
                "can_write_private_message", "status", "last_seen", "common_count", "relation", "counters", "followers_count",
                "wall_comments", "timezone", "screen_name", "personal", "activities", "interests", "music",
                "movies", "tv", "books", "games", "about", "quotes", "home_town"
            };

        /// <summary>
        ///    Пол пользователя
        /// </summary>
        public static readonly string Sex = "sex";

        /// <summary>
        ///     Дата рождения. Возвращается в формате DD.MM.YYYY или DD.MM (если год рождения скрыт). Если дата рождения скрыта целиком, поле отсутствует в ответе.
        /// </summary>
        public static readonly string BirthDate = "bdate";

        /// <summary>
        ///     Идентификатор города, указанного на странице пользователя в разделе «Контакты».
        /// </summary>
        public static readonly string City = "city";

        /// <summary>
        ///     Идентификатор страны, указанной на странице пользователя в разделе «Контакты».
        /// </summary>
        public static readonly string Country = "country";

        /// <summary>
        /// Возвращается true, если страница пользователя верифицирована, false — если не верифицирована.
        /// </summary>
        public static readonly string Verified = "verified";

        /// <summary>
        ///     url квадратной фотографии пользователя, имеющей ширину 50 пикселей. В случае отсутствия у пользователя фотографии возвращается http://vk.com/images/camera_c.gif.
        /// </summary>
        public static readonly string Photo50 = "photo_50";

        /// <summary>
        ///     url квадратной фотографии пользователя, имеющей ширину 100 пикселей. В случае отсутствия у пользователя фотографии возвращается http://vk.com/images/camera_b.gif.
        /// </summary>
        public static readonly string Photo100 = "photo_100";

        /// <summary>
        ///     url квадратной фотографии пользователя, имеющей ширину 200 пикселей. В случае отсутствия у пользователя фотографии возвращается http://vk.com/images/camera_a.gif.
        /// </summary>
        public static readonly string Photo200 = "photo_200";

        /// <summary>
        ///     url фотографии пользователя, имеющей ширину 200 пикселей. В случае отсутствия у пользователя фотографии возвращается http://vk.com/images/camera_a.gif.
        /// </summary>
        public static readonly string Photo200Orig = "photo_200_orig";

        /// <summary>
        ///     url фотографии пользователя, имеющей ширину 400 пикселей. Если у пользователя отсутствует фотография такого размера, ответ не будет содержать этого поля.
        /// </summary>
        public static readonly string Photo400Orig = "photo_400_orig";

        /// <summary>
        ///     url квадратной фотографии пользователя с максимальной шириной. Может быть возвращена фотография, имеющая ширину как 200, так и 100 пикселей. В случае отсутствия у пользователя фотографии возвращается http://vk.com/images/camera_b.gif.
        /// </summary>
        public static readonly string PhotoMax = "photo_max";

        /// <summary>
        ///    url фотографии пользователя максимального размера. Может быть возвращена фотография, имеющая ширину как 400, так и 200 пикселей. В случае отсутствия у пользователя фотографии возвращается http://vk.com/images/camera_a.gif.
        /// </summary>
        public static readonly string PhotoMaxOrig = "photo_max_orig";

        /// <summary>
        ///     Информация о том, находится ли пользователь сейчас на сайте.
        ///     Возвращаемые значения: 1 — находится, 0 — не находится.
        ///     Если пользователь использует мобильное приложение либо мобильную версию сайта,
        ///     возвращается дополнительное поле online_mobile, содержащее 1.
        ///     При этом, если используется именно приложение, дополнительно возвращается поле online_app, содержащее его идентификатор.
        /// </summary>
        public static readonly string Online = "online";

        /// <summary>
        /// Доступно ли комментирование стены
        /// </summary>
        public static readonly string WallComments = "wall_coments";

        /// <summary>
        ///     Разделенные запятой идентификаторы списков друзей, в которых состоит пользователь. Поле доступно только для метода friends.get. Получить информацию об id и названиях списков друзей можно с помощью метода friends.getLists. Если пользователь не состоит ни в одном списке друзей, данное поле отсутствует в ответе.
        /// </summary>
        public static readonly string Lists = "lists";

        /// <summary>
        ///     Короткий адрес страницы. Возвращается строка, содержащая короткий адрес страницы (возвращается только сам поддомен, например, andrew). Если он не назначен, возвращается "id"+userId, например, id35828305.
        /// </summary>
        public static readonly string Domain = "domain";

        /// <summary>
        ///     Информация о том, известен ли номер мобильного телефона пользователя. Возвращаемые значения: 1 — известен, 0 — не известен. Рекомендуется использовать перед вызовом метода secure.sendSMSNotification.
        /// </summary>
        public static readonly string HasMobile = "has_mobile";

        /// <summary>
        ///     Информация о телефонных номерах пользователя.
        ///     Если данные указаны и не скрыты настройками приватности, возвращаются следующие поля:
        ///     mobile_phone — номер мобильного телефона пользователя;
        ///     home_phone — номер домашнего телефона пользователя.
        /// </summary>
        public static readonly string Contacts = "contacts";

        /// <summary>
        ///     Возвращает данные о подключенных сервисах пользователя, таких как: skype, facebook, twitter, livejournal, instagram.
        /// </summary>
        public static readonly string Connections = "connections";

        /// <summary>
        /// Возвращает указанный в профиле сайт пользователя.
        /// </summary>
        public static readonly string Site = "site";

        /// <summary>
        /// Информация о высшем учебном заведении пользователя
        /// </summary>
        public static readonly string Education = "education";

        /// <summary>
        /// Список высших учебных заведений, в которых учился текущий пользователь.
        /// </summary>
        public static readonly string Universities = "universities";

        /// <summary>
        /// Список школ, в которых учился пользователь
        /// </summary>
        public static readonly string Schools = "schools";

        /// <summary>
        /// Информация о том, разрешено ли оставлять записи на стене у пользователя.
        /// </summary>
        public static readonly string CanPost = "can_post";

        /// <summary>
        /// Информация о том, разрешено ли видеть чужие записи на стене пользователя.
        /// </summary>
        public static readonly string CanSeeAllPosts = "can_see_all_posts";

        /// <summary>
        /// Информация о том, разрешено ли видеть чужие аудиозаписи на стене пользователя
        /// </summary>
        public static readonly string CanSeeAudio = "can_see_audio";

        /// <summary>
        /// Информация о том, разрешено ли написание личных сообщений данному пользователю.
        /// </summary>
        public static readonly string CanWritePrivateMessage = "can_write_private_message";

        /// <summary>
        /// Статус пользователя. Возвращается строка, содержащая текст статуса, расположенного в профиле под именем пользователя. Если у пользователя включена опция «Транслировать в статус играющую музыку», будет возвращено дополнительное поле status_audio, содержащее информацию о транслируемой композиции.
        /// </summary>
        public static readonly string Status = "status";
        
        /// <summary>
        /// время последнего посещения. Возвращается объект last_seen со следующими полями:
        /// time - время последнего посещения в формате unixtime. 
        /// platform - тип платформы, через которую был осуществлён последний вход. 
        /// </summary>
        public static readonly string LastSeen = "last_seen";

        /// <summary>
        /// Количество общих друзей с текущим пользователем.
        /// </summary>
        public static readonly string CommonCount = "common_count";

        /// <summary>
        /// Количество подписчиков пользователя
        /// </summary>
        public static readonly string FollowersCount = "followers_count";

        /// <summary>
        /// Семейное положение пользователя
        /// </summary>
        public static readonly string Relation = "relation";

        /// <summary>
        /// Список родственников текущего пользователя Возвращает список обхектов cодержащих поля id, type. (и name в случаях, когда родственник не является пользователем ВКонтакте).
        /// </summary>
        public static readonly string Relatives = "relatives";

        /// <summary>
        /// Количество различных объектов у пользователя. Может быть использовано только в методе users.get при запросе информации об одном пользователе, с передачей access_token. 
        /// </summary>
        public static readonly string Counters = "counters";

        /// <summary>
        /// Возвращается true, если текущий пользователь находится в черном списке у запрашиваемого.
        /// </summary>
        public static readonly string Blacklisted = "blacklisted";

        /// <summary>
        /// Временная зона пользователя. Возвращается только при запросе информации о текущем пользователе.
        /// </summary>
        public static readonly string Timezone = "timezone";

        /// <summary>
        /// Информация о полях из раздела «Жизненная позиция»
        /// </summary>
        public static readonly string Personal = "personal";

        /// <summary>
        /// Деятельность.
        /// </summary>
        public static readonly string Activities = "activities";

        /// <summary>
        /// Интересы.
        /// </summary>
        public static readonly string Interests = "interests";

        /// <summary>
        /// Любимая музыка.
        /// </summary>
        public static readonly string Music = "music";

        /// <summary>
        /// Любимые фильмы.
        /// </summary>
        public static readonly string Movies = "movies";

        /// <summary>
        /// Любимые шоу.
        /// </summary>
        public static readonly string TV = "tv";

        /// <summary>
        /// Любимые книги.
        /// </summary>
        public static readonly string Books = "books";

        /// <summary>
        /// Любимые игры.
        /// </summary>
        public static readonly string Games = "games";

        /// <summary>
        /// "О себе".
        /// </summary>
        public static readonly string About = "about";

        /// <summary>
        /// Любимые цитаты.
        /// </summary>
        public static readonly string Quotes = "quotes";

        /// <summary>
        /// Название родного города пользователя.
        /// </summary>
        public static readonly string Hometown = "home_town";

        /// <summary>
        /// Информация о текущем роде занятия пользователя.
        /// </summary>
        public static readonly string Occupation = "occupation";

        /// <summary>
        /// Никнейм (отчество) пользователя.
        /// </summary>
        public static readonly string Nickname = "nickname";

        /// <summary>
        /// Короткое имя (поддомен) страницы пользователя.
        /// </summary>
        public static readonly string ScreenName = "screen_name";
    }
}