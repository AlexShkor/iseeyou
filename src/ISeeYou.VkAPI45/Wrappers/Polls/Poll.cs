#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Polls
{
    /// <summary>
    /// Опрос
    /// </summary>
    public class Poll
    {
        /// <summary>
        ///     Идентификатор ответа текущего пользователя (если текущий пользователь еще не отвечал в данном опросе, то содержит 0)
        /// </summary>
        public int? AnswerId;

        /// <summary>
        ///     Массив, содержащий объекты с вариантами ответа на вопрос в опросе
        /// </summary>
        public List<PollAnswer> Answers;

        /// <summary>
        ///     Время создания опроса 
        /// </summary>
        public DateTime? DateCreated;

        /// <summary>
        ///      Идентификатор опроса
        /// </summary>
        public int? Id;

        /// <summary>
        ///      Идентификатор владельца опроса
        /// </summary>
        public int? OwnerId;

        /// <summary>
        ///     Текст вопроса для опроса
        /// </summary>
        public string Question;

        /// <summary>
        ///     Общее количество ответивших пользователей
        /// </summary>
        public int? Votes;

        /// <summary>
        /// Является ли опрос анонимным
        /// </summary>
        public bool? Anonymous { get; set; }

        public Poll(XmlNode node)
        {
            Id = node.Int("poll_id");
            OwnerId = node.Int("owner_id");
            DateCreated = node.DateTimeFromUnixTime("created");
            Question = node.String("question");
            Votes = node.Int("votes");
            AnswerId = node.Int("answer_id");
            Anonymous = node.Bool("anonymous");
            var answers = node.SelectNodes("answers/answer");
            if (answers != null && answers.Count > 0)
            {
                Answers = answers.Cast<XmlNode>().Select(x => new PollAnswer(x)).ToList();
            }
        }
    }
}