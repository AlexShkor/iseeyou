#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Groups;
using VkAPIAsync.Wrappers.Users;

#endregion

namespace VkAPIAsync.Wrappers.Audios
{
    /// <summary>
    /// Аудиозаписи
    /// </summary>
    public static class Audios
    {
        /// <summary>
        ///     Редактирует данные аудиозаписи на странице пользователя или группы.
        /// </summary>
        /// <param name="audioId">ID аудиозаписи.</param>
        /// <param name="ownerId">ID владельца аудиозаписи. Если редактируемая аудиозапись находится на странице группы, в этом параметре должно стоять значение, равное -id группы.</param>
        /// <param name="artist">Название исполнителя аудиозаписи.</param>
        /// <param name="title">Название аудиозаписи.</param>
        /// <param name="text">Текст аудиозаписи, если введен.</param>
        /// <param name="noSearch">true - скрывает аудиозапись из поиска по аудиозаписям, false (по умолчанию) - не скрывает.</param>
        /// <returns> После успешного выполнения возвращает id текста, введенного пользователем (lyrics_id), если текст не был введен, вернет 0.</returns>
        public static async Task<int> Edit(int audioId, int ownerId, string artist, string title, string text,
                                bool noSearch = false, int? genreId = null)
        {
            VkAPI.Manager.Method("audio.edit");
            VkAPI.Manager.Params("audio_id", audioId);
            VkAPI.Manager.Params("owner_id", ownerId);
            VkAPI.Manager.Params("artist", artist);
            VkAPI.Manager.Params("title", title);
            VkAPI.Manager.Params("text", text);
            VkAPI.Manager.Params("no_search", noSearch);
            if (genreId != null)
            {
                VkAPI.Manager.Params("genre_id", genreId);
            }
        
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Возвращает текст аудиозаписи по параметру lyrics_id, полученному в результате работы методов audio.get, audio.getById или audio.search.
        /// </summary>
        /// <param name="id">ID текста аудиозаписи, полученный в audio.get, audio.getById или audio.search.</param>
        public static async Task<string> GetLyrics(int id)
        {
            VkAPI.Manager.Method("audio.getLyrics");
            VkAPI.Manager.Params("lyrics_id", id);
   
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var node = result.SelectSingleNode("lyrics/text");
                if (node != null)
                    return node.InnerText;
            }
            return null;
        }

        /// <summary>
        /// Транслирует аудиозапись в статус пользователю или сообществу.
        /// </summary>
        /// <param name="audio">Идентификатор аудиозаписи, которая будет отображаться в статусе, в формате owner_id+audio_id. Например, 1_190442705. Если параметр не указан, аудиостатус указанных сообществ и пользователя будет удален. </param>
        /// <param name="targets">Перечисленные через запятую идентификаторы сообществ и пользователя, которым будет транслироваться аудиозапись. Идентификаторы сообществ должны быть заданы в формате "-gid", где gid - идентификатор сообщества. Например, 1,-34384434. По умолчанию аудиозапись транслируется текущему пользователю. </param>
        /// <returns>В случае успешного выполнения возвращает массив идентификаторов сообществ и пользователя, которым был установлен или удален аудиостатус.</returns>
        public static async Task<List<int>> SetBroadcast(string audio = null, IEnumerable<string> targets = null)
        {
            VkAPI.Manager.Method("audio.setBroadcast");

            if (audio != null)
            {
                VkAPI.Manager.Params("audio", audio);
            }
            if (targets != null)
            {
                VkAPI.Manager.Params("target_ids", targets.Aggregate((a,b) => a + "," + b));
            }

            
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.ChildNodes.Cast<XmlNode>().Select(x => x.IntVal()).Where(x => x.HasValue).Select(x => x.Value).ToList();
            }
            return null;
        }

