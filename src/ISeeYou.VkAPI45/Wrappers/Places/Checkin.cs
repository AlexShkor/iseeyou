#region Using

using System;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Отметка на месте
    /// </summary>
    public class Checkin
    {
        public Checkin(XmlNode node)
        {
            Id = node.Int("id");
            Uid = node.Int("user_id");
            Date = node.DateTimeFromUnixTime("date");
            Latitude = node.Double("latitude");
            Longitude = node.Double("longitude");
            Text = node.String("text");
            PlaceId = node.Int("place_id");
            PlaceTitle = node.String("place_title");
            PlaceCity = node.Int("place_city");
            PlaceCountry = node.Int("[lace_country");
            PlaceAddress = node.String("place_address");
            PlaceType = node.Int("place_type");
            PlaceIcon = node.String("place_icon");
        }

        /// <summary>
        /// Идентификатор отметки
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Идентификатор отметившегося пользователя
        /// </summary>
        public int? Uid { get; set; }

        /// <summary>
        /// Дата добавления отметки
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        ///  Географическая широта, заданная в градусах (от -90 до 90)
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Географическая долгота, заданная в градусах (от -180 до 180)
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Идентификатор места
        /// </summary>
        public int? PlaceId { get; set; }

        /// <summary>
        /// Текст сопроводительного сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Наименование места
        /// </summary>
        public string PlaceTitle { get; set; }

        /// <summary>
        /// Идентификатор города
        /// </summary>
        public int? PlaceCity { get; set; }

        /// <summary>
        /// Идентификатор страны
        /// </summary>
        public int? PlaceCountry { get; set; }

        /// <summary>
        /// Иконка места
        /// </summary>
        public string PlaceIcon { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string PlaceAddress { get; set; }

        /// <summary>
        /// Тип места
        /// </summary>
        public int? PlaceType { get; set; }
    }
}