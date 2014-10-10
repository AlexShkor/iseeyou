using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Account
{
    /// <summary>
    /// Информация об аккакунте
    /// </summary>
    public class AccountInfo
    {
        public AccountInfo(XmlNode node)
        {
            Country = node.String("country");
            HttpsRequired = node.Bool("https_required");
            Intro = node.Int("intro");
        }

        /// <summary>
        /// Строковой код страны, определенный по IP адресу, с которого сделан запрос
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// true - пользователь установил на сайте настройку "Всегда использовать безопасное соединение"; false - безопасное соединение не требуется
        /// </summary>
        public bool? HttpsRequired { get; set; }

        /// <summary>
        /// Битовая маска отвечающая за прохождение обучения использованию приложения
        /// </summary>
        public int? Intro { get; set; }
    }
}
