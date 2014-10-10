using System.Xml;
using VkAPIAsync.Utils;

namespace VkAPIAsync.Wrappers.Photos
{
    /// <summary>
    ///  Информация о копиях исходного изображения разных размеров
    /// </summary>
    public class PhotoSize
    {
        public PhotoSize(XmlNode node)
        {
            Width = node.Int("width");
            Height = node.Int("height");
            Source = node.String("src");
            Type = new PhotoSizeType((PhotoSizeType.PhotoSizeTypeEnum)node.Enum("type", typeof(PhotoSizeType.PhotoSizeTypeEnum)));
        }

        /// <summary>
        ///  Url копии изображения
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        ///  Ширина копии в пикселах
        /// </summary>
        public int? Width { get; set; }
        /// <summary>
        /// Высота копии в пикселах
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Тип размера
        /// </summary>
        public PhotoSizeType Type { get; set; }
    }

    /// <summary>
    /// Тип размера фотографии
    /// </summary>
    public class PhotoSizeType
    {
        public enum PhotoSizeTypeEnum
        {
            /// <summary>
            ///  Пропорциональная копия изображения с максимальной шириной 75px
            /// </summary>
            s,
            /// <summary>
            ///  Пропорциональная копия изображения с максимальной шириной 130px
            /// </summary>
            m,
            /// <summary>
            /// Пропорциональная копия изображения с максимальной шириной 604px
            /// </summary>
            x,
            /// <summary>
            ///  Если соотношение "ширина/высота" исходного изображения меньше или равно 3:2, то пропорциональная копия с максимальной шириной 130px. Если соотношение "ширина/высота" больше 3:2, то копия обрезанного слева изображения с максимальной шириной 130px и соотношением сторон 3:2
            /// </summary>
            o,
            /// <summary>
            ///  Если соотношение "ширина/высота" исходного изображения меньше или равно 3:2, то пропорциональная копия с максимальной шириной 200px. Если соотношение "ширина/высота" больше 3:2, то копия обрезанного слева изображения с максимальной шириной 200px и соотношением сторон 3:2
            /// </summary>
            p,
            /// <summary>
            /// Если соотношение "ширина/высота" исходного изображения меньше или равно 3:2, то пропорциональная копия с максимальной шириной 320px. Если соотношение "ширина/высота" больше 3:2, то копия обрезанного слева изображения с максимальной шириной 320px и соотношением сторон 3:2
            /// </summary>
            q,
            /// <summary>
            /// Пропорциональная копия изображения с максимальной шириной 807px
            /// </summary>
            y,
            /// <summary>
            /// Пропорциональная копия изображения с максимальным размером 1280x1024
            /// </summary>
            z,
            /// <summary>
            ///  Пропорциональная копия изображения с максимальным размером 2560x2048px
            /// </summary>
            w
        }

        public PhotoSizeTypeEnum Value { get; private set; }

        public string StringValue { get; private set; }

        public PhotoSizeType(PhotoSizeTypeEnum value)
        {
            StringValue = value.ToString("G");
            Value = value;
        }
    }
}
