#region Using

using System;
using System.Globalization;
using System.Linq;
using System.Xml;

#endregion

namespace VkAPIAsync.Utils
{
    /// <summary>
    ///     Парсер XmlNode
    /// </summary>
    public static class XmlNodeParser
    {
        /// <summary>
        /// Извлекает DateTime из текущего XmlNode
        /// </summary>
        public static DateTime? DateTimeVal(this XmlNode node)
        {
            try
            {
                return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((double)node.IntVal());
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Извлекает DateTime
        /// </summary>
        /// <param name="nodeName"> Имя дочерного XmlNode </param>
        /// <returns></returns>
        public static DateTime? DateTimeFromUnixTime(this XmlNode node, string nodeName)
        {
            if (node.SelectSingleNode(nodeName) != null)
            {
                try
                {
                    var time = Int(node, nodeName);
                    return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(time));
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///     Извлекает строку
        /// </summary>
        /// <param name="nodeName"> Имя дочерного XmlNode </param>
        public static string String(this XmlNode node, string nodeName)
        {
            if (node.SelectSingleNode(nodeName) != null)
            {
                try
                {
                    return node.SelectSingleNode(nodeName).InnerText
                               .Replace("&lt;br&gt;", "\r\n");
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///     Извлекает 32-битное число
        /// </summary>
        /// <param name="nodeName"> Имя дочерного XmlNode </param>
        public static int? Int(this XmlNode node, string nodeName)
        {
            if (node.SelectSingleNode(nodeName) != null)
            {
                try
                {
                    return Convert.ToInt32(node.SelectSingleNode(nodeName).InnerText);
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///     Извлекает 16-битное число
        /// </summary>
        /// <param name="nodeName"> Имя дочерного XmlNode </param>
        public static short? Short(this XmlNode node, string nodeName)
        {
            if (node.SelectSingleNode(nodeName) != null)
            {
                try
                {
                    return Convert.ToInt16(node.SelectSingleNode(nodeName).InnerText);
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///     Извлекает 32-битное число из текущего XmlNode
        /// </summary>
        public static int? IntVal(this XmlNode node)
        {
            try
            {
                return Convert.ToInt32(node.InnerText);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Извлекает Double
        /// </summary>
        /// <param name="nodeName"> Имя дочерного XmlNode </param>
        public static double? Double(this XmlNode node, string nodeName)
        {
            if (node.SelectSingleNode(nodeName) != null)
            {
                try
                {
                    return Convert.ToDouble(node.SelectSingleNode(nodeName).InnerText);
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///     Извлекает Float
        /// </summary>
        /// <param name="nodeName"> Имя дочерного XmlNode </param>
        /// <returns></returns>
        public static float? Float(this XmlNode node, string nodeName)
        {
            if (node.SelectSingleNode(nodeName) != null)
            {
                try
                {
                    return float.Parse(node.SelectSingleNode(nodeName).InnerText);
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///     Извлекает Bool
        /// </summary>
        /// <param name="nodeName"> Имя дочерного XmlNode </param>
        /// <returns></returns>
        public static bool? Bool(this XmlNode node, string nodeName)
        {
            if (node.SelectSingleNode(nodeName) != null)
            {
                try
                {
                    return ((node.SelectSingleNode(nodeName).InnerText == "1"));
                }
                catch (NullReferenceException)
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        ///     Извлекает Bool из текущего елемента
        /// </summary>
        public static bool? BoolVal(this XmlNode node)
        {
            //Проверка, можно ли значение XmlNode преобразовать в bool
            if (node.InnerText != "1" & node.InnerText != "0")
            {
                return null;
            }

            return (node.InnerText == "1");
        }

        /// <summary>
        ///     Превращает строкове значение дочерного елемента в еквивалентное значение перечисления
        /// </summary>
        /// <param name="nodeName">Имя дочерного елемента</param>
        /// <param name="enumType">Тип перечисления (typeof(ТИП))</param>
        public static Enum Enum(this XmlNode node, string nodeName, Type enumType)
        {
            try
            {
                var enumNode = node.SelectSingleNode(nodeName);
                if (enumNode != null)
                {
                    var names = enumNode.InnerText.Split(char.Parse("_"));
                    names = names.Select<string, string>((name) =>
                        {
                            if (name.Length <= 1)
                            {
                                return name.ToUpperInvariant();
                            }
                            var first = name.First();
                            return char.ToUpper(first) + name.Substring(1);
                        }).ToArray();
                    return
                        (Enum)
                        System.Enum.Parse(enumType, names.Aggregate((a,b) => a + b));
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}