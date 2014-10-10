using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Users;

namespace VkAPIAsync.Wrappers.Account
{
    /// <summary>
    /// Пользователь, найденный по контактам
    /// </summary>
    public class ContactUser : BaseUser
    {
        public ContactUser(XmlNode node) : base(node)
        {
            Contact = node.String("contact");
            RequestSent = node.Bool("request_sent");
            CommonCount = node.Int("common_count");
        }

        /// <summary>
        ///  Контакт, по которому был найден пользователь (не приходит если пользователь был найден при предыдущем использовании метода)
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Запрос на добавление в друзья уже был выслан, либо пользователь уже является другом
        /// </summary>
        public bool? RequestSent { get; set; }

        /// <summary>
        /// Если этот контакт также был импортирован друзьями или контактами текущего пользователя; количество общих друзей
        /// </summary>
        public int? CommonCount { get; set; }
    }
}
