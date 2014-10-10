#region Using

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Группы
    /// </summary>
    public static class Groups
    {
        /// <summary>
        ///     Возвращает список групп указанного пользователя. (Полная информация о группах пользователя)
        /// </summary>
        /// <param name="userId">ID пользователя, группы которого необходимо получить. По умолчанию выбираются группы текущего пользователя.</param>
        /// <param name="filter">
        ///     Список фильтров сообществ, которые необходимо вернуть, перечисленные через запятую. Доступны значения admin, groups, publics, events. По умолчанию возвращаются все сообщества пользователя.
        ///     При указании фильтра admin будут возвращены администрируемые пользователем сообщества.
        /// </param>
        /// <param name="fields">Список полей из информации о группах, которые необходимо получить.</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества групп.</param>
        /// <param name="count">Количество записей которое необходимо вернуть, не более 1000.</param>
        public static async Task<ListCount<Group>> GetExtended(int? uid = null, GroupsFilter filter = null,
                                                        IEnumerable<string> fields = null, int? offset = null,
                                                        int? count = null)
        {
            VkAPI.Manager.Method("groups.get");
            if (uid != null) VkAPI.Manager.Params("user_id", uid);
            VkAPI.Manager.Params("extended", 1);
            if (filter != null) VkAPI.Manager.Params("filter", filter.Value);
            if (fields != null) VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + ("," + b)));
            if (count != null) VkAPI.Manager.Params("count", count);
            if (offset != null) VkAPI.Manager.Params("offset", offset);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<Group>(result.Int("count").Value,
                                                   result.SelectNodes("items/*")
                                                         .Cast<XmlNode>()
                                                         .Select(x => new Group(x))
                                                         .ToList())
                       : null;
        }

        /// <summary>
        ///     Возвращает список групп указанного пользователя.
        /// </summary>
        /// <param name="userId">ID пользователя, группы которого необходимо получить. По умолчанию выбираются группы текущего пользователя.</param>
        /// <param name="filter">
        ///     Список фильтров сообществ, которые необходимо вернуть, перечисленные через запятую. Доступны значения admin, groups, publics, events. По умолчанию возвращаются все сообщества пользователя.
        ///     При указании фильтра admin будут возвращены администрируемые пользователем сообщества.
        /// </param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества групп.</param>
        /// <param name="count">Количество записей которое необходимо вернуть, не более 1000.</param>
        public static async Task<ListCount<int>> Get(int? uid = null, GroupsFilter filter = null,
                                    int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("groups.get");
            if (uid != null) VkAPI.Manager.Params("user_id", uid);
            VkAPI.Manager.Params("extended", 0);
            if (filter != null) VkAPI.Manager.Params("filter", filter.Value);
            if (count != null) VkAPI.Manager.Params("count", count);
            if (offset != null) VkAPI.Manager.Params("offset", offset);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                ? new ListCount<int>(result.Int("count").Value, result.SelectNodes("items/*").Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList())
                       : null;
        }

        /// <summary>
        ///     Возвращает информацию о заданной группе или о нескольких группах.
        /// </summary>
        /// <param name="gids">ID групп, перечисленные через запятую, информацию о которых необходимо получить. В качестве ID могут быть использованы короткие имена групп. Максимум 500 групп.</param>
        /// <param name="fields">Список полей из информации о группах, которые необходимо получить.</param>
        public static async Task<List<Group>> GetById(IEnumerable<string> gids, IEnumerable<string> fields = null)
        {
            VkAPI.Manager.Method("groups.getById");
            if (fields != null) VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + ("," + b)));
            VkAPI.Manager.Params("group_ids", gids.Aggregate((a, b) => a + ("," + b)));

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? result.SelectNodes("//group").Cast<XmlNode>().Select(x => new Group(x)).ToList()
                       : null;
        }

        /// <summary>
        ///     Возвращает информацию о том является ли пользователь участником заданной группы.
        /// </summary>
        /// <param name="gid">ID или короткое имя группы.</param>
        /// <param name="userId">ID пользователя. По умолчанию ID текущего пользователя.</param>
        public static async Task<bool> IsMember(int gid, int? uid = null)
        {
            VkAPI.Manager.Method("groups.isMember");
            VkAPI.Manager.Params("group_id", gid);
            if (uid != null) VkAPI.Manager.Params("user_id", uid);
            VkAPI.Manager.Params("extended", 0);

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            return apiManager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает информацию о том является ли пользователь участником заданной группы.
        /// </summary>
        /// <param name="gid">ID или короткое имя группы.</param>
        /// <param name="userId">ID пользователя. По умолчанию ID текущего пользователя.</param>
        public static async Task<GroupMemberInfo> IsMemberExtended(int gid, int? uid = null)
        {
            VkAPI.Manager.Method("groups.isMember");
            VkAPI.Manager.Params("group_id", gid);
            if (uid != null) VkAPI.Manager.Params("user_id", uid);
            VkAPI.Manager.Params("extended", 1);

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            return apiManager.MethodSuccessed ? new GroupMemberInfo(result) : null;
        }

        /// <summary>
        ///     Осуществляет поиск групп по заданной подстроке.
        /// </summary>
        /// <param name="q">Поисковый запрос по которому необходимо найти группу.</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества результатов поиска.</param>
        /// <param name="count">Количество результатов поиска которое необходимо вернуть.</param>
        /// <param name="sort">Порядок сортировки</param>
        /// <param name="cityId">Идентификатор города. При передаче этого параметра поле country_id игнорируется. </param>
        /// <param name="countryId">Идентификатор страны.</param>
        /// <param name="future">При передаче значения true будут выведены предстоящие события. Учитывается только при передаче в качестве type значения event. </param>
        /// <param name="type">Тип сообщества. Возможные значения: group, page, event.</param>
        public static async Task<ListCount<Group>> Search(string q, GroupsSortOrder sort = null, int? countryId = null, int? cityId = null, string type = null, int? offset = null, int? count = null, bool? future = null)
        {
            VkAPI.Manager.Method("groups.search");
            VkAPI.Manager.Params("q", q);
            if (offset != null) VkAPI.Manager.Params("offset", offset);
            if (count != null) VkAPI.Manager.Params("count", count);
            if (sort != null) VkAPI.Manager.Params("sort", sort.Value);
            if (countryId != null)
            {
                VkAPI.Manager.Params("country_id", countryId);
            }
            if (cityId != null)
            {
                VkAPI.Manager.Params("city_id", cityId);
            }
            if (future.HasValue)
            {
                VkAPI.Manager.Params("future", future.Value ? 1 : 0);
            }
            if (type != null)
            {
                VkAPI.Manager.Params("type", type);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<Group>(result.Int("count").Value, result.SelectNodes("//group").Cast<XmlNode>().Select(x => new Group(x)).ToList())
                       : null;
        }

        /// <summary>
        ///     Данный метод позволяет вступить в группу, публичную страницу, а также подтверждать об участии во встрече.
        /// </summary>
        /// <param name="gid">Идентификатор группы, публичной страницы или встречи.</param>
        /// <param name="notSure">Опциональный параметр учитываемый, если gid принадлежит встрече. true - Возможно пойду. false - Точно пойду. По умолчанию false.</param>
        /// <returns>В случае успешного вступления в группу метод вернёт true. </returns>
        public static async Task<bool> Join(int gid, bool? notSure = null)
        {
            VkAPI.Manager.Method("groups.join");
            VkAPI.Manager.Params("group_id", gid);
            if (notSure != null)
            {
                VkAPI.Manager.Params("not_sure", notSure.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return apiManager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Данный метод позволяет выходить из группы, публичной страницы, или встречи.
        /// </summary>
        /// <param name="gid">Идентификатор группы, публичной страницы или встречи.</param>
        /// <returns>В случае успешного вступления в группу метод вернёт true. </returns>
        public static async Task<bool> Leave(int gid)
        {
            VkAPI.Manager.Method("groups.leave");
            VkAPI.Manager.Params("group_id", gid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return apiManager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Данный метод возвращает список приглашений в сообщества и встречи.
        /// </summary>
        /// <param name="count">Количество приглашений, которое необходимо вернуть.</param>
        /// <param name="offset">Смещение, необходимое для выборки определённого подмножества приглашений.</param>
        /// <returns>Возвращает информацию о сообществах, приглашения в которые были высланы текущему пользователю. </returns>
        public static async Task<ListCount<GroupInvitedBy>> GetInvites(int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("groups.getInvites");
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                var nodesCount = result.SelectSingleNode("count");
                if (nodes != null && nodes.Count > 0 && nodesCount != null)
                {
                    return new ListCount<GroupInvitedBy>(nodesCount.IntVal().Value,
                                                     nodes.Cast<XmlNode>()
                                                          .Select(x => new GroupInvitedBy(x))
                                                          .ToList());
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает список участников сообщества.
        /// </summary>
        /// <param name="gid">Идентификатор или короткое имя сообщества. </param>
        /// <param name="sort">Сортировка, с которой необходимо вернуть список участников.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества участников. По умолчанию 0. </param>
        /// <param name="count">Количество участников сообщества, информацию о которых необходимо получить. </param>
        /// <param name="fields">Список дополнительных полей, которые необходимо вернуть. </param>
        /// <param name="filter">Фильтр участников сообщества</param>
        public static async Task<ListCount<int>> GetMembers(int gid, GroupsMembersSortOrder sort = null, IEnumerable<string> fields = null, 
                                                int? offset = null, int? count = null, MembersFilter filter = null)
        {
            VkAPI.Manager.Method("groups.getMembers");
            VkAPI.Manager.Params("group_id", gid);
            if (offset != null) VkAPI.Manager.Params("offset", offset);
            if (count != null) VkAPI.Manager.Params("count", count);
            if (sort != null) VkAPI.Manager.Params("sort", sort.Value);
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + b));
            }
            if (filter != null)
            {
                VkAPI.Manager.Params("filter", filter.Value);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<int>(result.Int("count").Value, result.SelectNodes("items/*").Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList())
                       : null;
        }

        /// <summary>
        /// Добавляет пользователя в черный список группы.
        /// </summary>
        /// <param name="gid">Идентификатор группы.</param>
        /// <param name="userId">Идентификатор пользователя, которого нужно добавить в черный список. </param>
        /// <param name="endDate">Дата завершения срока действия бана в формате unixtime. Если параметр не указан пользователь будет заблокирован навсегда. </param>
        /// <param name="reason">Причина бана</param>
        /// <param name="comment">Текст комментария к бану</param>
        /// <param name="commentVisible">Текст комментария будет отображаться пользователю</param>
        /// <returns>В случае успеха метод вернет true</returns>
        public static async Task<bool> BanUser(int gid, int uid, int? endDate = null, BanReason reason = null, string comment = null, bool? commentVisible = null)
        {
            VkAPI.Manager.Method("groups.banUser");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("user_id", uid);

            if (reason != null)
            {
                VkAPI.Manager.Params("reason",reason.Value);
            }
            if (endDate != null)
            {
                VkAPI.Manager.Params("end_date", endDate.Value);
            }
            if (comment != null)
            {
                VkAPI.Manager.Params("comment", comment);
            }
            if (commentVisible != null)
            {
                VkAPI.Manager.Params("comment_visible", commentVisible.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return apiManager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Убирает пользователя из черного списка сообщества.
        /// </summary>
        /// <param name="gid">Идентификатор сообщества. </param>
        /// <param name="userId">Идентификатор пользователя, которого нужно убрать из черного списка. </param>
        /// <returns>После успешного выполнения возвращает true.</returns>
        public static async Task<bool> UnbanUser(int gid, int uid)
        {
            VkAPI.Manager.Method("groups.unbanUser");
            VkAPI.Manager.Params("group_id", gid);
            VkAPI.Manager.Params("user_id", uid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return apiManager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Возвращает список забаненных пользователей в сообществе.
        /// </summary>
        /// <param name="gid">Идентификатор сообщества. </param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества черного списка. </param>
        /// <param name="count">Количество записей, которое необходимо вернуть. </param>
        /// <param name="fields">Cписок дополнительных полей, которые необходимо вернуть. 
        /// Доступные значения: sex, bdate, city, country, photo_50, photo_100, photo_200_orig, photo_200, photo_400_orig, photo_max, photo_max_orig, online, online_mobile, lists, domain, has_mobile, contacts, connections, site, education, universities, schools, can_post, can_see_all_posts, can_see_audio, can_write_private_message, status, last_seen, common_count, relation, relatives, counters 
        /// </param>
        public static async Task<ListCount<BannedUser>> GetBanned(int gid, int? offset = null, int? count = null, IEnumerable<string> fields = null)
        {
            VkAPI.Manager.Method("groups.getBanned");
            VkAPI.Manager.Params("group_id", gid);
            if (offset != null) VkAPI.Manager.Params("offset", offset);
            if (count != null) VkAPI.Manager.Params("count", count);
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + "," + b));
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed
                       ? new ListCount<BannedUser>(result.Int("count").Value, result.SelectNodes("items/*").Cast<XmlNode>().Select(x => new BannedUser(x)).ToList())
                       : null;
        }
    }
}