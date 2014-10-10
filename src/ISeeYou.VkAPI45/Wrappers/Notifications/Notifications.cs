#region Using

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Notifications
{
    /// <summary>
    /// Уведомления
    /// </summary>
    public static class Notifications
    {
        /// <summary>
        ///     Возвращает список оповещений об ответах других пользователей на записи текущего пользователя.
        /// </summary>
        /// <param name="filters">Перечисленные через запятую типы оповещений, которые необходимо получить.</param>
        /// <param name="startTime">Время, в формате unixtime, начиная с которого следует получить оповещения для текущего пользователя. Если параметр не задан, то он считается равным значению времени, которое было сутки назад.</param>
        /// <param name="endTime">Время, в формате unixtime, до которого следует получить оповещения для текущего пользователя. Если параметр не задан, то он считается равным текущему времени.</param>
        /// <param name="offset">Смещение, начиная с которого следует вернуть список оповещений.</param>
        /// <param name="count">Указывает, какое максимальное число оповещений следует возвращать, но не более 100. По умолчанию 30.</param>
        /// <param name="from">Строковый идентификатор последнего полученного предыдущим вызовом метода оповещения (см. описание поля new_from в результате)</param>
        public static async Task<NotificationsClassified> Get(IEnumerable<NotificationFilterType> filters = null, string from = null,
                                                  int? startTime = null, int? endTime = null, int? offset = null,
                                                  int? count = null)
        {
            VkAPI.Manager.Method("notifications.get");
            if (filters != null)
                VkAPI.Manager.Params("filters", filters.Select(x => x.Value).Aggregate((a, b) => a + "," + b));
            if (startTime != null)
                VkAPI.Manager.Params("start_time", startTime);
            if (endTime != null)
                VkAPI.Manager.Params("end_time", endTime);
            if (count != null)
                VkAPI.Manager.Params("count", count);
            if (offset != null)
                VkAPI.Manager.Params("offset", offset);

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new NotificationsClassified(result) : null;
        }

        /// <summary>
        ///     Сбрасывает счетчик непросмотренных оповещений об ответах других пользователей на записи текущего пользователя.
        /// </summary>
        /// <returns>Если у пользователя присутствовали непросмотренные ответы, возвращает true в случае успешного завершения. В противном случае возвращает false. </returns>
        public static async Task<bool> MarkAsViewed()
        {
            VkAPI.Manager.Method("notifications.markAsViewed");
            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }
    }
}