#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// Информация для подключения к LongPoll серверу
    /// </summary>
    public class LongPollServerConnectionInfo
    {
        public LongPollServerConnectionInfo(XmlNode node)
        {
            Key = node.String("key");
            Server = node.String("server");
            Ts = node.Int("ts");
        }

        /// <summary>
        ///  Секретный ключ сессии
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///  Адрес сервера к которому нужно отправлять запрос
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        ///  Номер последнего события, начиная с которого Вы хотите получать данные
        /// </summary>
        public int? Ts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Pts { get; set; }
    }
}