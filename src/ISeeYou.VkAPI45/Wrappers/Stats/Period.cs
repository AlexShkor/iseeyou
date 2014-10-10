#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Stats
{
    /// <summary>
    /// Период
    /// </summary>
    public class Period
    {
        public delegate Enum StringEnumDelegate(string str);

        public delegate int? StringIntDelegate(string str);

        public Period(XmlNode node)
        {
            Day = DateTime.ParseExact(node.String("day"), "yyyy'-'MM'-'dd", CultureInfo.InvariantCulture);
            Views = node.Int("views");
            Visitors = node.Int("visitors");
            Reach = node.Int("reach");
            ReachSubscribers = node.Int("reach_subscribers");
            var sexNodes = node.SelectNodes("sex/item");
            if (sexNodes != null && sexNodes.Count > 0)
            {
                Sex = new List<Statistic<UserSex>>();
                foreach (var sexNode in sexNodes.Cast<XmlNode>())
                {
                    var parser = new StringEnumDelegate((str) =>
                        {
                            if (str != null)
                            {
                                return str == "f" ? UserSex.UserSexEnum.Female : UserSex.UserSexEnum.Male;
                            }
                            return null;
                        });
                    Sex.Add(new Statistic<UserSex>(sexNode, parser));
                }
            }
            var ageNodes = node.SelectNodes("age/item");
            if (ageNodes != null && ageNodes.Count > 0)
            {
                Age = new List<Statistic<AgeEnum>>();
                foreach (var ageNode in ageNodes.Cast<XmlNode>())
                {
                    var parser = new StringEnumDelegate((str) =>
                        {
                            if (str != null)
                            {
                                switch (str)
                                {
                                    case "12-18":
                                        return AgeEnum.From12To18;
                                    case "18-21":
                                        return AgeEnum.From18To21;
                                    case "21-24":
                                        return AgeEnum.From21To24;
                                    case "24-27":
                                        return AgeEnum.From24To27;
                                    case "27-30":
                                        return AgeEnum.From27To30;
                                    case "30-35":
                                        return AgeEnum.From30To35;
                                    case "35-45":
                                        return AgeEnum.From35To45;
                                    case "45-100":
                                        return AgeEnum.From45To100;
                                }
                            }
                            return null;
                        });
                    Age.Add(new Statistic<AgeEnum>(ageNode, parser));
                }
            }
            var sexAgeNodes = node.SelectNodes("sex_age/item");
            if (sexAgeNodes != null && sexAgeNodes.Count > 0)
            {
                SexAge = new List<Statistic<SexAgeEnum>>();
                foreach (var sexAgeNode in sexAgeNodes.Cast<XmlNode>())
                {
                    var parser = new StringEnumDelegate((str) =>
                        {
                            if (str != null)
                            {
                                switch (str)
                                {
                                    case "m;12-18":
                                        return SexAgeEnum.MFrom12To18;
                                    case "m;18-21":
                                        return SexAgeEnum.MFrom18To21;
                                    case "m;21-24":
                                        return SexAgeEnum.MFrom21To24;
                                    case "m;24-27":
                                        return SexAgeEnum.MFrom24To27;
                                    case "m;27-30":
                                        return SexAgeEnum.MFrom27To30;
                                    case "m;30-35":
                                        return SexAgeEnum.MFrom30To35;
                                    case "m;35-45":
                                        return SexAgeEnum.MFrom35To45;
                                    case "m;45-100":
                                        return SexAgeEnum.MFrom45To100;
                                    case "f;12-18":
                                        return SexAgeEnum.FFrom12To18;
                                    case "f;18-21":
                                        return SexAgeEnum.FFrom18To21;
                                    case "f;21-24":
                                        return SexAgeEnum.FFrom21To24;
                                    case "f;24-27":
                                        return SexAgeEnum.FFrom24To27;
                                    case "f;27-30":
                                        return SexAgeEnum.FFrom27To30;
                                    case "f;30-35":
                                        return SexAgeEnum.FFrom30To35;
                                    case "f;35-45":
                                        return SexAgeEnum.FFrom35To45;
                                    case "f;45-100":
                                        return SexAgeEnum.FFrom45To100;
                                }
                            }
                            return null;
                        });
                    SexAge.Add(new Statistic<SexAgeEnum>(sexAgeNode, parser));
                }
            }
            var citiesNodes = node.SelectNodes("cities/item");
            if (citiesNodes != null && citiesNodes.Count > 0)
            {
                Cities = new List<Statistic<int>>();
                foreach (var cityNode in citiesNodes.Cast<XmlNode>())
                {
                    var parser = new StringIntDelegate((str) =>
                        {
                            if (str != null)
                            {
                                int res;
                                int.TryParse(str, out res);
                                return res;
                            }
                            return -1;
                        });
                    Cities.Add(new Statistic<int>(cityNode, parser));
                }
            }
            var countriesNodes = node.SelectNodes("countries/item");
            if (countriesNodes != null && countriesNodes.Count > 0)
            {
                Countries = new List<Statistic<int>>();
                foreach (var countryNode in countriesNodes.Cast<XmlNode>())
                {
                    var parser = new StringIntDelegate((str) =>
                        {
                            if (str != null)
                            {
                                int res;
                                int.TryParse(str, out res);
                                return res;
                            }
                            return -1;
                        });
                    Countries.Add(new Statistic<int>(countryNode, parser));
                }
            }
        }

        /// <summary>
        /// День
        /// </summary>
        public DateTime Day { get; set; }

        /// <summary>
        /// Количество просмотров
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// Количество уникальных посетителей
        /// </summary>
        public int? Visitors { get; set; }

        /// <summary>
        /// Полный охват
        /// </summary>
        public int? Reach { get; set; }

        /// <summary>
        ///  Охват подписчиков
        /// </summary>
        public int? ReachSubscribers { get; set; }

        /// <summary>
        ///  Список структур, описывающих статистику по полу
        /// </summary>
        public List<Statistic<UserSex>> Sex { get; set; }

        /// <summary>
        /// Список структур, описывающих статистику по возрасту
        /// </summary>
        public List<Statistic<AgeEnum>> Age { get; set; }

        /// <summary>
        /// Список структур, описывающих статистику по полу и возрасту
        /// </summary>
        public List<Statistic<SexAgeEnum>> SexAge { get; set; }

        /// <summary>
        /// Список структур, описывающих статистику по городам
        /// </summary>
        public List<Statistic<int>> Cities { get; set; }

        /// <summary>
        /// Список структур, описывающих статистику по странам
        /// </summary>
        public List<Statistic<int>> Countries { get; set; }
    }
}