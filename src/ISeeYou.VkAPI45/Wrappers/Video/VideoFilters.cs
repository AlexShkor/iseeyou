using System.Collections.Generic;
using System.Linq;

namespace VkAPIAsync.Wrappers.Videos
{
    /// <summary>
    /// Поисковые фильтры для видео
    /// </summary>
    public class VideoFilters
    {
        private List<string> Values { get; set; }

        /// <summary>
        /// Искать только видео в формате mp4 (воспроиводимое на iOS)
        /// </summary>
        public void Mp4()
        {
            Values.Add("mp4");
        }

        /// <summary>
        /// Возвращать только youtube видео
        /// </summary>
        public void Youtube()
        {
            Values.Add("youtube");
        }

        /// <summary>
        /// Возвращать только vimeo видео
        /// </summary>
        public void Vimeo()
        {
            Values.Add("vimeo");
        }

        /// <summary>
        ///  Возвращать только короткие видеозаписи
        /// </summary>
        public void Short()
        {
            Values.Add("short");
        }

        /// <summary>
        /// Возвращать только длинные видеозаписи
        /// </summary>
        public void Long()
        {
            Values.Add("long");
        }

        public override string ToString()
        {
            return Values.Distinct().Aggregate((a, b) => a + "," + b);
        }
    }
}
