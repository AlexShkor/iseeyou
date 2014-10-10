using System;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Utils
{
    /// <summary>
    /// Утилиты VkAPI
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Возвращает информацию о том, является ли внешняя ссылка заблокированной на сайте ВКонтакте
        /// </summary>
        /// <param name="url">Внешняя ссылка, которую необходимо проверить</param>
        /// <returns>Возвращает одно из трёх значений поля status:
        /// not_banned – ссылка не заблокирована
        /// banned – ссылка заблокирована
        /// processing – ссылка проверяется; необходимо выполнить повторный запрос через несколько секунд.</returns>
        public static async Task<string> CheckLink(string url)
        {
            VkAPI.Manager.Method("utils.checkLink");
            VkAPI.Manager.Params("url", url);

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? result.String("status") : null;
        }

        /// <summary>
        /// Определяет тип объекта (пользователь, сообщество, приложение) и его идентификатор по короткому имени screen_name
        /// </summary>
        /// <param name="screenName">Короткое имя пользователя, группы или приложения. Например, apiclub, andrew или rules_of_war</param>
        public static async Task<ObjectIdType> ResolveScreenName(string screenName)
        {
            VkAPI.Manager.Method("utils.resolveScreenName");
            VkAPI.Manager.Params("screen_name", screenName);

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new ObjectIdType(result) : null;
        }

        /// <summary>
        /// Возвращает текущее время на сервере ВКонтакте
        /// </summary>
        public static async Task<DateTime> GetServerTime()
        {
            VkAPI.Manager.Method("utils.getServerTime");

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            return result.DateTimeVal().Value;
        }
    }

    /// <summary>
    /// Результат метода Utils.ResolveScreenName
    /// </summary>
    public class ObjectIdType
    {
        public ObjectIdType(XmlNode node)
        {
            Type = node.String("type");
            ObjectId = node.Int("object_id");
        }

        /// <summary>
        ///  Тип объекта (user, group, application)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        public int? ObjectId { get; set; }
    }
}
