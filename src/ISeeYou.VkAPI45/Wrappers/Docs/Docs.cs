#region Using

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;

#endregion

namespace VkAPIAsync.Wrappers.Docs
{
    /// <summary>
    /// Документы
    /// </summary>
    public class Docs
    {
        /// <summary>
        ///     Возвращает адрес сервера для загрузки документов.
        /// </summary>
        /// <param name="groupId">Идентификатор сообщества (если необходимо загрузить документ в список документов сообщества). Если документ размещается на стене группы, этот параметр указывать не нужно. </param>
        public static async Task<string> GetUploadServer(int? groupId = null)
        {
            VkAPI.Manager.Method("docs.getUploadServer");

            if (groupId != null)
            {
                VkAPI.Manager.Params("group_id", groupId);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? result.String("upload_url") : "";
        }

        /// <summary>
        ///     Возвращает адрес сервера для загрузки документов в папку отправленные, для последующей отправки документа на стену или личным сообщением.
        /// </summary>
        /// <param name="groupId">Идентификатор сообщества, в которое нужно загрузить документ</param>
        public static async Task<string> GetWallUploadServer(int? groupId = null)
        {
            VkAPI.Manager.Method("docs.getWallUploadServer");

            if (groupId != null)
            {
                VkAPI.Manager.Params("group_id", groupId);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? result.String("upload_url") : "";
        }

        /// <summary>
        ///     Сохраняет документ после его успешной загрузки на сервер.
        /// </summary>
        /// <param name="file">Параметр, возвращаемый в результате загрузки файла на сервер.</param>
        /// <param name="title">Название документа</param>
        /// <param name="tags">Метки для поиска</param>
        public static async Task<Document> Save(string file, string title = null, string tags = null)
        {
            VkAPI.Manager.Method("docs.save");
            VkAPI.Manager.Params("file", file);

            if (title != null)
            {
                VkAPI.Manager.Params("title", title);
            }
            if (tags != null)
            {
                VkAPI.Manager.Params("tags", tags);
            }

            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed ? new Document(result.SelectSingleNode("doc")) : null;
        }

        /// <summary>
        ///     Возвращает информацию о документах.
        /// </summary>
        /// <param name="docsIds">Перечисленные через запятую идентификаторы – идущие через знак подчеркивания id пользователей, которым принадлежат документы, и id самих документов.</param>
        public static async Task<List<Document>> GetById(IEnumerable<string> docsIds)
        {
            VkAPI.Manager.Method("docs.getById");
            VkAPI.Manager.Params("docs", docsIds.Aggregate((a, b) => a + "," + b));

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (!VkAPI.Manager.MethodSuccessed) return null;

            XmlNodeList nodes = result.SelectNodes("doc");
            return (from XmlNode node in nodes select new Document(node)).ToList();
        }

        /// <summary>
        ///     Возвращает расширенную информацию о документах текущего пользователя.
        /// </summary>
        /// <param name="oid">ID пользователя или группы, документы которого нужно вернуть. По умолчанию – id текущего пользователя. Если необходимо получить документы группы, в этом параметре должно стоять значение, равное -id группы.</param>
        /// <param name="count">Количество документов, которое нужно вернуть. (по умолчанию – все документы)</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества документов.</param>
        public static async Task<ListCount<Document>> Get(int? oid = null, int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("docs.get");
            if (oid.HasValue) VkAPI.Manager.Params("owner_id", oid);
            if (count.HasValue) VkAPI.Manager.Params("count", count);
            if (offset.HasValue) VkAPI.Manager.Params("offset", offset);
            XmlNode result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && nodes.Count > 0)
                    return new ListCount<Document>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Document(x)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Удаляет документ пользователя или группы.
        /// </summary>
        /// <param name="docId">ID документа.</param>
        /// <param name="ownerId">ID владельца документы. Если удаляемый документ находится на странице группы, в этом параметре должно стоять значение, равное -id группы.</param>
        /// <returns>При успешном удалении документа сервер вернет true. </returns>
        public static async Task<bool> Delete(int docId, int ownerId)
        {
            VkAPI.Manager.Method("docs.delete");
            VkAPI.Manager.Params("doc_id", docId);
            VkAPI.Manager.Params("owner_id", ownerId);
            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        /// Копирует документ в документы текущего пользователя
        /// </summary>
        /// <param name="docId">Идентификатор документа</param>
        /// <param name="ownerId">Идентификатор пользователя или сообщества, которому принадлежит документ</param>
        /// <param name="accessKey">Ключ доступа документа. Этот параметр следует передать, если вместе с остальными данными о документе было возвращено поле access_key. </param>
        /// <returns>После успешного выполнения возвращает идентификатор созданного документа</returns>
        public static async Task<int> Add(int docId, int ownerId, string accessKey = null)
        {
            VkAPI.Manager.Method("docs.add");
            VkAPI.Manager.Params("doc_id", docId);
            VkAPI.Manager.Params("owner_id", ownerId);

            if (accessKey != null)
            {
                VkAPI.Manager.Params("access_key", accessKey);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }
    }
}