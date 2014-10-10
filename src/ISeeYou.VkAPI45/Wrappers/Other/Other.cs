using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace VkAPIAsync.Wrappers.Other
{
    /// <summary>
    /// Прочее
    /// </summary>
    public static class Other
    {
        /// <summary>
        /// Универсальный метод, который позволяет запускать последовательность других методов, сохраняя и фильтруя промежуточные результаты. 
        /// </summary>
        /// <param name="code">Код алгоритма в VKScript - формате, похожем на JavaSсript или ActionScript (предполагается совместимость с ECMAScript). Алгоритм должен завершаться командой return %выражение%. Операторы должны быть разделены точкой с запятой.</param>
        public static async Task<Dictionary<string,object>> Execute(string code)
        {
            VkAPI.Manager.JSON = true;
            VkAPI.Manager.Method("execute");
            if(code == null)
                throw new ArgumentNullException("code");

            VkAPI.Manager.Params("code", Uri.EscapeDataString(code));

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseString();
            if (string.IsNullOrWhiteSpace(result))
            {
                return null;
            }
            VkAPI.Manager.JSON = false;
            return new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(result);
        }
    }
}
