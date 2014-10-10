#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Информация о местоположении
    /// </summary>
    public class Place : BaseEntity
    {
        /// <summary>
        /// Строка с указанием адреса места в городе
        /// </summary>
        public string Address{ get; set; }

        /// <summary>
        /// Идентификатор города
        /// </summary>
        public int? CityId{ get; set; }

        /// <summary>
        /// Идентификатор страны
        /// </summary>
        public int? CountryId{ get; set; }

        /// <summary>
        /// Тип места
        /// </summary>
        public int? PlaceType{ get; set; }

        /// <summary>
        /// Название места
        /// </summary>
        public string Title{ get; set; }

        /// <summary>
        ///  Ширина
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public string Longtitude { get; set; }

        /// <summary>
        /// Дата добавления места
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Дата последнего обновления местав
        /// </summary>
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Расстояние от исходной точки
        /// </summary>
        public int? Distance { get; set; }

        /// <summary>
        /// Иконка места
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Количество checkin'ов
        /// </summary>
        public int? Checkins { get; set; }

        public Place(XmlNode x)
        {
            Id = x.Int("id");
            Title = x.String("title");
            Latitude = x.String("latitude");
            Longtitude = x.String("longtitude");
            Address = x.String("address");
            CityId = x.Int("city");
            CountryId = x.Int("country");
            PlaceType = x.Int("type");
            Created = x.DateTimeFromUnixTime("created");
            Updated = x.DateTimeFromUnixTime("updated");
            Distance = x.Int("distance");
            Icon = x.String("icon");
            Checkins = x.Int("checkins");
        }
    }
}