#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    ///     Представляет информацию о активности Вконтакте
    /// </summary>
    public class ActivityInfo
    {
        public ActivityInfo(XmlNode node)
        {
            Online = node.Bool("online");
            Time = node.DateTimeFromUnixTime("time");
        }

        /// <summary>
        /// Время последней активности
        /// </summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// Текущий статус пользователя (true — в сети, false — не в сети)
        /// </summary>
        public bool? Online { get; set; }
    }
}