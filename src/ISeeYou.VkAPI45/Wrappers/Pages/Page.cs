#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Photos;

#endregion

namespace VkAPIAsync.Wrappers.Pages
{
    /// <summary>
    /// Страница
    /// </summary>
    public class Page
    {
        /// <summary>
        ///  Указывает, кто может редактировать вики-страницу 
        /// </summary>
        public AccessWikiPrivacy.AccessWikiPrivacyEnum WhoCanEdit;

        /// <summary>
        ///  Указывает, кто может просматривать вики-страницу
        /// </summary>
        public AccessWikiPrivacy.AccessWikiPrivacyEnum WhoCanView;

        public Page(XmlNode node)
        {
            Id = node.Int("id");
            GroupId = node.Int("group_id");
            CreatorId = node.Int("creator_id");
            Title = node.String("title");
            Source = node.String("source");
            CurrentUserCanEdit = node.Bool("current_user_can_edit");
            CurrentUserCanEditAccess = node.Bool("current_user_can_edit_access");

            var whoCanView = node.Enum("who_can_view", typeof(AccessWikiPrivacy.AccessWikiPrivacyEnum));
            if (whoCanView != null)
            {
                WhoCanView = (AccessWikiPrivacy.AccessWikiPrivacyEnum)whoCanView;
            }
            var whoCanEdit = node.Enum("who_can_edit", typeof(AccessWikiPrivacy.AccessWikiPrivacyEnum));
            if (whoCanEdit != null)
            {
                WhoCanEdit = (AccessWikiPrivacy.AccessWikiPrivacyEnum)whoCanEdit;
            }

            EditorId = node.Int("editor_id");
            Edited = DateTime.Parse(node.String("edited"));
            Created = DateTime.Parse(node.String("created"));
            Parent = node.String("parent");
            Parent2 = node.String("parent2");
            Html = node.String("html");
            ViewUrl = node.String("view_url");
        }

        /// <summary>
        ///  Идентификатор страницы
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Адрес страницы для отображения вики-страницы
        /// </summary>
        public string ViewUrl { get; set; }

        /// <summary>
        /// Идентификатор сообщества
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        ///  Идентификатор создателя страницы
        /// </summary>
        public int? CreatorId { get; set; }

        /// <summary>
        /// Название страницы
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст страницы в вики-формате
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Указывает, может ли текущий пользователь редактировать текст страницы
        /// </summary>
        public bool? CurrentUserCanEdit { get; set; }

        /// <summary>
        /// Указывает, может ли текущий пользователь изменять права доступа на страницу
        /// </summary>
        public bool? CurrentUserCanEditAccess { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который редактировал страницу последним
        /// </summary>
        public int? EditorId { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime Edited { get; set; }

        /// <summary>
        ///  Дата создания страницы
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///  Заголовок родительской страницы для навигации, если есть
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        ///  Заголовок второй родительской страницы для навигации, если есть
        /// </summary>
        public string Parent2 { get; set; }

        /// <summary>
        /// HTML представление страницы
        /// </summary>
        public string Html { get; set; }
    }

    /// <summary>
    /// Расширенная версия страницы
    /// </summary>
    public class VersionPageObject
    {
        public VersionPageObject(XmlNode node)
        {
            Pid = node.Int("pid");
            Hid = node.Int("hid");
            Created = node.DateTimeFromUnixTime("version_created");
            Parent = node.String("parent");
            Parent2 = node.String("parent2");
            GroupId = node.Int("group_id");
            CreatorId = node.Int("creator_id");
            Title = node.String("title");
            Source = node.String("source");
            CurrentUserCanEdit = node.Bool("current_user_can_edit");

            var whoCanView = node.Enum("who_can_view", typeof(AccessWikiPrivacy.AccessWikiPrivacyEnum));
            if (whoCanView != null)
            {
                WhoCanView = (AccessWikiPrivacy.AccessWikiPrivacyEnum)whoCanView;
            }
            var whoCanEdit = node.Enum("who_can_edit", typeof(AccessWikiPrivacy.AccessWikiPrivacyEnum));
            if (whoCanEdit != null)
            {
                WhoCanEdit = (AccessWikiPrivacy.AccessWikiPrivacyEnum)whoCanEdit;
            }
        }

        /// <summary>
        ///  Идентификатор версии страницы
        /// </summary>
        public int? Hid { get; set; }

        /// <summary>
        ///  Идентификатор страницы
        /// </summary>
        public int? Pid { get; set; }     

        /// <summary>
        ///  Дата создания версии страницы
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        ///  Заголовок родительской страницы для навигации, если есть
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        ///  Заголовок второй родительской страницы для навигации, если есть
        /// </summary>
        public string Parent2 { get; set; }

        /// <summary>
        ///  Указывает, кто может редактировать вики-страницу 
        /// </summary>
        public AccessWikiPrivacy.AccessWikiPrivacyEnum WhoCanEdit;

        /// <summary>
        ///  Указывает, кто может просматривать вики-страницу
        /// </summary>
        public AccessWikiPrivacy.AccessWikiPrivacyEnum WhoCanView;

        /// <summary>
        /// Идентификатор сообщества
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        ///  Идентификатор создателя страницы
        /// </summary>
        public int? CreatorId { get; set; }

        /// <summary>
        /// Название страницы
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Текст страницы в вики-формате
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Указывает, может ли текущий пользователь редактировать текст страницы
        /// </summary>
        public bool? CurrentUserCanEdit { get; set; }
    }

    /// <summary>
    /// Версия страницы
    /// </summary>
    public class HistoryPageObject
    {
        public HistoryPageObject(XmlNode node)
        {
            Hid = node.Int("hid");
            Length = node.Int("length");
            EditorName = node.String("editor_name");
            EditorId = node.Int("editor_id");
            Edited = DateTime.Parse(node.String("edited"));
        }

        /// <summary>
        ///  Идентификатор версии страницы
        /// </summary>
        public int? Hid { get; set; }

        /// <summary>
        ///  Длина версии страницы в байтах
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Имя редактора
        /// </summary>
        public string EditorName { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который редактировал страницу последним
        /// </summary>
        public int? EditorId { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime Edited { get; set; }
    }
}