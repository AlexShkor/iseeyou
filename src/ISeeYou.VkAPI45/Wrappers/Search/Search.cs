using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Search
{
    /// <summary>
    /// Поиск
    /// </summary>
    public static class Search
    {
        /// <summary>
        /// Метод позволяет получить результаты быстрого поиска по произвольной подстроке
        /// </summary>
        /// <param name="q">Текст запроса, результаты которого нужно получить</param>
        /// <param name="limit">Ограничение на количество возвращаемых результатов</param>
        /// <param name="filters">Перечисленные через запятую типы данных, которые необходимо вернуть</param>
        /// <param name="searchGlobal">По-умолчанию к результатам поиска добавляются результаты глобального поиска по всем пользователям и группам, это можно отключить передав false флаг, может принимать значения true или false, по умолчанию true</param>
        public static async Task<List<SearchHint>> GetHints(string q, int? limit = null, HintFilters filters = null, bool? searchGlobal = null)
        {
            if (q == null)
            {
                throw new ArgumentNullException("q");
            }

            VkAPI.Manager.Method("search.getHints");
            VkAPI.Manager.Params("q", q);

            if (limit != null)
            {
                VkAPI.Manager.Params("limit", limit);
            }
            if (filters != null)
            {
                VkAPI.Manager.Params("filters", filters.ToString());
            }
            if (searchGlobal != null)
            {
                VkAPI.Manager.Params("limit", searchGlobal.Value ? 1 : 0);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("/*");
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => x.String("type") == "group" ? (SearchHint)new GroupSearchHint(x) : (SearchHint)new UserSearchHint(x)).ToList();
            }
            return null;
        }
    }
}
