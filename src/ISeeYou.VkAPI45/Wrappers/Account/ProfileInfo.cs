using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Users;

namespace VkAPIAsync.Wrappers.Account
{
    /// <summary>
    /// Информация о профиле
    /// </summary>
    public class ProfileInfo
    {
        public ProfileInfo(XmlNode node)
        {
            FirstName = node.String("first_name");
            LastName = node.String("last_name");
            MaidenName = node.String("maiden_name");
            Sex = (UserSex.UserSexEnum)node.Int("sex");

            Relation = (UserMaritalStatus.UserMaritalStatusEnum) node.Int("relation");
            if (node.SelectSingleNode("relation_user") != null)
            {
                RelationUser = new BaseUser(node.SelectSingleNode("relation_user"));
            }

            BirthDate = DateTime.Parse(node.String("bdate"));
            BirthDateVisibility = node.Short("bdate_visibility");

            Hometown = node.String("hometown");
            if (node.SelectSingleNode("city") != null)
            {
                City = new IdTitleObject(node.SelectSingleNode("city"));
            }
            if (node.SelectSingleNode("country") != null)
            {
                Country = new IdTitleObject(node.SelectSingleNode("country"));
            }

            if (node.SelectSingleNode("name_request") != null)
            {
                NameRequest = new NameRequest(node.SelectSingleNode("name_request"));
            }
        }

        /// <summary>
        ///  Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///  Девичья фамилия пользователя (только для женского пола)
        /// </summary>
        public string MaidenName { get; set; }

        /// <summary>
        ///  Пол пользователя
        /// </summary>
        public UserSex.UserSexEnum Sex { get; set; }

        /// <summary>
        /// Семейное положение пользователя
        /// </summary>
        public UserMaritalStatus.UserMaritalStatusEnum Relation { get; set; }

        /// <summary>
        ///  Объект пользователя, с которым связано семейное положение (если есть)
        /// </summary>
        public BaseUser RelationUser { get; set; }

        /// <summary>
        ///  Дата рождения пользователя
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// видимость даты рождения, возвращаемые значения:
        /// 1 — показывать дату рождения;
        /// 2 — показывать только месяц и день;
        /// 0 — не показывать дату рождения;
        /// </summary>
        public short? BirthDateVisibility { get; set; }

        /// <summary>
        /// Родной город пользователя;
        /// </summary>
        public string Hometown { get; set; }

        /// <summary>
        /// Город пользователя
        /// </summary>
        public IdTitleObject City { get; set; }

        /// <summary>
        ///  Страна пользователя
        /// </summary>
        public IdTitleObject Country { get; set; }

        /// <summary>
        /// Объект, содержащий информацию о заявке на смену имени
        /// </summary>
        public NameRequest NameRequest { get; set; }
    }

    /// <summary>
    /// Заявка на смену имени
    /// </summary>
    public class NameRequest
    {
        public NameRequest(XmlNode node)
        {
            Id = node.Int("id");
            Status = node.String("status");
            FirstName = node.String("first_name");
            LastName = node.String("last_name");
        }

        /// <summary>
        /// Идентификатор заявки, необходимый для её отмены (только если status равен processing
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Статус заявки, возвращаемые значения:
        /// processing – заявка рассматривается;
        /// declined – заявка отклонена;
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        ///  Имя пользователя, указанное в заявке
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя, указанная в заявке
        /// </summary>
        public string LastName { get; set; }
    }
}
