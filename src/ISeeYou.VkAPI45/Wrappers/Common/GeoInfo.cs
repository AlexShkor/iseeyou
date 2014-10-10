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
    public class GeoInfo
    {
        /// <summary>
        /// Координаты
        /// </summary>
        public GeoCoords Coordinates;

        /// <summary>
        /// Место
        /// </summary>
        public Place Place;

        /// <summary>
        /// Тип места
        /// </summary>
        public string Type;

        /// <summary>
        /// true, если местоположение является прикреплённой картой
        /// </summary>
        public bool? Showmap { get; set; }

        public GeoInfo(XmlNode node)
        {
            if (node == null) return;

            Type = node.String("/geo/type");
            var coordinates = node.String("/geo/coordinates").Split(new[] {" "}, StringSplitOptions.None);
            Coordinates = new GeoCoords
                {
                    Latitude = double.Parse(coordinates[0]),
                    Longitude = double.Parse(coordinates[1])
                };
            Place = new Place(node.SelectSingleNode("place"));
            Showmap = node.Bool("showmap");
        }
    }
}