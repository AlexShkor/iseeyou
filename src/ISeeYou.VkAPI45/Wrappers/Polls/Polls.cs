#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Polls
{
    /// <summary>
    /// Опросы
    /// </summary>
    public static class Polls
    {
        /// <summary>
        ///     Возвращает детальную информацию об опросе по его идентификатору.
        /// </summary>
        /// <param name="pollId">Идентификатор опроса, информацию о котором необходимо получить.</param>
        /// <param name="ownerId">Идентификатор владельца опроса, информацию о котором необходимо получить. Если параметр не указан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="isBoard">true – опрос находится в обсуждении, false – опрос прикреплен к стене</param>
        public static async Task<Poll> GetById(int pollId, int? ownerId = null, bool? isBoard = null)
        {
            VkAPI.Manager.Method("polls.getById");
            VkAPI.Manager.Params("poll_id", pollId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (isBoard!= null)
            {
                VkAPI.Manager.Params("is_board", isBoard.Value ? 1 : 0);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new Poll(result) : null;
        }

        /// <summary>
        ///     Отдает голос текущего пользователя за выбранный вариант ответа в указанном опросе.
        /// </summary>
        /// <param name="pollId">Идентификатор опроса, в котором необходимо проголосовать.</param>
        /// <param name="answerId">Идентификатор варианта ответа, за который необходимо проголосовать.</param>
        /// <param name="ownerId">Идентификатор владельца опроса. Если параметр не указан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="isBoard">true – опрос находится в обсуждении, false – опрос прикреплен к стене</param>
        /// <returns>
        ///     В случае успеха возвращает:
        ///     true – если голос текущего пользователя был отдан за выбранный вариант ответа
        ///     false – если текущий пользователь уже голосовал в указанном опросе
        /// </returns>
        public static async Task<bool> AddVote(int pollId, int answerId, int? ownerId = null, bool? isBoard = null)
        {
            VkAPI.Manager.Method("polls.addVote");
            VkAPI.Manager.Params("poll_id", pollId);
            VkAPI.Manager.Params("answer_id", answerId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (isBoard != null)
            {
                VkAPI.Manager.Params("is_board", isBoard.Value ? 1 : 0);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Снимает голос текущего пользователя с выбранного варианта ответа в указанном опросе.
        /// </summary>
        /// <param name="ownerId">Идентификатор владельца опроса. Если параметр не указан, то он считается равным идентификатору текущего пользователя.</param>
        /// <param name="pollId">Идентификатор опроса, в котором необходимо снять голос.</param>
        /// <param name="answerId">Идентификатор варианта ответа, с которого необходимо снять голос.</param>
        /// <param name="isBoard">true – опрос находится в обсуждении, false – опрос прикреплен к стене</param>
        /// <returns>
        ///     В случае успеха возвращает:
        ///     true – если голос текущего пользователя был снят с выбранного варианта ответа
        ///     false – если текущий пользователь еще не голосовал в указанном опросе или указан не выбранный им вариант ответа
        /// </returns>
        public static async Task<bool> DeleteVote(int pollId, int answerId, int? ownerId = null, bool? isBoard = null)
        {
            VkAPI.Manager.Method("polls.deleteVote");
            VkAPI.Manager.Params("poll_id", pollId);
            VkAPI.Manager.Params("answer_id", answerId);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (isBoard != null)
            {
                VkAPI.Manager.Params("is_board", isBoard.Value ? 1 : 0);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Получает список идентификаторов пользователей, которые выбрали определенные варианты ответа в опросе
        /// </summary>
        /// <param name="answerIds">Идентификаторы вариантов ответа</param>
        /// <param name="pollId">Идентификатор опроса</param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит опрос</param>
        /// <param name="friendsOnly">Указывает, необходимо ли возвращать только пользователей, которые являются друзьями текущего пользователя</param>
        /// <param name="isBoard">true – опрос находится в обсуждении, false – опрос прикреплен к стене</param>
        /// <param name="offset">Смещение относительно начала списка, для выборки определенного подмножества. Если параметр не задан, то считается, что он равен 0. </param>
        /// <param name="count">Количество возвращаемых идентификаторов пользователей.
        /// Если параметр не задан, то считается, что он равен 100, если не задан параметр friends_only, в противном случае 10.
        /// Максимальное значение параметра 1000, если не задан параметр friends_only, в противном случае 100. </param>
        /// <param name="fields">Перечисленные через запятую поля анкет, необходимые для получения. Доступные значения: nickname, screen_name, sex, bdate (birthdate), city, country, timezone, photo, photo_medium, photo_big, has_mobile, rate, contacts, education, online, counters. </param>
        /// <param name="nameCase">Падеж для склонения имени и фамилии пользователя. Возможные значения: именительный – nom, родительный – gen, дательный – dat, винительный – acc, творительный – ins, предложный – abl. По умолчанию nom. </param>
        public static async Task<List<AnswerUsers>> GetVoters(IEnumerable<int> answerIds, int pollId, int? ownerId = null, bool? isBoard = null, int? offset = null,
                                                  int? count = null, IEnumerable<string> fields = null, string nameCase = null, bool? friendsOnly = null)
        {
            if (answerIds == null || !answerIds.Any())
            {
                throw new ArgumentException("answerIds");
            }

            VkAPI.Manager.Method("polls.getVoters");
            VkAPI.Manager.Params("poll_id", pollId);
            VkAPI.Manager.Params("answer_ids", answerIds.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b));
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (isBoard != null)
            {
                VkAPI.Manager.Params("is_board", isBoard.Value ? 1 : 0);
            }
            if (friendsOnly != null)
            {
                VkAPI.Manager.Params("friends_only", friendsOnly.Value ? 1 : 0);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("fields", fields.Aggregate((a, b) => a + "," + b));
            }
            if (nameCase != null)
            {
                VkAPI.Manager.Params("name_case", nameCase);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.ChildNodes;
                if (nodes != null && nodes.Count > 0)
                    return nodes.Cast<XmlNode>().Select(x => new AnswerUsers(x)).ToList();
            }
            return null;
        }

        /// <summary>
        /// Позволяет создавать опросы, которые впоследствии можно прикреплять к записям на странице пользователя или сообщества
        /// </summary>
        /// <param name="question">Текст вопроса</param>
        /// <param name="answers">список вариантов ответов, например:
        /// ["yes", "no", "maybe"]
        /// Может быть не менее 1 и не более 10 вариантов ответа. 
        /// Данные в формате JSON</param>
        /// <param name="ownerId">Если опрос будет добавлен в группу необходимо передать отрицательный идентификатор группы. По умолчанию текущий пользователь. </param>
        /// <param name="isAnonymous">true – анонимный опрос, список проголосовавших недоступен; 
        /// false – опрос публичный, список проголосовавших доступен; 
        /// По умолчанию – 0. </param>
        public static async Task<Poll> Create(string question, string answers, int? ownerId = null, bool? isAnonymous = null)
        {
            VkAPI.Manager.Method("polls.create");
            VkAPI.Manager.Params("question", question);
            VkAPI.Manager.Params("add_answers", answers);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (isAnonymous != null)
            {
                VkAPI.Manager.Params("is_anonymous", isAnonymous.Value ? 1 : 0);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new Poll(result) : null;
        }

        /// <summary>
        /// Позволяет редактировать созданные опросы
        /// </summary>
        /// <param name="pollId">Идентификатор редактируемого опроса </param>
        /// <param name="ownerId">Идентификатор владельца опроса </param>
        /// <param name="question">Новый текст редактируемого опроса </param>
        /// <param name="addAnswers">Список вариантов ответов</param>
        /// <param name="editAnswers">Объект, содержащий варианты ответов, которые необходимо отредактировать; 
        /// ключ – идентификатор ответа, значение – новый текст ответа </param>
        /// <param name="deleteAnswers">Список идентификаторов ответов, которые необходимо удалить</param>
        /// <returns>В случае успешного выполнения - true</returns>
        public static async Task<bool> Edit(int pollId, int ownerId, string question = null, string addAnswers = null, string deleteAnswers = null, string editAnswers = null)
        {
            VkAPI.Manager.Method("polls.edit");
            VkAPI.Manager.Params("poll_id", pollId);
            VkAPI.Manager.Params("owner_id", ownerId);
            if (question != null)
            {
                VkAPI.Manager.Params("question", question);
            }
            if (addAnswers != null)
            {
                VkAPI.Manager.Params("add_answers", addAnswers);
            }
            if (editAnswers != null)
            {
                VkAPI.Manager.Params("edit_answers", editAnswers);
            }
            if (deleteAnswers != null)
            {
                VkAPI.Manager.Params("delete_answers", deleteAnswers);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }
    }
}