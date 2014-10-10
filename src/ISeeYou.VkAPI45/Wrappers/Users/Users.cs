#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public static class Users
    {
        /// <summary>
        ///     Возвращает расширенную информацию о пользователях.
        /// </summary>
        /// <param name="uids">Перечисленные через запятую ID пользователей или их короткие имена (screen_name). Максимум 1000 пользователей.</param>
        /// <param name="fields">Перечисленные через запятую поля анкет, необходимые для получения. Доступные значения: userId, first_name, last_name, nickname, screen_name, sex, bdate (birthdate), city, country, timezone, photo, photo_medium, photo_big, has_mobile, rate, contacts, education, online, counters.</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя. Возможные значения: именительный – nom, родительный – gen, дательный – dat, винительный – acc, творительный – ins, предложный – abl. По умолчанию nom.</param>
        public static async Task<List<User>> Get(IEnumerable<string> uids = null, IEnumerable<string> fields = null,
                                          string nameCase = null)
        {
            VkAPI.Manager.Method("users.get");
            if (nameCase != null)
            {
                VkAPI.Manager.Params("name_case", nameCase);
            }
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((str, cur) => str + "," + cur));
            }
            if (uids != null)
            {
                VkAPI.Manager.Params("user_ids", uids.Aggregate((a,b) => a + "," + b));
            }

            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }
                var nodes =
                    resp.ChildNodes.OfType<XmlNode>()
                     .Where(el => el.NodeType == XmlNodeType.Element && el.LocalName == "user");
                return nodes.Select(node => new User(node)).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список пользователей в соответствии с заданным критерием поиска.
        /// </summary>
        /// <param name="query">Cтрока поискового запроса. Например, Вася Бабич.</param>
        /// <param name="fields">Перечисленные через запятую поля анкет, необходимые для получения. Доступные значения: userId, first_name, last_name, nickname, screen_name, sex, bdate (birthdate), city, country, timezone, photo, photo_medium, photo_big, has_mobile, rate, contacts, education, online.</param>
        /// <param name="count">Количество возвращаемых пользователей (максимум 1000). По умолчанию 20.</param>
        /// <param name="offset">Смещение относительно первого найденного пользователя для выборки определенного подмножества.</param>
        /// <param name="ageFrom">Начиная с какого возраста. </param>
        /// <param name="ageTo">До какого возраста.</param>
        /// <param name="birthDay">День рождения. </param>
        /// <param name="birthMonth">Месяц рождения. </param>
        /// <param name="birthYear">Год рождения. </param>
        /// <param name="city">Идентификатор города. </param>
        /// <param name="company">Название компании, в которой работают пользователи. </param>
        /// <param name="country">Идентификатор страны.</param>
        /// <param name="groupId">Идентификатор группы, среди пользователей которой необходимо проводить поиск. </param>
        /// <param name="hasPhoto">Только с фотографией</param>
        /// <param name="hometown">Название города строкой</param>
        /// <param name="interests">Интересы.</param>
        /// <param name="isOnline">Только в сети</param>
        /// <param name="position">Название должности.</param>
        /// <param name="religion">Религиозные взгляды.</param>
        /// <param name="school">Идентификатор школы, которую закончили пользователи.</param>
        /// <param name="schoolCity">Идентификатор города, в котором пользователи закончили школу. </param>
        /// <param name="schoolCountry">Идентификатор страны, в которой пользователи закончили школу.</param>
        /// <param name="schoolYear">Год окончания школы.</param>
        /// <param name="sex">Пол</param>
        /// <param name="sort">Сортировка результатов</param>
        /// <param name="status">Семейное положение</param>
        /// <param name="university">Идентификатор ВУЗа.</param>
        /// <param name="universityCountry">Идентификатор страны, в которой пользователи закончили ВУЗ.</param>
        /// <param name="universityYear">Год окончания ВУЗа.</param>
        public static async Task<ListCount<User>> Search(string query = null, IEnumerable<string> fields = null, int? count = null,
                                             int? offset = null, UsersSortOrder sort = null, int? city =  null, int? country = null,
                                             string hometown = null, int? universityCountry = null, int? university = null, int? universityYear = null, int? universityFaculty = null, int? universityChair = null,
                                             UserSex sex = null, UserMaritalStatus status = null, int? ageFrom = null, int? ageTo = null, int? birthDay = null, int? birthMonth = null, int? birthYear = null,
                                             bool? isOnline = null, bool? hasPhoto = null, int? school = null, int? schoolCountry = null, int? schoolCity = null, int? schoolYear = null, int? schoolClass = null,
                                             string religion = null, string interests = null, string company = null, string position = null, int? groupId = null)
        {
            VkAPI.Manager.Method("users.search");

            if (query != null)
            {
                VkAPI.Manager.Params("q", query);
            }
            if (fields != null)
            {
                int fieldsCount = fields.Count();
                VkAPI.Manager.Params("fields", fields.Aggregate((str, cur) => str + "," + cur));
            }
            if (count.HasValue)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset.HasValue)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if(sort != null)
            {
                VkAPI.Manager.Params("sort", sort.Value);
            }
            if (city.HasValue)
            {
                VkAPI.Manager.Params("city", city.Value);
            }
            if (country.HasValue)
            {
                VkAPI.Manager.Params("country", country.Value);
            }
            if (hometown != null)
            {
                VkAPI.Manager.Params("hometown", hometown);
            }
            if (universityCountry.HasValue)
            {
                VkAPI.Manager.Params("university_country", universityCountry.Value);
            }
            if (university.HasValue)
            {
                VkAPI.Manager.Params("university", university.Value);
            }
            if (universityYear.HasValue)
            {
                VkAPI.Manager.Params("university_year", universityYear.Value);
            }
            if (universityFaculty.HasValue)
            {
                VkAPI.Manager.Params("university_faculty", universityFaculty.Value);
            }
            if (universityChair.HasValue)
            {
                VkAPI.Manager.Params("university_chair", universityChair.Value);
            }
            if(sex != null)
            {
                VkAPI.Manager.Params("sex", sex.Value);
            }
            if(status != null)
            {
                VkAPI.Manager.Params("status", status.Value);
            }
            if (ageFrom.HasValue)
            {
                VkAPI.Manager.Params("age_from", ageFrom.Value);
            }
            if (ageTo.HasValue)
            {
                VkAPI.Manager.Params("age_to", ageTo.Value);
            }
            if (birthDay.HasValue)
            {
                VkAPI.Manager.Params("birth_day", birthDay.Value);
            }
            if (birthMonth.HasValue)
            {
                VkAPI.Manager.Params("birth_month", birthMonth.Value);
            }
            if (birthYear.HasValue)
            {
                VkAPI.Manager.Params("birth_year", birthYear.Value);
            }
            if (isOnline.HasValue)
            {
                VkAPI.Manager.Params("online", isOnline.Value ? 1 : 0);
            }
            if (hasPhoto.HasValue)
            {
                VkAPI.Manager.Params("has_photo", hasPhoto.Value ? 1 : 0);
            }
            if (school.HasValue)
            {
                VkAPI.Manager.Params("school", school.Value);
            }
            if (schoolYear.HasValue)
            {
                VkAPI.Manager.Params("school_year", schoolYear.Value);
            }
            if (schoolCountry.HasValue)
            {
                VkAPI.Manager.Params("school_country", schoolCountry.Value);
            }
            if (schoolCity.HasValue)
            {
                VkAPI.Manager.Params("school_city", schoolCity.Value);
            }
            if (schoolClass.HasValue)
            {
                VkAPI.Manager.Params("school_class", schoolClass.Value);
            }
            if(religion != null)
            {
                VkAPI.Manager.Params("religion", religion);
            }
            if(interests != null)
            {
                VkAPI.Manager.Params("interests", interests);
            }
            if (company != null)
            {
                VkAPI.Manager.Params("company", company);
            }
            if (position != null)
            {
                VkAPI.Manager.Params("position", position);
            }
            if (groupId.HasValue)
            {
                VkAPI.Manager.Params("group_id", groupId.Value);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }
                var nodes =
                    resp.SelectNodes("items/user");
                return new ListCount<User>(resp.Int("count").Value, nodes.Cast<XmlNode>().Select(node => new User(node)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает список идентификаторов пользователей, которые являются подписчиками пользователя. Идентификаторы пользователей в списке отсортированы в порядке убывания времени их добавления.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя. </param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя. Возможные значения: именительный – nom, родительный – gen, дательный – dat, винительный – acc, творительный – ins, предложный – abl. По умолчанию nom. </param>
        /// <param name="count">Количество подписчиков, информацию о которых нужно получить. </param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества подписчиков. </param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть. </param>
        public static async Task<ListCount<User>> GetFollowers(int? uid = null, string nameCase = null, int? count = null,
                                              int? offset = null, string[] fields = null)
        {
            VkAPI.Manager.Method("users.getFollowers");
            if (uid != null)
            {
                VkAPI.Manager.Params("user_id", uid);
            }
            if (nameCase != null)
            {
                VkAPI.Manager.Params("name_case", nameCase);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", String.Join(",", fields));
            }

            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                var c = resp.Int("count");
                XmlNodeList msgsNodes = resp.SelectNodes("items/user");
                var res = msgsNodes.Count > 0 ? new ListCount<User>(c.Value, msgsNodes.Cast<XmlNode>().Select(y => new User(y)).ToList()) : null;
                if (res != null) return res;
                msgsNodes = resp.SelectNodes("items/user_id");
                res = msgsNodes.Count > 0 ? new ListCount<User>(c.Value, msgsNodes.Cast<XmlNode>().Select(y => new User() { Id = int.Parse(y.InnerText) }).ToList()) : null;
                return res;
            }
            return null;
        }

        /// <summary>
        /// Позволяет пожаловаться на пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, на которого осуществляется жалоба</param>
        /// <param name="type">Тип жалобы</param>
        /// <param name="comment">Комментарий к жалобе на пользователя</param>
        /// <returns>В случае успешной жалобы метод вернет true.</returns>
        public static async Task<bool> Report(int userId, UserReportType type, string comment = null)
        {
            VkAPI.Manager.Method("users.report");
            VkAPI.Manager.Params("user_id", userId);
            VkAPI.Manager.Params("type", type.StringValue);
            if (comment != null)
            {
                VkAPI.Manager.Params("comment", comment);
            }
            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return false;
                }
                return resp.BoolVal().Value;
            }
            return false;
        }

        /// <summary>
        /// Возвращает список идентификаторов пользователей и групп, которые входят в список подписок пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, подписки которого необходимо получить. </param>
        public static async Task<UsersGroupsIDList> GetSubcriptions(int? userId = null)
        {
            VkAPI.Manager.Method("users.getSubscriptions");
            if (userId != null)
            {
                VkAPI.Manager.Params("user_id", userId.Value);
            }
            
            var resp = (await VkAPI.Manager.Execute(false)).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }
                XmlNodeList groupsNodes = resp.SelectNodes("groups/items/group_id");
                XmlNodeList usersNodes = resp.SelectNodes("users/items/user_id");
                var res = new UsersGroupsIDList();
                res.Groups = new ListCount<int>(resp.SelectSingleNode("groups/count").IntVal().Value, (from XmlNode node in groupsNodes select node.IntVal().Value).ToList());
                res.Users = new ListCount<int>(resp.SelectSingleNode("users/count").IntVal().Value, (from XmlNode node in usersNodes select node.IntVal().Value).ToList());
                return res;
            }
            return null;
        }
    }
}