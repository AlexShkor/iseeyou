#region Using

using System.Threading.Tasks;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Status
{
    /// <summary>
    /// Статус
    /// </summary>
    public static class Status
    {
        /// <summary>
        ///     Получает текст статуса пользователя.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, статус которого необходимо получить. Если параметр не задан, то считается, что он равен идентификатору текущего пользователя.</param>
        public static async Task<StatusInfo> Get(int? userId = null)
        {
            VkAPI.Manager.Method("status.get");
            if (userId != null)
            {
                VkAPI.Manager.Params("user_id", userId);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            return new StatusInfo(resp);
        }

        /// <summary>
        ///     Устанавливает новый статус текущему пользователю.
        /// </summary>
        /// <param name="status">Текст статуса, который необходимо установить текущему пользователю. Если параметр не задан или равен пустой строке, то статус текущего пользователя будет очищен.</param>
        /// <returns>В случае успешной установки или очистки статуса возвращает true. </returns>
        public static async Task<bool> Set(string status = null)
        {
            VkAPI.Manager.Method("status.set");
            if (status != null)
            {
                VkAPI.Manager.Params("text", status);
            }

            var resp = (await VkAPI.Manager.Execute()).GetResponseXml();
            return resp.BoolVal().Value;
        }
    }
}