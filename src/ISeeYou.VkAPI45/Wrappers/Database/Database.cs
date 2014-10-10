using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

namespace VkAPIAsync.Wrappers.Database
{
    /// <summary>
    /// База данных ВК
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Возвращает список стран
        /// </summary>
        /// <param name="needFull">true — вернуть список всех стран</param>
        /// <param name="codes"> Двухбуквенные коды стран в стандарте ISO 3166-1 alpha-2, для которых необходимо выдать информацию</param>
        /// <param name="offset">Отступ, необходимый для выбора определенного подмножества стран</param>
        /// <param name="count">Количество стран, которое необходимо вернуть</param>
        public static async Task<ListCount<IdTitleObject>> GetCountries(bool? needFull = null, IEnumerable<string> codes = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("database.getCountries");
            if (needFull != null)
            {
                VkAPI.Manager.Params("need_all", needFull.Value ? 1 : 0);
            }
            if (codes != null && codes.Any())
            {
                VkAPI.Manager.Params("code", codes.Aggregate((a, b) => a + "," + b));
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<IdTitleObject>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает список регионов
        /// </summary>
        /// <param name="country">Идентификатор страны, полученный в методе database.getCountries</param>
        /// <param name="q">Строка поискового запроса. Например, Лен</param>
        /// <param name="offset">Отступ, необходимый для выбора определенного подмножества регионов</param>
        /// <param name="count">Количество регионов, которое необходимо вернуть</param>
        public static async Task<ListCount<IdTitleObject>> GetRegions(int country, string q = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("database.getRegions");
            VkAPI.Manager.Params("country_id", country);
            if (q != null)
            {
                VkAPI.Manager.Params("q", q);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<IdTitleObject>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает информацию об улицах по их идентификаторам
        /// </summary>
        /// <param name="sids">Идентификаторы улиц</param>
        public static async Task<List<IdTitleObject>> GetStreetsById(IEnumerable<string> sids)
        {
            VkAPI.Manager.Method("database.getStreetsById");

            if (sids != null && sids.Any())
            {
                VkAPI.Manager.Params("streets_ids", sids.Aggregate((a, b) => a + "," + b));
            }
            else throw new ArgumentException("sids не может быть пустым");

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (result != null && result.ChildNodes.Count > 0)
                    return result.ChildNodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList();
            }
            return null;
        }

        /// <summary>
        /// Возвращает информацию о странах по их идентификаторам
        /// Идентификаторы (id) могут быть получены с помощью методов users.get, places.getById, places.search, places.getCheckins
        /// </summary>
        /// <param name="countryIds">Идентификаторы стран</param>
        public static async Task<List<IdTitleObject>> GetCountriesById(IEnumerable<string> countryIds)
        {
            VkAPI.Manager.Method("database.getCountriesById");

            if (countryIds != null && countryIds.Any())
            {
                VkAPI.Manager.Params("country_ids", countryIds.Aggregate((a, b) => a + "," + b));
            }
            else throw new ArgumentException("countryIds не может быть пустым");

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (result != null && result.ChildNodes.Count > 0)
                    return result.ChildNodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList();
            }
            return null;
        }

        /// <summary>
        /// Возвращает список городов
        /// </summary>
        /// <param name="country">Идентификатор страны, полученный в методе database.getCountries</param>
        /// <param name="region">Идентификатор региона, города которого необходимо получить. (параметр не обязателен) </param>
        /// <param name="q">Строка поискового запроса. Например, Санкт</param>
        /// <param name="needAll">true – возвращать все города. false – возвращать только основные города</param>
        /// <param name="offset">Отступ, необходимый для получения определенного подмножества городов</param>
        /// <param name="count">Количество городов, которые необходимо вернуть</param>
        public static async Task<ListCount<IdTitleObject>> GetCities(int country, int? region = null, string q = null, bool? needAll = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("database.getCities");
            VkAPI.Manager.Params("country_id", country);

            if (region != null)
            {
                VkAPI.Manager.Params("region_id", region);
            }
            if (needAll != null)
            {
                VkAPI.Manager.Params("need_all", needAll.Value ? 1 : 0);
            }
            if (q != null)
            {
                VkAPI.Manager.Params("q", q);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<IdTitleObject>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает информацию о городах по их идентификаторам.
        /// Идентификаторы (id) могут быть получены с помощью методов users.get, places.getById, places.search, places.getCheckins.
        /// </summary>
        /// <param name="cityIds">Идентификаторы городов</param>
        public static async Task<List<IdTitleObject>> GetCitiesById(IEnumerable<string> cityIds)
        {
            VkAPI.Manager.Method("database.getCitiesById");

            if (cityIds != null && cityIds.Any())
            {
                VkAPI.Manager.Params("city_ids", cityIds.Aggregate((a, b) => a + "," + b));
            }
            else throw new ArgumentException("cityIds не может быть пустым");

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                if (result != null && result.ChildNodes.Count > 0)
                    return result.ChildNodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList();
            }
            return null;
        }

        /// <summary>
        /// Возвращает список высших учебных заведений
        /// </summary>
        /// <param name="country">Идентификатор страны, учебные заведения которой необходимо вернуть</param>
        /// <param name="city">Идентификатор города, учебные заведения которого необходимо вернуть</param>
        /// <param name="q">Строка поискового запроса. Например, СПБ</param>
        /// <param name="offset">Отступ, необходимый для получения определенного подмножества учебных заведений</param>
        /// <param name="count">Количество учебных заведений, которое необходимо вернуть</param>
        public static async Task<ListCount<IdTitleObject>> GetUniversities(int country, int city, string q = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("database.getUniversities");
            VkAPI.Manager.Params("country_id", country);
            VkAPI.Manager.Params("city_id", city);
            
            if (q != null)
            {
                VkAPI.Manager.Params("q", q);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<IdTitleObject>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает список школ
        /// </summary>
        /// <param name="country">Идентификатор страны, школы которого необходимо вернуть</param>
        /// <param name="city">Идентификатор города, школы которого необходимо вернуть</param>
        /// <param name="q">Строка поискового запроса. Например, гимназия</param>
        /// <param name="offset">Отступ, необходимый для получения определенного подмножества школ</param>
        /// <param name="count">Количество школ, которое необходимо вернуть</param>
        public static async Task<ListCount<IdTitleObject>> GetSchools(int country, int city, string q = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("database.getSchools");
            VkAPI.Manager.Params("country_id", country);
            VkAPI.Manager.Params("city_id", city);

            if (q != null)
            {
                VkAPI.Manager.Params("q", q);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<IdTitleObject>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает список факультетов
        /// </summary>
        /// <param name="university">Идентификатор университета, факультеты которого необходимо получить</param>
        /// <param name="offset">Отступ, необходимый для получения определенного подмножества факультетов.</param>
        /// <param name="count">Количество факультетов которое необходимо получить</param>
        public static async Task<ListCount<IdTitleObject>> GetFaculties(int university, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("database.getFaculties");
            VkAPI.Manager.Params("university_id", university);
           
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<IdTitleObject>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает список кафедр университета по указанному факультету.
        /// </summary>
        /// <param name="faculty">Идентификатор факультета, кафедры которого необходимо получить.</param>
        /// <param name="offset">Отступ, необходимый для получения определенного подмножества кафедр.</param>
        /// <param name="count">Количество кафедр которое необходимо получить.
        ///  По умолчанию 100, максимальное значение 10000</param>
        public static async Task<ListCount<IdTitleObject>> GetChairs(int faculty, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("database.getChairs");
            VkAPI.Manager.Params("faculty_id", faculty);

            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<IdTitleObject>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x)).ToList());
            }
            return null;
        }

        /// <summary>
        /// Возвращает список классов, характерных для школ определенной страны.
        /// </summary>
        /// <param name="country">Идентификатор страны, доступные классы в которой необходимо вернуть.</param>
        public static async Task<List<IdTitleObject>> GetSchoolClasses(int? country = null)
        {
            VkAPI.Manager.Method("database.getSchoolClasses");
           
            if (country != null)
            {
                VkAPI.Manager.Params("country_id", country);
            }

            var apiManager = await VkAPI.Manager.Execute(false);
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("list");
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => new IdTitleObject(x, "item[1]", "item[2]")).ToList();
            }
            return null;
        }
    }
}
