#region Using

using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Friends
{
    /// <summary>
    /// Информация о заявке
    /// </summary>
    public class RequestInfo
    {
        public RequestInfo(XmlNode node)
        {
            UserID = node.Int("user_id");
            Message = node.String("message");
            var mutual = node.SelectSingleNode("mutual");
            if (mutual == null) return;
            var nodes = mutual.SelectNodes("users/user_id");
            if (nodes != null && nodes.Count > 0)
            {
                Mutual = new ListCount<int>(nodes.Count, nodes.Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList());
            }
        }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int? UserID { get; set; }

        /// <summary>
        /// Сообщение заявки
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Список идентификаторов общих друзей
        /// </summary>
        public ListCount<int> Mutual { get; set; }
    }
}