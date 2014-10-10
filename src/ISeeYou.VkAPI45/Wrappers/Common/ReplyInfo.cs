#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Информация о ответе
    /// </summary>
    public class ReplyInfo
    {
        /// <summary>
        /// Дата и время ответа
        /// </summary>
        public DateTime? Date;

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int? Id;

        /// <summary>
        /// Текст ответа
        /// </summary>
        public string Text;

        public ReplyInfo(XmlNode node)
        {
            Id = node.Int("id");
            Date = node.DateTimeFromUnixTime("date");
            Text = node.String("text");
        }
    }
}