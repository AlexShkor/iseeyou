using System;
using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Groups
{
    /// <summary>
    /// Информация о пользователе, которого забанили
    /// </summary>
    public class BanInfo
    {
        public BanInfo(XmlNode node)
        {
            if (node == null)
                return;

            AdminId = node.Int("admin_id");
            Date = node.DateTimeFromUnixTime("date");
            Reason = new BanReason((BanReason.BanReasonEnum)node.Int("reason"));
            Comment = node.String("comment");
            EndDate = node.DateTimeFromUnixTime("end_date");
        }

        /// <summary>
        /// ID админа
        /// </summary>
        public int? AdminId { get; set; }
        /// <summary>
        /// Время бана
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Причина бана
        /// </summary>
        public BanReason Reason { get; set; }
        /// <summary>
        /// Комментарий к бану
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Время окончания бана (или 1.1.1970 00:00, если бан навсегда)
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
