using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Users
{
    /// <summary>
    /// ВУЗ, в котором учился(учится) пользователь
    /// </summary>
    public class University
    {
        public University(XmlNode node)
        {
            Id = node.Int("id");
            Country = node.Short("country");
            City = node.Int("city");
            Name = node.String("name");
            Faculty = node.Int("faculty");
            FacultyName = node.String("faculty_name");
            Chair = node.Int("chair");
            ChairName = node.String("chair_name");
        }

        /// <summary>
        ///  Идентификатор университета.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Идентификатор страны, в которой расположен университет.
        /// </summary>
        public short? Country { get; set; }

        /// <summary>
        /// Идентификатор города, в котором расположен университет.
        /// </summary>
        public int? City { get; set; }

        /// <summary>
        /// Наименование университета.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор факультета.
        /// </summary>
        public int? Faculty { get; set; }

        /// <summary>
        /// Наименование факультета.
        /// </summary>
        public string FacultyName { get; set; }

        /// <summary>
        /// Идентификатор кафедры.
        /// </summary>
        public int? Chair { get; set; }

        /// <summary>
        /// Наименование кафедры.
        /// </summary>
        public string ChairName { get; set; }

        /// <summary>
        /// Год окончания обучения. 
        /// </summary>
        public short? Graduation { get; set; }
    }
}
