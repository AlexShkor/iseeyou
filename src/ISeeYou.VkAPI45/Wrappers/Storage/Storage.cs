#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;

#endregion

namespace VkAPIAsync.Wrappers.Storage
{
    /// <summary>
    /// Хранилище
    /// </summary>
    public static class Storage
    {
        /// <summary>
        ///     Возвращает значение переменной, название которой передано в параметре key.
        ///     Задать значение позволяет метод storage.set.
        /// </summary>
        /// <param name="key">Строковое название переменной длиной не более 100 символов.</param>
        /// <param name="keys">Список ключей, разделённых запятыми. Если указан этот параметр, то параметр key не учитывается. Максимальное количество ключей не должно превышать 1000 штук.</param>
        /// <param name="global">Указывается 1, если необходимо получить глобальную переменную, а не переменную пользователя. По умолчанию 0.</param>
        /// <returns>Возвращает значение одной или нескольких переменных. Если переменная на сервере отсутствует, то будет возвращена пустая строка. </returns>
        /// <remarks>
        ///     Переменные могут храниться в двух областях видимости:
        ///     Пользовательская переменная привязана к пользователю, и только он или сервер приложения может получить к ней доступ. Может быть создано не более 1000 переменных для каждого пользователя.
        ///     Глобальная переменная привязана к приложению, и работа с ней не зависит от пользователя. Для того чтобы задать глобальную переменную при работе с API от имени пользователя, нужно передать параметр global. Может быть создано не более 5000 глобальных переменных.
        /// </remarks>
        public static async Task<string> Get(string key, IEnumerable<string> keys = null, bool? global = null)
        {
            VkAPI.Manager.Method("storage.get");
            VkAPI.Manager.Params("key", key);
            if (keys != null && keys.Any())
            {
                VkAPI.Manager.Params("keys", keys.Aggregate((a, b) => a + "," + b));
            }
            if (global != null)
            {
                VkAPI.Manager.Params("global", global.Value ? 1 : 0);
            }

            await VkAPI.Manager.Execute();
            var result = VkAPI.Manager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.InnerText;
            }
            return null;
        }

        /// <summary>
        ///     Сохраняет значение переменной, название которой передано в параметре key.
        ///     Получить сохранённое значение позволяет метод storage.get.
        /// </summary>
        /// <param name="key">Строковое название переменной длиной не более 100 символов. Может содержать символы латинского алфавита, цифры, знак тире, нижнее подчёркивание [a-zA-Z_\-0-9].</param>
        /// <param name="value">Строковое значение переменной, ограниченное 4096 байтами.</param>
        /// <param name="global">Указывается 1, если необходимо работать с глобальными переменными, а не с переменными пользователя. По умолчанию 0.</param>
        /// <returns>Возвращает 1 в случае удачного сохранения переменной. </returns>
        /// <remarks>
        ///     Переменные могут храниться в двух областях видимости:
        ///     Пользовательская переменная привязана к пользователю, и только он или сервер приложения может получить к ней доступ. Может быть создано не более 1000 переменных для каждого пользователя.
        ///     Глобальная переменная привязана к приложению, и работа с ней не зависит от пользователя. Для того чтобы задать глобальную переменную при работе с API от имени пользователя, нужно передать параметр global. Может быть создано не более 5000 глобальных переменных.
        /// </remarks>
        public static async Task<bool> Set(string key, string value = null, bool? global = null)
        {
            VkAPI.Manager.Method("storage.set");
            VkAPI.Manager.Params("key", key);
            if (value != null)
            {
                VkAPI.Manager.Params("value", value);
            }
            if (global != null)
            {
                VkAPI.Manager.Params("global", global.Value ? 1 : 0);
            }

            await VkAPI.Manager.Execute();
            var result = VkAPI.Manager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Возвращает названия всех переменных.
        /// </summary>
        /// <param name="userId">ID пользователя, названия переменных которого получаются, в случае если данные запрашиваются серверным методом. </param>
        /// <param name="global">Указывается true, если необходимо работать с глобальными переменными, а не с переменными пользователя. </param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества названий переменных. По умолчанию 0. </param>
        /// <param name="count">Количество названий переменных, информацию о которых необходимо получить. </param>
        public static async Task<List<string>> GetKeys(int? userId = null, bool? global = null, int? offset = null, int? count = null)
        {
            VkAPI.Manager.Method("storage.getKeys");
            
            if (userId != null)
            {
                VkAPI.Manager.Params("user_id", userId);
            }
            if (global != null)
            {
                VkAPI.Manager.Params("global", global.Value ? 1 : 0);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }

            await VkAPI.Manager.Execute();
            var result = VkAPI.Manager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.ChildNodes.Cast<XmlNode>().Select(x => x.InnerText).ToList();
            }
            return null;
        }
    }
}