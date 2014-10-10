#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Friends
{
    /// <summary>
    /// Друзья
    /// </summary>
    public static class Friends
    {
        /// <summary>
        /// Возвращает список идентификаторов друзей
        /// </summary>
        /// <param name="node">Xml-элемент, который содержит список</param>
        internal static List<int> BuildFriendsList(IEnumerable<XmlNode> nodes)
        {
            return nodes != null && nodes.Any() ? nodes.Select(n => n.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList() : null;
        }

        /// <summary>
        /// Возвращает список сущностей друзей
        /// </summary>
        /// <param name="node">Xml-элемент, который содержит список</param>
        internal static ListCount<User> BuildFriendsObjectsList(XmlNode x)
        {
            XmlNodeList msgsNodes = x.SelectNodes("//user");
            return msgsNodes.Count > 0 ? new ListCount<User>(x.Int("count").Value, msgsNodes.Cast<XmlNode>().Select(y => new User(y)).ToList()) : null;
        }

        /// <summary>
        ///     Возвращает список идентификаторов друзей пользователя или расширенную информацию о друзьях пользователя (при использовании параметра fields).
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, для которого необходимо получить список друзей. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя.</param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя. Возможные значения: именительный – nom, родительный – gen, дательный – dat, винительный – acc, творительный – ins, предложный – abl. По умолчанию nom.</param>
        /// <param name="count">Количество друзей, которое нужно вернуть. (по умолчанию – все друзья)</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества друзей.</param>
        /// <param name="listId">Идентификатор списка друзей, полученный методом friends.getLists, друзей из которого необходимо получить. Данный параметр учитывается, только когда параметр userId равен идентификатору текущего пользователя.</param>
        /// <param name="fields">Перечисленные через запятую поля анкет, необходимые для получения. Доступные значения: userId, first_name, last_name, nickname, sex, bdate (birthdate), city, country, timezone, photo, photo_medium, photo_big, domain, has_mobile, rate, contacts, education.</param>
        /// <param name="order">Порядок в котором нужно вернуть список друзей. Допустимые значения: name - сортировать по имени (работает только при переданном параметре fields). hints - сортировать по рейтингу, аналогично тому, как друзья сортируются в разделе Мои друзья </param>
        public static async Task<ListCount<User>> Get(int? uid = null, IEnumerable<string> fields = null, string nameCase = null, int? listId = null,
                                              int? offset = null, int? count = null, FriendsResultOrder order = null)
        {
            VkAPI.Manager.Method("friends.get");
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
            if (listId != null)
            {
                VkAPI.Manager.Params("list_id", listId);
            }
            if (fields != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a,b) => a + "," + b));
            }
            if (order != null)
            {
                VkAPI.Manager.Params("order", order.Value);
            }
            var apiManager = await VkAPI.Manager.Execute();
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (!resp.HasChildNodes)
                {
                    return null;
                }
                return fields != null && fields.Any() ? 
                       BuildFriendsObjectsList(resp) : 
                       new ListCount<User>(resp.Int("count").Value, BuildFriendsList(resp.SelectNodes("items/user_id").Cast<XmlNode>()).Select(x => new User(x)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список идентификаторов друзей текущего пользователя, которые установили данное приложение.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<int>> GetAppUsers()
        {
            VkAPI.Manager.Method("friends.getAppUsers");

            var apiManager = await VkAPI.Manager.Execute();
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : BuildFriendsList(resp.SelectNodes("user_id").Cast<XmlNode>());
            }
            return null;
        }

        /// <summary>
        /// Возвращает список идентификаторов друзей пользователя, находящихся на сайте.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, для которого необходимо получить список друзей онлайн. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя. </param>
        /// <param name="lid">Идентификатор списка друзей. Если параметр не задан, возвращается информация обо всех друзьях, находящихся на сайте. </param>
        /// <param name="onlineMobile">true — будет возвращено дополнительное поле online_mobile. </param>
        /// <param name="order">Порядок, в котором нужно вернуть список друзей, находящихся на сайте. Допустимые значения: random - возвращает друзей в случайном порядке, hints - сортировать по рейтингу, аналогично тому, как друзья сортируются в разделе Мои друзья (данный параметр доступен только для Desktop-приложений). </param>
        /// <param name="count">Количество друзей онлайн, которое нужно вернуть. (по умолчанию – все друзья онлайн) </param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества друзей онлайн. </param>
        public static async Task<GetOnlineResult> GetOnline(int? uid = null, int? lid = null, bool? onlineMobile = null, RandomUsersResultOrder order = null, int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("friends.getOnline");

            if (uid != null)
            {
                VkAPI.Manager.Params("user_id", uid);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (lid != null)
            {
                VkAPI.Manager.Params("list_id", lid);
            }
            if (onlineMobile != null)
            {
                VkAPI.Manager.Params("online_mobile", onlineMobile.Value ? 1 : 0);
            }
            if (order != null)
            {
                VkAPI.Manager.Params("order", order.Value);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var res = new GetOnlineResult();
                if (!resp.HasChildNodes)
                {
                    return null;
                }

                //Когда onlineMobile равен false или null, в корне ответа возвращается список пользователей, которые онлайн с десктопов
                var onlineNodes = resp.SelectNodes(onlineMobile.HasValue && onlineMobile.Value ? "online/*" : "*");
                if (onlineNodes.Count > 0)
                    res.Online = onlineNodes.Cast<XmlNode>().Select(y => y.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();

                if (onlineMobile.HasValue && onlineMobile.Value)
                {
                    var onlineMobileNodes = resp.SelectNodes("online_mobile/*");
                    if (onlineMobileNodes.Count > 0)
                        res.OnlineMobile = onlineMobileNodes.Cast<XmlNode>().Select(y => y.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
                }

                return res;
            }
            return null;
        }

        /// <summary>
        /// Возвращает список идентификаторов общих друзей между парой пользователей.
        /// </summary>
        /// <param name="targetUid">Идентификатор пользователя, с которым необходимо искать общих друзей.</param>
        /// <param name="sourceUid">Идентификатор пользователя, чьи друзья пересекаются с друзьями пользователя с идентификатором target_uid. Если параметр не задан, то считается, что source_uid равен идентификатору текущего пользователя. </param>
        /// <param name="random">true - возвращает друзей в случайном порядке</param>
        /// <param name="count">Количество общих друзей, которое нужно вернуть. (по умолчанию – все общие друзья) </param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества общих друзей. </param>
        public static async Task<List<int>> GetMutual(int targetUid, int? sourceUid = null, bool random = false, int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("friends.getMutual");

            VkAPI.Manager.Params("target_uid", targetUid);
            if (sourceUid != null)
            {
                VkAPI.Manager.Params("source_uid", sourceUid);
            }
            if (random)
                VkAPI.Manager.Params("order", "random");
            if(count != null)
                VkAPI.Manager.Params("count", count);
            if (offset != null)
                VkAPI.Manager.Params("offset", offset);

            var apiManager = await VkAPI.Manager.Execute();
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : BuildFriendsList(resp.SelectNodes("user_id").Cast<XmlNode>());
            }
            return null;
        }

        /// <summary>
        ///     Возвращает информацию о том добавлен ли текущий пользователь в друзья у указанных пользователей.
        ///     Также возвращает информацию о наличии исходящей или входящей заявки в друзья (подписки).
        /// </summary>
        /// <param name="uids">Список идентификаторов пользователей, раделённых запятыми, статус дружбы с которыми необходимо получить.</param>
        /// <paparam name="needSign"></paparam>
        public static async Task<List<FriendStatus>> AreFriends(IEnumerable<int> uids, bool? needSign = null)
        {
            VkAPI.Manager.Method("friends.areFriends");

            VkAPI.Manager.Params("user_ids", uids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));

            if (needSign != null)
            {
                VkAPI.Manager.Params("need_sign", needSign);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : resp.SelectNodes("//status").Cast<XmlNode>().Select(y => new FriendStatus(y)).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список меток друзей текущего пользователя.
        /// </summary>
        /// <returns>Возвращает массив меток друзей текущего пользователя, каждый из объектов которого содержит поля lid и name. </returns>
        public static async Task<ListCount<ListInfo>> GetLists()
        {
            VkAPI.Manager.Method("friends.getLists");

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return new ListCount<ListInfo>(resp.Int("count").Value, resp.SelectNodes("//list").Cast<XmlNode>().Select(y => new ListInfo(y)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Создает новый список друзей у текущего пользователя.
        /// </summary>
        /// <param name="name">Название создаваемого списка друзей.</param>
        /// <param name="uids">Перечисленные через запятую идентификаторы друзей пользователя, которых необходимо включить в создаваемый список. Идентификаторы пользователей, не являющихся друзьями текущего пользователя, игнорируются.</param>
        /// <returns>В случае успеха возвращает идентификатор (lid) созданного списка друзей. </returns>
        public static async Task<int> AddList(string name, IEnumerable<int> uids = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            VkAPI.Manager.Method("friends.addList");

            VkAPI.Manager.Params("name", name);
            if (uids != null)
            {
                VkAPI.Manager.Params("user_ids", uids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Редактирует существующий список друзей текущего пользователя.
        /// </summary>
        /// <param name="lid">Идентификатор существующего списка друзей.</param>
        /// <param name="name">Название списка друзей.</param>
        /// <param name="uids">Перечисленные через запятую идентификаторы друзей пользователя, которым необходимо поставить метку. Идентификаторы пользователей, не являющихся друзьями текущего пользователя, игнорируются.</param>
        /// <returns>В случае успеха возвращает true. </returns>
        public static async Task<bool> EditList(int lid, string name, IEnumerable<int> uids = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            VkAPI.Manager.Method("friends.editList");
            VkAPI.Manager.Params("list_id", lid);
            VkAPI.Manager.Params("name", name);
            if (uids != null)
            {
                VkAPI.Manager.Params("user_ids", uids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Если идентификатор выбранного пользователя присутствует в списке заявок на добавление в друзья, полученным методом friends.getRequests, то одобряет заявку на добавление и добавляет выбранного пользователя в друзья к текущему пользователю. В противном случае создает заявку на добавление в друзья текущего пользователя к выбранному пользователю.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя которому необходимо отправить заявку, либо заявку от которого необходимо одобрить.</param>
        /// <param name="text">Текст сопроводительного сообщения для заявки на добавление в друзья. Максимальная длина сообщения - 500 символов.</param>
        /// <returns>В случае успешной отправки заявки на добавление в друзья возвращает 1. В случае успешного одобрения заявки на добавление в друзья в возвращает 2. В случае повторной отправки заявки возвращает 4. </returns>
        public static async Task<int> Add(int uid, string text = null)
        {
            VkAPI.Manager.Method("friends.add");

            VkAPI.Manager.Params("user_id", uid);
            if (text != null)
            {
                VkAPI.Manager.Params("text", text);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Если идентификатор выбранного пользователя присутствует в списке заявок на добавление в друзья, полученным методом friends.getRequests, то отклоняет заявку на добавление в друзья к текущему пользователю. В противном случае удаляет выбранного пользователя из списка друзей текущего пользователя, который может быть получен методом friends.get.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, которого необходимо удалить из списка друзей, либо заявку от которого необходимо отклонить.</param>
        /// <returns>В случае успешного удаления пользователя из списка друзей возвращает 1. В случае успешного отклонения заявки на добавление в друзья возвращает 2. В случае успешного удаления рекомендации в друзья возвращает 3. </returns>
        public static async Task<int> Delete(int uid)
        {
            VkAPI.Manager.Method("friends.delete");

            VkAPI.Manager.Params("user_id", uid);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Возвращает информацию о полученных или отправленных заявках на добавление в друзья для текущего пользователя.
        /// </summary>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества заявок на добавление в друзья.</param>
        /// <param name="count">Максимальное количество заявок на добавление в друзья, которые необходимо получить (не более 1000). Если параметр не задан, то считается, что он равен 100.</param>
        /// <param name="extended">Определяет требуется ли возвращать в ответе сообщения от пользователей, подавших заявку на добавление в друзья. И отправителя рекомендации при suggested=1.</param>
        /// <param name="needMutual">Определяет требуется ли возвращать в ответе список общих друзей, если они есть. Обратите внимание, что при использовании need_mutual будет возвращено не более 20 заявок.</param>
        /// <param name="outcoming">false - возвращать полученные заявки в друзья (по умолчанию), true - возвращать отправленные пользователем заявки.</param>
        /// <param name="suggested">true - возвращать рекомендованных другими пользователями друзей, false - возвращать заявки в друзья (по умолчанию).</param>
        /// <param name="order">0 - сортировать по дате добавления, 1 - сортировать по количеству общих друзей. (Если out = true – данный параметр работать не будет.)</param>
        public static async Task<ListCount<RequestInfo>> GetRequests(int? offset = null, int? count = null, bool? extended = null,
                                                    bool? needMutual = null, bool? outcoming = null,
                                                    bool? suggested = null, FriendsSortOrder order = null)
        {
            VkAPI.Manager.Method("friends.getRequests");
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (extended != null)
            {
                VkAPI.Manager.Params("extended", extended.Value ? 1 : 0);
            }
            if (needMutual != null)
            {
                VkAPI.Manager.Params("need_mutual", needMutual.Value ? 1 : 0);
            }
            if (outcoming != null)
            {
                VkAPI.Manager.Params("out", outcoming.Value ? 1 : 0);
            }
            if (suggested != null)
            {
                VkAPI.Manager.Params("suggested", suggested.Value ? 1 : 0);
            }
            if (order != null)
            {
                VkAPI.Manager.Params("sort", order.Value);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return !resp.HasChildNodes
                           ? null
                           : new ListCount<RequestInfo>(resp.Int("count").Value, resp.SelectNodes("//items/*").Cast<XmlNode>().Select(y => new RequestInfo(y)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Отмечает все входящие заявки на добавление в друзья как просмотренные.
        /// </summary>
        /// <returns>Возвращает true в случае, если все заявки на добавление в друзья были успешно отклонены, иначе false. </returns>
        public static async Task<bool> DeleteAllRequests()
        {
            VkAPI.Manager.Method("friends.deleteAllRequests");

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает список профилей пользователей, которые могут быть друзьями текущего пользователя.
        ///     Для того, чтобы данный метод вернул достаточное количество подсказок необходимо предварительно вызвать метод account.importContacts.
        /// </summary>
        /// <param name="filter">
        ///     Типы предлагаемых друзей которые нужно вернуть, перечисленные через запятую.
        ///     Параметр может принимать следующие значения:
        ///     mutual - пользователи, с которыми много общих друзей,
        ///     contacts - пользователи найденные благодаря методу account.importContacts.
        ///     mutual_contacts - пользователи, которые импортировали те же контакты что и текущий пользователь, используя метод account.importContacts.
        ///     По умолчанию будут возвращены все возможные друзья.
        /// </param>
        /// <param name="offset">Cмещение необходимое для выбора определённого подмножества списка.</param>
        /// <param name="count">Количество рекомендаций, которое необходимо вернуть.</param>
        /// <returns>В результате выполнения данного метода, будет возвращён список профилей, которые могут быть предложены пользователю в качестве возможных друзей. </returns>
        public static async Task<ListCount<BaseUser>> GetSuggestions(IEnumerable<string> filter = null, int? offset = null,
                                                     int? count = null, string nameCase = null)
        {
            VkAPI.Manager.Method("friends.getSuggestions");
            if (filter != null)
            {
                VkAPI.Manager.Params("filter", filter.Aggregate((a, b) => a + "," + b));
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (nameCase != null)
            {
                VkAPI.Manager.Params("name_case", nameCase);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return new ListCount<BaseUser>(resp.Int("count").Value, !resp.HasChildNodes
                           ? null
                           : resp.SelectNodes("items/user").Cast<XmlNode>().Select(y => new BaseUser(y)).ToList());
            }
            return null;
        }
    }

    /// <summary>
    /// Результат метода GetOnline
    /// </summary>
    public class GetOnlineResult
    {
        /// <summary>
        /// Список идентификаторов пользователей, которые онлайн
        /// </summary>
        public List<int> Online { get; set; }

        /// <summary>
        /// Список идентификаторов пользователей, которые онлайн с мобильного устройства
        /// </summary>
        public List<int> OnlineMobile { get; set; }
    }
}