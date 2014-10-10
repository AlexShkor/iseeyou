#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

#endregion

namespace VkAPIAsync.Wrappers.Stats
{
    /// <summary>
    /// Статистика
    /// </summary>
    public static class Stats
    {
        /// <summary>
        ///     Возвращает статистику группы или приложения.
        /// </summary>
        /// <param name="gid">ID группы, статистику которой необходимо получить.</param>
        /// <param name="aid">ID приложения, статистику которой необходимо получить.</param>
        /// <param name="fromDate">Начальная дата выводимой статистики в формате YYYY-MM-DD, пример: 2011-09-27 - 27 сентября 2011</param>
        /// <param name="toDate">Конечная дата выводимой статистики в формате YYYY-MM-DD, пример: 2011-09-27 - 27 сентября 2011</param>
        /// <returns>Статистика по дням</returns>
        public static async Task<List<Period>> Get(int? gid, int? aid, DateTime fromDate, DateTime toDate)
        {
            if (fromDate == null)
            {
                throw new ArgumentNullException("fromTime");
            }
            if (toDate == null)
            {
                throw new ArgumentNullException("toTime");
            }
            if (gid != null & aid != null)
            {
                throw new Exception("Невозможно использовать метод одновременно и с gid и с aid");
            }
            if (gid == null & aid == null)
            {
                throw new ArgumentNullException("Один параметр должен быть указан: gid или aid");
            }

            VkAPI.Manager.Method("stats.get");
            if (gid != null)
            {
                VkAPI.Manager.Params("group_id", gid);
            }
            if (aid != null)
            {
                VkAPI.Manager.Params("app_id", aid);
            }
            VkAPI.Manager.Params("date_from", fromDate.ToString("yyyy'-'MM'-'dd"));
            VkAPI.Manager.Params("date_to", toDate.ToString("yyyy'-'MM'-'dd"));

            var apiManager = await VkAPI.Manager.Execute();
            var resp = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = resp
                     .ChildNodes.Cast<XmlNode>()
                     .Where(el => el.NodeType == XmlNodeType.Element && el.LocalName == "period");
                return nodes.Select(node => new Period(node)).ToList();
            }
            return null;
        }
    }
}