#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common.AttachmentTypes;
using VkAPIAsync.Wrappers.Common.Factories;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Медиавложение на стене
    /// </summary>
    public class WallAttachment
    {
        public WallAttachment(XmlNode node)
        {
            var typeEnum = node.Enum("type", typeof(WallAttachmentType.WallAttachmentTypeEnum));
            if (typeEnum != null)
            {
                Type = new WallAttachmentType((WallAttachmentType.WallAttachmentTypeEnum)typeEnum);
            }
           
            Xml = node.InnerXml;
            XmlNode attachmentData = node.SelectSingleNode(Type.Value);
            Data = attachmentData != null ? Attachments.GetAttachment(Type, attachmentData) : null;
        }

        /// <summary>
        /// XML-представление вложения
        /// </summary>
        public string Xml { get; set; }

        /// <summary>
        /// Тип вложения
        /// </summary>
        public WallAttachmentType Type { get; set; }

        /// <summary>
        /// Информация о вложении
        /// </summary>
        public AttachmentData Data { get; set; }
    }

    /// <summary>
    /// Медиавложение в ЛС
    /// </summary>
    public class MessageAttachment
    {
        public MessageAttachment(XmlNode node)
        {
            Type = new MessageAttachmentType((MessageAttachmentType.MessageAttachmentTypeEnum)node.Enum("type", typeof(MessageAttachmentType.MessageAttachmentTypeEnum)));
            Xml = node.InnerXml;
            XmlNode attachmentData = node.SelectSingleNode(Type.Value);
            Data = attachmentData != null ? Attachments.GetAttachment(Type, attachmentData) : null;
        }

        /// <summary>
        /// XML-представление вложения
        /// </summary>
        public string Xml { get; set; }

        /// <summary>
        /// Тип вложения
        /// </summary>
        public MessageAttachmentType Type { get; set; }

        /// <summary>
        /// Информация о вложении
        /// </summary>
        public AttachmentData Data { get; set; }
    }
}