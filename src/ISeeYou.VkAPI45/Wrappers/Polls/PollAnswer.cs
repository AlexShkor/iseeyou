#region Using

using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Polls
{
    /// <summary>
    /// Ответ на опрос
    /// </summary>
    public class PollAnswer
    {
        /// <summary>
        ///      Идентификатор ответа на вопрос
        /// </summary>
        public int? Id;

        /// <summary>
        ///     Рейтинг данного варианта ответа, выраженный в процентах
        /// </summary>
        public float? Rate;

        /// <summary>
        ///      Текст ответа на вопрос
        /// </summary>
        public string Text;

        /// <summary>
        ///      Количество пользователей, проголосовавших за данный вариант ответа
        /// </summary>
        public int? Votes;

        public PollAnswer(XmlNode node)
        {
            Id = node.Int("id");
            Rate = node.Float("rate");
            Text = node.String("text");
            Votes = node.Int("votes");
        }
    }
}