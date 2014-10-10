using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    /// Школа, в которой учился(учится) пользователь
    /// </summary>
    public class School
    {
        public School(XmlNode node)
        {
            Id = node.Int("id");
            Name = node.String("name");
            Country = node.Short("country");
            City = node.Int("city");
            YearFrom = node.Short("year_from");
            YearTo = node.Short("year_to");
            YearGraduated = node.Short("year_graduated");
            Class = node.String("class");
            Specialiaty = node.String("speciality");
            Type = node.Short("type");
            TypeString = node.String("type_str");
        }

        /// <summary>
        ///  Идентификатор школы.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Наименование школы.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор страны, в которой расположена школа.
        /// </summary>
        public short? Country { get; set; }

        /// <summary>
        /// Идентификатор города, в котором расположена школа.
        /// </summary>
        public int? City { get; set; }

        /// <summary>
        ///  Год начала обучения.
        /// </summary>
        public short? YearFrom { get; set; }

        /// <summary>
        ///  Год окончания обучения.
        /// </summary>
        public short? YearTo { get; set; }

        /// <summary>
        ///  Год выпуска.
        /// </summary>
        public short? YearGraduated { get; set; }

        /// <summary>
        /// Буква класса.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        ///  Специализация.
        /// </summary>
        public string Specialiaty { get; set; }

        /// <summary>
        /// Идентификатор типа.
        /// </summary>
        public short? Type { get; set; }

        /// <summary>
        ///  Название типа.
        /// </summary>
        public string TypeString { get; set; }
    }
}
