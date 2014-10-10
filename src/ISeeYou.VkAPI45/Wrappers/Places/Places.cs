#region Using

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Места
    /// </summary>
    public static class Places
    {
        /// <summary>
        ///     Добавляет новое место в базу географических мест. Созданное место будет выводиться в поиске по местам только тому, кто его добавил.
        /// </summary>
        /// <param name="title">Название нового места.</param>
        /// <param name="latitude">Географическая широта нового места, заданная в градусах (от -90 до 90).</param>
        /// <param name="longitude">Географическая долгота нового места, заданная в градусах (от -180 до 180).</param>
        /// <param name="type">Идентификатор типа нового места, полученный методом places.getTypes.</param>
        /// <param name="country">Идентификатор страны нового места, полученный методом places.getCountries.</param>
        /// <param name="city">Идентификатор города нового места, полученный методом places.getCities.</param>
        /// <param name="address">Строка с адресом нового места (например, Невский просп. 1).</param>
        /// <returns>В случае успешного создания места метод возвратит идентификатор созданного места (pid). </returns>
        public static async Task<int> Add(string title, double latitude, double longitude, int type, int? country = null,
                              int? city = null, string address = null)
        {
            VkAPI.Manager.Method("places.add");
            VkAPI.Manager.Params("title", title);
            VkAPI.Manager.Params("latitude", latitude);
            VkAPI.Manager.Params("longitude", longitude);
            VkAPI.Manager.Params("type", type);
            if (country != null)
            {
                VkAPI.Manager.Params("country", country);
            }
            if (city != null)
            {
                VkAPI.Manager.Params("city", city);
            }
            if (address != null)
            {
                VkAPI.Manager.Params("address", address);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.Int("id").HasValue ? result.Int("id").Value : -1;
        }

        /// <summary>
        ///     Возвращает информацию о местах по их идентификаторам.
        /// </summary>
        /// <param name="ids">Перечисленные через запятую идентификаторы мест.</param>
        public static async Task<List<Place>> GetById(IEnumerable<int> ids = null)
        {
            VkAPI.Manager.Method("places.getById");
            if (ids != null)
            {
                VkAPI.Manager.Params("places", ids.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("place");
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => new Place(x)).ToList();
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список мест, найденных по заданным условиям поиска. Поиск производится среди мест, добавленных модераторами сайта и текущим пользователем. Места в списке расположены в порядке увеличения дистанции от исходной точки поиска.
        /// </summary>
        /// <param name="latitude">Географическая широта точки, в радиусе которой необходимо производить поиск, заданная в градусах (от -90 до 90).</param>
        /// <param name="longitude">Географическая долгота точки, в радиусе которой необходимо производить поиск, заданная в градусах (от -180 до 180).</param>
        /// <param name="city">Идентификатор города</param>
        /// <param name="count">Количество мест, информацию о которых необходимо вернуть</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества результатов поиска</param>
        /// <param name="q">Строка поискового запроса.</param>
        /// <param name="radius">Тип радиуса зоны поиска</param>
        public static async Task<ListCount<Place>> Search(int latitude, int longitude, string q = null, int? city = null, int? offset = null, int? count = null, SearchRadius radius = null)
        {
            VkAPI.Manager.Method("places.search");
            VkAPI.Manager.Params("latitude", latitude);
            VkAPI.Manager.Params("longitude", longitude);
            if (q != null)
            {
                VkAPI.Manager.Params("q", q);
            }
            if (radius != null)
            {
                VkAPI.Manager.Params("radius", radius);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (city != null)
            {
                VkAPI.Manager.Params("city", city);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("//place");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<Place>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Place(x)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Отмечает пользователя в указанном месте.
        /// </summary>
        /// <param name="placeId">Идентификатор места.</param>
        /// <param name="text">Комментарий к отметке длиной до 255 символов (переводы строк не поддерживаются).</param>
        /// <param name="latitude">Географическая широта отметки, заданная в градусах (от -90 до 90).</param>
        /// <param name="longitude">Географическая долгота отметки, заданная в градусах (от -180 до 180).</param>
        /// <param name="services">Список сервисов или сайтов, на которые необходимо экспортировать отметку, в случае если пользователь настроил соответствующую опцию. Например twitter, facebook.</param>
        /// <param name="friendsOnly">1 - отметка будет доступна только друзьям, 0 - всем пользователям. По умолчанию публикуемые отметки доступны всем пользователям.</param>
        /// <returns>В случае успешного создания места метод возвратит идентификатор созданной отметки (сid). </returns>
        public static async Task<int> Checkin(int placeId, string text = null, int? latitude = null, int? longitude = null,
                                  IEnumerable<string> services = null, bool? friendsOnly = null)
        {
            VkAPI.Manager.Method("places.checkin");
            VkAPI.Manager.Params("text", text);
            if (latitude != null)
            {
                VkAPI.Manager.Params("latitude", latitude);
            }
            if (longitude != null)
            {
                VkAPI.Manager.Params("longitude", longitude);
            }
            if (services != null && services.Any())
            {
                VkAPI.Manager.Params("services", services.Aggregate((a, b) => a + "," + b));
            }
            if (friendsOnly != null)
            {
                VkAPI.Manager.Params("friends_only", friendsOnly.Value);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Возвращает список отметок пользователей в местах, согласно заданным параметрам.
        /// </summary>
        /// <param name="latitude">Географическая широта исходной точки поиска, заданная в градусах (от -90 до 90).</param>
        /// <param name="longitude">Географическая долгота исходной точки поиска, заданная в градусах (от -180 до 180).</param>
        /// <param name="place">Идентификатор места. Игнорируется, если указаны latitude и longitude.</param>
        /// <param name="userId">Идентификатор пользователя. Игнорируется, если указаны latitude и longitude или place.</param>
        /// <param name="offset">Смещение относительно первой отметки для выборки определенного подмножества. Игнорируется, если установлен ненулевой timestamp.</param>
        /// <param name="count">Количество возвращаемых отметок (максимум 50). Игнорируется, если установлен ненулевой timestamp.</param>
        /// <param name="timestamp">Указывает, что нужно вернуть только те отметки, которые были созданы после заданного timestamp.</param>
        /// <param name="friendsOnly">Указывает, что следует выводить только отметки друзей, если заданы географические координаты. Игнорируется, если не заданы параметры latitude и longitude.</param>
        /// <param name="needPlaces">Указывает, следует ли возвращать информацию о месте в котором была сделана отметка. Игнорируется, если указан параметр place.</param>
        public static async Task<ListCount<Checkin>> GetCheckins(int? latitude = null, int? longitude = null, int? place = null,
                                                     int? uid = null, int? offset = null, int? count = null,
                                                     int? timestamp = null, bool? friendsOnly = null,
                                                     bool? needPlaces = null)
        {
            VkAPI.Manager.Method("places.getCheckins");
            if (latitude != null)
            {
                VkAPI.Manager.Params("latitude", latitude);
            }
            if (longitude != null)
            {
                VkAPI.Manager.Params("longitude", longitude);
            }
            if (place != null)
            {
                VkAPI.Manager.Params("place", place);
            }
            if (uid != null)
            {
                VkAPI.Manager.Params("uid", uid);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (timestamp != null)
            {
                VkAPI.Manager.Params("timestamp", timestamp);
            }
            if (friendsOnly != null)
            {
                VkAPI.Manager.Params("friends_only", friendsOnly);
            }
            if (needPlaces != null)
            {
                VkAPI.Manager.Params("need_places", needPlaces);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("//checkin");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<Checkin>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Checkin(x)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список всех возможных типов мест.
        /// </summary>
        /// <returns>Возвращает массив всех возможных типов мест, каждый из объектов которого содержит поля id, title и icon. </returns>
        public static async Task<List<PlaceType>> GetTypes()
        {
            VkAPI.Manager.Method("places.getType");

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("type");
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => new PlaceType(x)).ToList();
            }
            return null;
        }
    }
}