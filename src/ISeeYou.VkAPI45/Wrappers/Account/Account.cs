#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Account
{
    /// <summary>
    /// Действия с аккаунтом пользователя
    /// </summary>
    public static class Account
    {
        /// <summary>
        /// Возвращает ненулевые значения счетчиков пользователя.
        /// </summary>
        /// <param name="filter">Счетчики, информацию о которых нужно вернуть (friends, messages, photos, videos, notes, gifts, events, groups, notifications). 
        /// Список строк, разделенных через запятую</param>
        public static async Task<AccountCounters> GetCounters(IEnumerable<string> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            VkAPI.Manager.Method("account.getCounters");
            VkAPI.Manager.Params("filter", filter.Aggregate((a, b) => a + "," + b));

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return new AccountCounters(result);
            }
            return null;
        }

        /// <summary>
        /// Устанавливает короткое название приложения (до 17 символов), которое выводится пользователю в левом меню.
        /// Это происходит только в том случае, если пользователь добавил приложение в левое меню со страницы приложения, списка приложений или настроек.
        /// </summary>
        /// <param name="name">Короткое название приложения</param>
        /// <returns></returns>
        public static async Task<bool> SetName(string name)
        {
            VkAPI.Manager.Method("account.setName");

            if(name != null)
                VkAPI.Manager.Params("name", name);

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Помечает текущего пользователя как online на 15 минут.
        /// </summary>
        /// <returns>В случае успешного выполнения метода будет возвращёно true. </returns>
        public static async Task<bool> SetOnline(bool? voip = null)
        {
            VkAPI.Manager.Method("account.setOnline");

            if (voip != null)
            {
                VkAPI.Manager.Params("voip", voip.Value ? 1 : 0);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Помечает текущего пользователя как offline
        /// </summary>
        /// <returns>В случае успешного выполнения метода будет возвращёно true. </returns>
        public static async Task<bool> SetOffline()
        {
            VkAPI.Manager.Method("account.setOffline");

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Позволяет искать пользователей ВКонтакте, используя телефонные номера, email-адреса, и идентификаторы пользователей в других сервисах. Найденные пользователи могут быть также в дальнейшем получены методом friends.getSuggestions.
        /// </summary>
        /// <param name="contacts">Список контактов, разделенных через запятую </param>
        /// <param name="service">Строковой идентификатор сервиса, по контактам которого производится поиск</param>
        /// <param name="myContact">Контакт текущего пользователя в заданном сервисе</param>
        /// <param name="returnAll">true – возвращать также контакты, найденные ранее используя этот сервис, false – возвращать только контакты, найденные используя поле contacts. </param>
        public static async Task<List<ContactUser>> LookupContacts(IEnumerable<string> contacts, ContactService service, string myContact = null, bool? returnAll = null)
        {
            VkAPI.Manager.Method("account.lookupContacts");

            if (contacts == null || !contacts.Any())
            {
                throw new ArgumentException("contacts == null || contacts.Count() == 0");
            }

            VkAPI.Manager.Params("contacts", contacts.Aggregate((a, b) => a + "," + b));
            VkAPI.Manager.Params("service", service.StringValue);

            if (myContact != null)
            {
                VkAPI.Manager.Params("my_contact", myContact);
            }
            if (returnAll != null)
            {
                VkAPI.Manager.Params("return_all", returnAll.Value ? 1 : 0);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("found/*");
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => new ContactUser(x)).ToList();
            }
            return null;
        }

        /// <summary>
        /// Возвращает список активных рекламных предложений (офферов), выполнив которые пользователь сможет получить соответствующее количество голосов на свой счёт внутри приложения.
        /// </summary>
        public static async Task<ListCount<AdOffer>> GetActiveOffers()
        {
            VkAPI.Manager.Method("account.getActiveOffers");

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<AdOffer>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new AdOffer(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Добавляет пользователя в черный список
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, которого нужно добавить в черный список</param>
        /// <returns>В случае успеха метод вернет true</returns>
        public static async Task<bool> BanUser(int userId)
        {
            VkAPI.Manager.Method("account.banUser");
            VkAPI.Manager.Params("user_id", userId);

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Убирает пользователя из черного списка
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, которого нужно убрать из черного списка</param>
        /// <returns>В случае успеха метод вернет true</returns>
        public static async Task<bool> UnbanUser(int userId)
        {
            VkAPI.Manager.Method("account.unbanUser");
            VkAPI.Manager.Params("user_id", userId);

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Получает настройки текущего пользователя в данном приложении
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, информацию о настройках которого необходимо получить. По умолчанию — текущий пользователь.</param>
        /// <returns>После успешного выполнения возвращает битовую маску настроек текущего пользователя в данном приложении</returns>
        public static async Task<int> GetAppPermissions(int? uid = null)
        {
            VkAPI.Manager.Method("account.getAppPermissions");
            if (uid != null)
            {
                VkAPI.Manager.Params("uid", uid);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        /// Возвращает список пользователей, находящихся в черном списке
        /// </summary>
        /// <param name="count">Количество записей, которое необходимо вернуть</param>
        /// <param name="offset">Смещение необходимое для выборки определенного подмножества черного списка</param>
        public static async Task<ListCount<BaseUser>> GetBanned(int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("account.getBanned");
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<BaseUser>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new BaseUser(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает информацию о текущем аккаунте
        /// </summary>
        public static async Task<AccountInfo> GetInfo()
        {
            VkAPI.Manager.Method("account.getInfo");

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return new AccountInfo(result);
            }
            return null;
        }

        /// <summary>
        /// Позволяет редактировать информацию о текущем аккаунте
        /// </summary>
        /// <param name="intro">Битовая маска, отвечающая за прохождение обучения в мобильных клиентах</param>
        /// <returns>В случае успеха метод вернет true</returns>
        public static async Task<bool> SetInfo(int intro)
        {
            VkAPI.Manager.Method("account.setInfo");
            VkAPI.Manager.Params("intro", intro);

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Позволяет сменить пароль пользователя после успешного восстановления доступа к аккаунту через СМС, используя метод auth.restore.
        /// </summary>
        /// <param name="oldPass">Текущий пароль пользователя. </param>
        /// <param name="newPass">Новый пароль, который будет установлен в качестве текущего.</param>
        /// <param name="restoreSid">Идентификатор сессии, полученный при восстановлении доступа используя метод auth.restore. (В случае если пароль меняется сразу после восстановления доступа) </param>
        /// <param name="changePassHash">Хэш, полученный при успешной OAuth авторизации по коду полученному по СМС (В случае если пароль меняется сразу после восстановления доступа) </param>
        public static async Task<ChangePasswordResult> ChangePassword(string oldPass, string newPass, string restoreSid = null, string changePassHash = null)
        {
            if (oldPass == null | newPass == null)
            {
                throw new ArgumentNullException();
            }

            VkAPI.Manager.Method("account.changePassword");
            VkAPI.Manager.Params("old_password", oldPass);
            VkAPI.Manager.Params("new_password", newPass);
            if (restoreSid != null)
            {
                VkAPI.Manager.Params("restore_sid", restoreSid);
            }
            if (changePassHash != null)
            {
                VkAPI.Manager.Params("change_password_hash", changePassHash);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new ChangePasswordResult(result) : null;
        }

        /// <summary>
        /// Возвращает информацию о текущем профиле
        /// </summary>
        public static async Task<ProfileInfo> GetProfileInfo()
        {
            VkAPI.Manager.Method("account.getProfileInfo");

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new ProfileInfo(result) : null;
        }

        /// <summary>
        /// Редактирует информацию текущего профиля
        /// </summary>
        /// <param name="firstName">Имя пользователя</param>
        /// <param name="lastName">Фамилия пользователя</param>
        /// <param name="maidenName">Девичья фамилия пользователя</param>
        /// <param name="cancelRequestId">Идентификатор заявки на смену имени, которую необходимо отменить. 
        /// Если передан этот параметр, все остальные параметры игнорируются. </param>
        /// <param name="sex">Пол пользователя</param>
        /// <param name="relation">Семейное положение пользователя</param>
        /// <param name="relationPartnerId">Идентификатор пользователя, с которым связано семейное положение</param>
        /// <param name="bDate">Дата рождения пользователя в формате DD.MM.YYYY, например "15.11.1984"</param>
        /// <param name="bDateVisibility">Видимость даты рождения. Возможные значения:
        /// 1 — показывать дату рождения;
        /// 2 — показывать только месяц и день;
        /// 0 — не показывать дату рождения</param>
        /// <param name="hometown">Родной город пользователя</param>
        /// <param name="country">Идентификатор страны пользователя</param>
        /// <param name="city">Идентификатор города пользователя</param>
        public static async Task<SetProfileInfoResult> SetProfileInfo(string firstName = null, string lastName = null, string maidenName = null,
                                                          int? cancelRequestId = null, UserSex sex = null, UserMaritalStatus relation = null,
                                                          int? relationPartnerId = null, string bDate = null, int? bDateVisibility = null,
                                                          string hometown = null, int? country = null, int? city = null)
        {
            VkAPI.Manager.Method("account.setProfileInfo");

            if (firstName != null)
            {
                VkAPI.Manager.Params("first_name", firstName);
            }
            if (lastName != null)
            {
                VkAPI.Manager.Params("last_name", lastName);
            }
            if (maidenName != null)
            {
                VkAPI.Manager.Params("maiden_name", maidenName);
            }
            if (cancelRequestId != null)
            {
                VkAPI.Manager.Params("cancel_request_id", cancelRequestId);
            }
            if (sex != null)
            {
                VkAPI.Manager.Params("sex", sex.Value);
            }
            if (relation != null)
            {
                VkAPI.Manager.Params("relation", relation.Value);
            }
            if (relationPartnerId != null)
            {
                VkAPI.Manager.Params("relation_partner_id", relationPartnerId);
            }
            if (bDate != null)
            {
                VkAPI.Manager.Params("bdate", bDate);
            }
            if (bDateVisibility != null)
            {
                VkAPI.Manager.Params("bdate_visibility", bDateVisibility);
            }
            if (hometown != null)
            {
                VkAPI.Manager.Params("home_town", hometown);
            }
            if (city!= null)
            {
                VkAPI.Manager.Params("city_id", city);
            }
            if (country != null)
            {
                VkAPI.Manager.Params("country_id", country);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new SetProfileInfoResult(result) : null;
        }
    }

    /// <summary>
    /// Результат метода account.setProfileInfo
    /// </summary>
    public class SetProfileInfoResult
    {
        public SetProfileInfoResult(XmlNode node)
        {
            Changed = node.Bool("changed");
            if (node.SelectSingleNode("name_request") != null)
            {
                NameRequest = new NameRequest(node.SelectSingleNode("name_request"));
            }
        }

        /// <summary>
        ///  true — если информация была сохранена, false — если ни одно из полей не было сохранено.
        /// </summary>
        public bool? Changed { get; set; }

        /// <summary>
        /// Объект, содержащий информацию о заявке на смену имени
        /// </summary>
        public NameRequest NameRequest { get; set; }
    }

    /// <summary>
    /// Результат метода account.changePassword
    /// </summary>
    public class ChangePasswordResult
    {
        public ChangePasswordResult(XmlNode node)
        {
            Token = node.String("token");
            Secret = node.String("secret");
        }

        /// <summary>
        /// Новый токен
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// Возвращается в случае, если токен был nohttps
        /// </summary>
        string Secret { get; set; }
    }
}