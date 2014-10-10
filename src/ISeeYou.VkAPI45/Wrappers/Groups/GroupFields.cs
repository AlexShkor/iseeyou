namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Опциональные поля группы
    /// </summary>
    public static class GroupFields
    {
        /// <summary>
        /// Все поля
        /// </summary>
        public static readonly string[] All = new[]
            {
                "city", "country", "place", "description", "wiki_page", "members_count", "counters", "start_date",
                "end_date", "can_see_all_posts", "can_create_topic", "status", "contacts", "links", "fixed_post", "verified",
                "can_post", "activity", "site", "can_upload_doc"
            };

        /// <summary>
        /// Идентификатор города, указанного в информации о сообществе. Если город не указан, возвращается 0. 
        /// </summary>
        public static readonly string City = "city";

        /// <summary>
        /// Идентификатор страны, указанной в информации о сообществе. Если страна не указана, возвращается 0. 
        /// </summary>
        public static readonly string Country = "country";

        /// <summary>
        /// Место, указанное в информации о сообществе.
        /// </summary>
        public static readonly string Place = "place";

        /// <summary>
        /// Текст описания сообщества. 
        /// </summary>
        public static readonly string Description = "description";

        /// <summary>
        /// Название главной вики-страницы сообщества. 
        /// </summary>
        public static readonly string WikiPage = "wiki_page";

        /// <summary>
        /// Количество участников сообщества. 
        /// </summary>
        public static readonly string MembersCount = "members_count";

        /// <summary>
        /// Возвращается объект counters, содержащий счётчики сообщества
        /// </summary>
        public static readonly string Counters = "counters";

        /// <summary>
        /// Время начала встречи, для публичной страницы - дата основания
        /// </summary>
        public static readonly string StartDate = "start_date";

        /// <summary>
        /// Время окончания встречи, для публичной страницы - не возвращается
        /// </summary>
        public static readonly string EndDate = "end_date";

        /// <summary>
        /// Информация о том, может ли текущий пользователь оставлять записи на стене сообщества.
        /// </summary>
        public static readonly string CanPost = "can_post";

        /// <summary>
        /// Строка состояния публичной страницы. У групп возвращается строковое значение, открыта ли группа или нет, а у событий дата начала. 
        /// </summary>
        public static readonly string Activity = "activity";

        /// <summary>
        /// Информация о том, разрешено видеть чужие записи на стене группы. 
        /// </summary>
        public static readonly string CanSeeAllPosts = "can_see_all_posts";

        /// <summary>
        /// Информация о том, может ли текущий пользователь создать тему обсуждения в группе, используя метод board.addTopic.
        /// </summary>
        public static readonly string CanCreateTopic = "can_create_topic";

        /// <summary>
        /// Статус сообщества. Возвращается строка, содержащая текст статуса, расположенного на странице сообщества под его названием. 
        /// </summary>
        public static readonly string Status = "status";

        /// <summary>
        /// Информация из блока контактов публичной страницы. 
        /// </summary>
        public static readonly string Contacts = "contacts";

        /// <summary>
        /// Информация из блока ссылок сообщества. 
        /// </summary>
        public static readonly string Links = "links";

        /// <summary>
        /// Идентификатор post_id закрепленного поста сообщества. Сам пост можно получить, используя wall.getById, передав в поле posts – {group_id}_{post_id}. 
        /// </summary>
        public static readonly string FixedPost = "fixed_post";

        /// <summary>
        /// Возвращает информацию о том, является ли сообщество верифицированным. 
        /// </summary>
        public static readonly string Verified = "verified";

        /// <summary>
        /// Адрес сайта из поля «веб-сайт» в описании сообщества. 
        /// </summary>
        public static readonly string Site = "site";

        /// <summary>
        /// Информация о том, может ли текущий пользователь загружать документы в группу.
        /// </summary>
        public static readonly string CanUploadDoc = "can_upload_doc";
    }
}