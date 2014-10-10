#region Using

using System;
using System.Xml;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Messages
{
    /// <summary>
    /// Результат метода messages.searchDialogs
    /// </summary>
    public class DialogSearchItem
    {
        public DialogSearchItem(XmlNode node)
        {
            var type = node.SelectSingleNode("type");
            if (type == null) return;
            ItemType = type.InnerText;
            switch (ItemType)
            {
                case "profile":
                    Value = new User(node);
                    ValueType = typeof (User);
                    break;
                case "chat":
                    Value = new ChatInfo(node);
                    ValueType = typeof (ChatInfo);
                    break;
            }
        }

        /// <summary>
        /// Значение
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Тип обьекта
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// Тип значения
        /// </summary>
        public Type ValueType { get; set; }
    }
}