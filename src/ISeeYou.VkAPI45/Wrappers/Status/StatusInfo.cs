#region Using

using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Audios;

#endregion

namespace VkAPIAsync.Wrappers.Status
{
    /// <summary>
    /// Информация о статусе
    /// </summary>
    public class StatusInfo
    {
        public StatusInfo(XmlNode node)
        {
            Text = node.String("text");

            var audio = node.SelectSingleNode("audio");
            if (audio != null)
            {
                Audio = new Audio(audio);
            }
        }

        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Аудиозапись, которая играет сейчас
        /// </summary>
        public Audio Audio { get; set; }
    }
}