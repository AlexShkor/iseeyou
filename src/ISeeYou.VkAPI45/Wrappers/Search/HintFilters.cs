using System.Collections.Generic;
using System.Linq;

namespace VkAPIAsync.Wrappers.Search
{
    /// <summary>
    /// Фильтр поисковых подсказок
    /// </summary>
    public class HintFilters
    {
        private List<string> Values { get; set; }

        /// <summary>
        ///  Друзья пользователя
        /// </summary>
        public void Friends()
        {
            Values.Add("friends");
        }

        /// <summary>
        /// Подписки пользователя
        /// </summary>
        public void Idols()
        {
            Values.Add("idols");
        }

        /// <summary>
        /// Публичные страницы, на которые подписан пользователь
        /// </summary>
        public void Publics()
        {
            Values.Add("publics");
        }

        /// <summary>
        ///  Группы пользователя
        /// </summary>
        public void Groups()
        {
            Values.Add("groups");
        }

        /// <summary>
        ///  Встречи пользователя
        /// </summary>
        public void Events()
        {
            Values.Add("events");
        }

        /// <summary>
        /// Люди, с которыми данный пользователь имеет переписку
        /// </summary>
        public void Correspondents()
        {
            Values.Add("correspondents");
        }

        /// <summary>
        /// Люди, у которых есть общие друзья с текущим пользователем (данный фильтр позволяет получить не всех пользователей имеющих общих друзей)
        /// </summary>
        public void MutualFriends()
        {
            Values.Add("mutual_friends");
        }

        public override string ToString()
        {
            return Values.Distinct().Aggregate((a, b) => a + "," + b);
        }
    }
}
