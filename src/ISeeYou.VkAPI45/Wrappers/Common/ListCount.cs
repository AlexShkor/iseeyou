#region Using

using System.Collections.Generic;

#endregion

namespace VkAPIAsync.Wrappers.Common
{
    /// <summary>
    /// Список с дополнительным полем TotalCount
    /// </summary>
    /// <typeparam name="T">Тип обьекта, который хранится в списке</typeparam>
    public class ListCount<T> : List<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">Всего элементов</param>
        /// <param name="list">Список элементов</param>
        public ListCount(int count, List<T> list)
        {
            TotalCount = count;
            this.AddRange(list);
        }

        /// <summary>
        /// Количество всех обьектов (необязательно равно количеству полученных элементов)
        /// </summary>
        public int TotalCount { get; set; }
    }
}