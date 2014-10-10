using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Account
{
    /// <summary>
    /// Рекламное предложение
    /// </summary>
    public class AdOffer
    {
        public AdOffer(XmlNode node)
        {
            Id = node.Int("id");
            Title = node.String("title");
            Instruction = node.String("instruction");
            InstructionHtml = node.String("instruction_html");
            ShortDescription = node.String("short_description");
            Description = node.String("description");
            Image = node.String("img");
            Tag = node.String("tag");
            Price = node.Int("price");
        }

        /// <summary>
        /// Идентификатор предложения
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Заголовок
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Инструкция по выполнению
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// Инструкция в HTML формате
        /// </summary>
        public string InstructionHtml { get; set; }

        /// <summary>
        /// Короткое описание
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Полное описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ссылка на изображение предложения
        /// </summary>
        public string Image { get; set; }

        public string Tag { get; set; }

        /// <summary>
        /// Награда за выполнение
        /// </summary>
        public int? Price { get; set; }
    }
}