        /// <summary>
        /// Возвращает список друзей и сообществ пользователя, которые транслируют музыку в статус.
        /// </summary>
        /// <param name="filter">Определяет, какие типы объектов необходимо получить</param>
        /// <param name="active">true — будут возвращены только друзья и сообщества, которые транслируют музыку в данный момент.</param>
        public static async Task<UsersGroupsList> GetBroadcastList(BroadcastListFilter filter = null, bool? active = null)
        {
            VkAPI.Manager.Method("audio.getBroadcastList");

            if (filter != null)
            {
                VkAPI.Manager.Params("filter", filter);
            }
            if (active!= null)
            {
                VkAPI.Manager.Params("active", active.Value ? 1 : 0);
            }
    
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var res = new UsersGroupsList();
                var userNodes = result.SelectNodes("//user");
                if (userNodes.Count > 0)
                    res.Users = userNodes.Cast<XmlNode>().Select(x => new BaseUser(x)).ToList();
                var groupNodes = result.SelectNodes("//group");
                if (groupNodes.Count > 0)
                    res.Groups = groupNodes.Cast<XmlNode>().Select(x => new Group(x)).ToList();
                return res;
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список рекомендуемых аудиозаписей на основе списка воспроизведения заданного пользователя или на основе одной выбранной аудиозаписи.
        /// </summary>
        /// <param name="targetAudio">Идентификатор аудиозаписи, на основе которой будет строиться список рекомендаций. Используется вместо параметра userId. Идентификатор представляет из себя разделённые знаком подчеркивания id пользователя, которому принадлежит аудиозапись, и id самой аудиозаписи. Если аудиозапись принадлежит сообществу, то в качестве первого параметра используется -id сообщества.</param>
        /// <param name="userId">Идентификатор пользователя для получения списка рекомендаций на основе его набора аудиозаписей (по умолчанию — идентификатор текущего пользователя).</param>
        /// <param name="offset">Смещение относительно первой найденной аудиозаписи для выборки определенного подмножества.</param>
        /// <param name="count">Количество возвращаемых аудиозаписей. Максимальное значение — 200, по умолчанию — 100.</param>
        /// <param name="shufle">true — включен случайный порядок.</param>
        public static async Task<ListCount<Audio>> GetRecommendations(string targetAudio = null, int? uid = null, int? offset = null,
                                                          int? count = null, bool? shufle = null)
        {
            VkAPI.Manager.Method("audio.getRecommendations");
            if (uid != null)
            {
                VkAPI.Manager.Params("user_id", uid);
            }
            if (targetAudio != null)
            {
                VkAPI.Manager.Params("target_audio", targetAudio);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (shufle != null)
            {
                VkAPI.Manager.Params("shufle", shufle.Value ? 1 : 0);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            var nodes = result.SelectNodes("items/audio");
            return VkAPI.Manager.MethodSuccessed && nodes != null && nodes.Count > 0
                       ? new ListCount<Audio>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new Audio(x)).ToList())
                       : null;
        }

        /// <summary>
        /// Возвращает список аудиозаписей из раздела "Популярное"
        /// </summary>
        /// <param name="onlyEng">true – возвращать только зарубежные аудиозаписи. false – возвращать все аудиозаписи. (по умолчанию) </param>
        /// <param name="genre">Идентификатор жанра из списка жанров.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества аудиозаписей. </param>
        /// <param name="count">Количество возвращаемых аудиозаписей. </param>
        public static async Task<List<Audio>> GetPopular(bool? onlyEng = null, AudioGenre genre = null, int? offset = null,
                                                          int? count = null)
        {
            VkAPI.Manager.Method("audio.getPopular");
            if (onlyEng != null)
            {
                VkAPI.Manager.Params("only_eng", onlyEng.Value ? 1 : 0);
            }
            if (genre != null)
            {
                VkAPI.Manager.Params("genre_id", genre.Value);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            var nodes = result.SelectNodes("audio");
            return VkAPI.Manager.MethodSuccessed && nodes != null && nodes.Count > 0
                       ? nodes.Cast<XmlNode>().Select(x => new Audio(x)).ToList()
                       : null;
        }

        /// <summary>
        ///     Копирует аудиозапись на страницу пользователя или группы.
        /// </summary>
        /// <param name="audioId">ID аудиозаписи.</param>
        /// <param name="ownerId">ID владельца аудиозаписи. Если копируемая аудиозапись находится на странице группы, в этом параметре должно стоять значение, равное -id группы.</param>
        /// <param name="groupId">ID группы, в которую следует копировать аудиозапись. Если параметр не указан, аудиозапись копируется не в группу, а на страницу текущего пользователя. Если аудиозапись все же копируется в группу, у текущего пользователя должны быть права на эту операцию.</param>
        public static async Task<bool> Add(int audioId, int ownerId, int? groupId = null)
        {
            VkAPI.Manager.Method("audio.add");
            VkAPI.Manager.Params("audio_id", audioId);
            VkAPI.Manager.Params("owner_id", ownerId);
            if (groupId != null) VkAPI.Manager.Params("group_id", groupId);
            
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal() > 0;
        }

        /// <summary>
        ///     Возвращает список альбомов аудиозаписей пользователя или группы.
        /// </summary>
        /// <param name="oid">идентификатор пользователя или сообщества, у которого необходимо получить список альбомов с аудио. </param>
        /// <param name="count">Количество альбомов, которое необходимо вернуть. (по умолчанию – не больше 50, максимум - 100).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества альбомов.</param>
        public static async Task<ListCount<AudioAlbum>> GetAlbums(int? oid = null, int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("audio.getAlbums");
            if (oid != null)
            {
                VkAPI.Manager.Params("owner_id", oid);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
    
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            var nodes = result.SelectNodes("items/album");
            return VkAPI.Manager.MethodSuccessed && nodes != null && nodes.Count > 0
                       ? new ListCount<AudioAlbum>(result.Int("count").Value, nodes.Cast<XmlNode>().Select(x => new AudioAlbum(x)).ToList())
                       : null;
        }

        /// <summary>
        ///     Создает пустой альбом аудиозаписей.
        /// </summary>
        /// <param name="title">Название альбома.</param>
        /// <param name="gid">ID группы, которой принадлежат аудиозаписи. Если параметр не указан, то альбом создается у текущего пользователя.</param>
        /// <returns>Возвращает album_id - id созданного альбома. </returns>
        public static async Task<int> AddAlbum(string title, int? gid)
        {
            VkAPI.Manager.Method("audio.addAlbum");
            VkAPI.Manager.Params("title", title);
            if (gid != null) VkAPI.Manager.Params("group_id", gid);
       
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Редактирует название альбома аудиозаписей.
        /// </summary>
        /// <param name="title">Новое название альбома.</param>
        /// <param name="albumId">ID редактируемого альбома.</param>
        /// <param name="gid">ID группы, которой принадлежат аудиозаписи. Если параметр не указан, то изменяется альбом текущего пользователя.</param>
        /// <returns>Возвращает true в случае успеха. </returns>
        public static async Task<bool> EditAlbum(string title, int albumId, int? gid)
        {
            VkAPI.Manager.Method("audio.editAlbum");
            VkAPI.Manager.Params("album_id", albumId);
            VkAPI.Manager.Params("title", title);
            if (gid != null) VkAPI.Manager.Params("group_id", gid);

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return result.BoolVal().Value;
        }

        /// <summary>
        ///     Удаляет альбом аудиозаписей.
        /// </summary>
        /// <param name="albumId">ID удаляемого альбома.</param>
        /// <param name="gid">ID группы, которой принадлежат аудиозаписи. Если параметр не указан, то альбом удаляется у текущего пользователя.</param>
        /// <returns>Возвращает true в случае успеха. </returns>
        public static async Task<bool> DeleteAlbum(int albumId, int? gid)
        {
            VkAPI.Manager.Method("audio.deleteAlbum");
            VkAPI.Manager.Params("album_id", albumId);
            if (gid != null) VkAPI.Manager.Params("group_id", gid);

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return result.BoolVal().Value;
        }

        /// <summary>
        ///     Перемещает аудиозаписи в альбом.
        /// </summary>
        /// <param name="aids">ID аудиозаписей, перечисленные через запятую.</param>
        /// <param name="albumId">ID альбома, в который перемещаются аудиозаписи.</param>
        /// <param name="groupId">ID группы, которой принадлежат аудиозаписи. Если параметр не указан, то работа ведется с альбомом текущего пользователя.</param>
        /// <returns>Возвращает true в случае успеха. </returns>
        public static async Task<bool> MoveToAlbum(IEnumerable<string> aids, int albumId, int? groupId)
        {
            VkAPI.Manager.Method("audio.moveToAlbum");
            VkAPI.Manager.Params("audio_ids", aids.Aggregate((a, b) => a + "," + b));
            VkAPI.Manager.Params("album_id", albumId);
            if (groupId != null)
                VkAPI.Manager.Params("group_id", groupId);

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return result.BoolVal().Value;
        }

        /// <summary>
        ///     Возвращает список аудиозаписей пользователя или группы.
        /// </summary>
        /// <param name="ownerId">ID пользователя, которому принадлежат аудиозаписи (по умолчанию — текущий пользователь)</param>
        /// <param name="albumId">ID альбома, аудиозаписи которого необходимо вернуть (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
        /// <param name="audioIds">Перечисленные через запятую id аудиозаписей, входящие в выборку по userId или gid.</param>
        /// <param name="count">Количество возвращаемых аудиозаписей.</param>
        /// <param name="offset">Смещение относительно первой найденной аудиозаписи для выборки определенного подмножества.</param>
        public static async Task<ListCount<Audio>> Get(int? ownerId = null, int? albumId = null,
                                           IEnumerable<string> audioIds = null, int? count = null,
                                           int? offset = null)
        {
            VkAPI.Manager.Method("audio.get");
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (audioIds != null)
            {
                VkAPI.Manager.Params("audio_ids", audioIds.Aggregate((a, b) => a + "," + b));
            }
            if (albumId != null)
            {
                VkAPI.Manager.Params("album_id", albumId);
            }         
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
           
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (!VkAPI.Manager.MethodSuccessed) return null;

            XmlNodeList nodes = result.SelectNodes("items/audio");
            return new ListCount<Audio>(result.Int("count").Value, (from XmlNode node in nodes select new Audio(node)).ToList());
        }

        /// <summary>
        /// Возвращает список аудиозаписей пользователя или группы, дополнительно возвращается объект user.
        /// </summary>
        /// <param name="ownerId">ID пользователя, которому принадлежат аудиозаписи (по умолчанию — текущий пользователь)</param>
        /// <param name="albumId">ID альбома, аудиозаписи которого необходимо вернуть (по умолчанию возвращаются аудиозаписи из всех альбомов).</param>
        /// <param name="audioIds">Перечисленные через запятую id аудиозаписей, входящие в выборку по userId или gid.</param>
        /// <param name="count">Количество возвращаемых аудиозаписей.</param>
        /// <param name="offset">Смещение относительно первой найденной аудиозаписи для выборки определенного подмножества.</param>
        public static async Task<GetWithUserInfoResult> GetWithUserInfo(int? ownerId = null, int? albumId = null,
                                           IEnumerable<string> audioIds = null, int? count = null,
                                           int? offset = null)
        {
            VkAPI.Manager.Method("audio.get");
            VkAPI.Manager.Params("need_user", 1);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            if (audioIds != null)
            {
                VkAPI.Manager.Params("audio_ids", audioIds.Aggregate((a, b) => a + "," + b));
            }
            if (albumId != null)
            {
                VkAPI.Manager.Params("album_id", albumId);
            }
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (!VkAPI.Manager.MethodSuccessed) return null;

            return new GetWithUserInfoResult(result);
        }

        /// <summary>
        ///     Возвращает информацию об аудиозаписях.
        /// </summary>
        /// <param name="audioIds">Перечисленные через запятую идентификаторы – идущие через знак подчеркивания id пользователей, которым принадлежат аудиозаписи, и id самих аудиозаписей. Если аудиозапись принадлежит группе, то в качестве первого параметра используется -id группы.</param>
        public static async Task<List<Audio>> GetById(IEnumerable<string> audioIds)
        {
            VkAPI.Manager.Method("audio.getById");
            VkAPI.Manager.Params("audios", audioIds.Aggregate((a, b) => a + "," + b));

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (!VkAPI.Manager.MethodSuccessed) return null;

            XmlNodeList nodes = result.SelectNodes("audio");
            return (from XmlNode node in nodes select new Audio(node)).ToList();
        }

        /// <summary>
        ///     Возвращает количество аудиозаписей пользователя или группы.
        /// </summary>
        /// <param name="ownerId">ID владельца аудиозаписей. Если необходимо получить количество аудиозаписей группы, в этом параметре должно стоять значение, равное -id группы.</param>
        public static async Task<int> GetCount(int ownerId)
        {
            VkAPI.Manager.Method("audio.getCount");
            VkAPI.Manager.Params("owner_id", ownerId);

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.IntVal().HasValue ? result.IntVal().Value : -1;
        }

        /// <summary>
        ///     Возвращает список аудиозаписей в соответствии с заданным критерием поиска.
        /// </summary>
        /// <param name="query">Строка поискового запроса. Например, The Beatles.</param>
        /// <param name="autoComplete">Если этот параметр равен true, возможные ошибки в поисковом запросе будут исправлены. Например, при поисковом запросе Иуфдуы поиск будет осуществляться по строке Beatles.</param>
        /// <param name="order">Вид сортировки. 2 - по популярности, 1 - по длительности аудиозаписи, 0 - по дате добавления.</param>
        /// <param name="withLyrics">Если этот параметр равен true, поиск будет производиться только по тем аудиозаписям, которые содержат тексты.</param>
        /// <param name="count">Количество возвращаемых аудиозаписей (максимум 200).</param>
        /// <param name="offset">Смещение относительно первой найденной аудиозаписи для выборки определенного подмножества.</param>
        /// <param name="searchOwn">true – искать по аудиозаписям пользователя, false – не искать по аудиозаписям пользователя (по умолчанию). </param>
        /// <param name="performersOnly">Если этот параметр равен true, поиск будет осуществляться только по названию исполнителя. </param>
        public static async Task<ListCount<Audio>> Search(string query, bool? autoComplete = null, AudioSortOrder order = null,
                                              bool? withLyrics = null, int? count = null,
                                              int? offset = null, bool? searchOwn = null, bool? performersOnly = null)
        {
            VkAPI.Manager.Method("audio.search");
            VkAPI.Manager.Params("q", query);
            if (order != null) VkAPI.Manager.Params("sort", order);
            if (withLyrics != null) VkAPI.Manager.Params("lyrics", withLyrics.Value ? 1 : 0);
            if (autoComplete != null) VkAPI.Manager.Params("auto_complete", autoComplete);
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }
            if (searchOwn != null)
            {
                VkAPI.Manager.Params("search_own", searchOwn.Value ? 1 : 0);
            }
            if (performersOnly != null)
            {
                VkAPI.Manager.Params("performer_only", performersOnly.Value ? 1 : 0);
            }
            
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                XmlNodeList nodes = result.SelectNodes("items/audio");
                return new ListCount<Audio>(result.Int("count").Value, (from XmlNode node in nodes select new Audio(node)).ToList());
            }
            return null;
        }

        /// <summary>
        ///     Возвращает адрес сервера для загрузки аудиозаписей.
        /// </summary>
        public static async Task<string> GetUploadServer()
        {
            VkAPI.Manager.Method("audio.getUploadServer");
            
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                return result.String("upload_url");
            }
            return null;
        }

        /// <summary>
        ///     Удаляет аудиозапись со страницы пользователя или группы.
        /// </summary>
        /// <param name="audioId">ID аудиозаписи.</param>
        /// <param name="ownerId">ID владельца аудиозаписи. Если удаляемая аудиозапись находится на странице группы, в этом параметре должно стоять значение, равное -id группы.</param>
        public static async Task<bool> Delete(int audioId, int ownerId)
        {
            VkAPI.Manager.Method("audio.delete");
            VkAPI.Manager.Params("audio_id", audioId);
            VkAPI.Manager.Params("owner_id", ownerId);
            
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Восстанавливает удаленную аудиозапись пользователя после удаления.
        /// </summary>
        /// <param name="audioId">ID удаленной аудиозаписи.</param>
        /// <param name="ownerId">ID владельца аудиозаписи. По умолчанию - id текущего пользователя.</param>
        public static async Task<bool> Restore(int audioId, int? ownerId = null)
        {
            VkAPI.Manager.Method("audio.restore");
            VkAPI.Manager.Params("audio_id", audioId);
            if (ownerId != null) VkAPI.Manager.Params("owner_id", ownerId);
            
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Изменяет порядок аудиозаписи, перенося ее между аудиозаписями, идентификаторы которых переданы параметрами after и before.
        /// </summary>
        /// <param name="audioId">ID аудиозаписи, порядок которой изменяется.</param>
        /// <param name="before">ID аудиозаписи, перед которой нужно поместить аудиозапись. Если аудиозапись переносится в конец, параметр может быть равен нулю.</param>
        /// <param name="after">ID аудиозаписи, после которой нужно поместить аудиозапись. Если аудиозапись переносится в начало, параметр может быть равен нулю.</param>
        /// <param name="ownerId">ID владельца изменяемой аудиозаписи. По умолчанию - id текущего пользователя.</param>
        public static async Task<bool> Reorder(int audioId, int before, int after, int? ownerId = null)
        {
            VkAPI.Manager.Method("audio.reorder");
            VkAPI.Manager.Params("audio_id", audioId);
            VkAPI.Manager.Params("before", before);
            VkAPI.Manager.Params("after", after);
            if (ownerId != null)
            {
                VkAPI.Manager.Params("owner_id", ownerId);
            }
            
            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            return VkAPI.Manager.MethodSuccessed && result.BoolVal().Value;
        }

        /// <summary>
        ///     Сохраняет аудиозаписи после успешной загрузки.
        /// </summary>
        /// <param name="server">Параметр, возвращаемый в результате загрузки аудиофайла на сервер.</param>
        /// <param name="audio">Параметр, возвращаемый в результате загрузки аудиофайла на сервер.</param>
        /// <param name="hash">Параметр, возвращаемый в результате загрузки аудиофайла на сервер.</param>
        /// <param name="title">Название композиции. По умолчанию берется из ID3 тегов.</param>
        /// <param name="artist">Автор композиции. По умолчанию берется из ID3 тегов.</param>
        public static async Task<Audio> Save(string server, string audio, string hash, string title = null,
                                      string artist = null)
        {
            VkAPI.Manager.Method("audio.save");
            VkAPI.Manager.Params("server", server);
            VkAPI.Manager.Params("audio", audio);
            VkAPI.Manager.Params("hash", hash);
            if (title != null)
            {
                VkAPI.Manager.Params("title", title);
            }
            if (artist != null)
            {
                VkAPI.Manager.Params("artist", artist);
            }

            var result = (await VkAPI.Manager.Execute()).GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var node = result.SelectSingleNode("audio");
                return node != null ? new Audio(node) : null;
            }
            return null;
        }
    }

    /// <summary>
    /// Результат выполнения метода GetWithUserInfo
    /// </summary>
    public class GetWithUserInfoResult
    {
        public GetWithUserInfoResult(XmlNode node)
        {
            XmlNodeList audioNodes = node.SelectNodes("items/audio");
            if (audioNodes.Count > 0)
            {
                Audios = new ListCount<Audio>(node.Int("count").Value, (from XmlNode x in audioNodes select new Audio(node)).ToList());
            }

            var userNode = node.SelectSingleNode("//user");
            if (userNode != null)
            {
                UserId = userNode.Int("user_id");
                Name = userNode.String("name");
                Photo = userNode.String("photo");
                NameGen = userNode.String("name_gen");
            }
        }

        /// <summary>
        /// Список аудиозаписей пользователя
        /// </summary>
        public ListCount<Audio> Audios { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Имя и фамилия пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// URL на фотографию пользователя
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Имя пользователя в родительном падеже
        /// </summary>
        public string NameGen { get; set; }

    }
}