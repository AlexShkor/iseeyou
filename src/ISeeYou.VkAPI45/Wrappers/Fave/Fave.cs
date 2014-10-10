#region Using

using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using VkAPIAsync.Utils;
using VkAPIAsync.Wrappers.Common;
using VkAPIAsync.Wrappers.Photos;
using VkAPIAsync.Wrappers.Users;
using VkAPIAsync.Wrappers.Videos;
using VkAPIAsync.Wrappers.Wall;

#endregion

namespace VkAPIAsync.Wrappers.Fave
{
    /// <summary>
    /// Закладки
    /// </summary>
    public static class Fave
    {
        /// <summary>
        /// Возвращает список пользователей, добавленных текущим пользователем в закладки
        /// </summary>
        /// <param name="count">Количество пользователей, информацию о которых необходимо вернуть</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества пользователей. По умолчанию — 0</param>
        public static async Task<ListCount<BaseUser>> GetUsers(int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("fave.getUsers");
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                if (nodes != null && count != null)
                {
                    return new ListCount<BaseUser>(result.Int("count").Value,
                                                        nodes.Cast<XmlNode>().Select(x => new BaseUser(x)).ToList());
                }
            }
            return null;
        }

        /// <summary>
        ///     Возвращает фотографии, на которых текущий пользователь поставил отметку "Мне нравится".
        /// </summary>
        /// <param name="count">Количество фотографий, которое необходимо получить.</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества фотографий.</param>
        public static async Task<ListCount<Photo>> GetPhotos(int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("fave.getPhotos");
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                var countNode = result.SelectSingleNode("count");
                if (nodes != null && countNode != null)
                {
                    return new ListCount<Photo>(countNode.IntVal().Value,
                                                         nodes.Cast<XmlNode>()
                                                              .Select(x => new Photo(x))
                                                              .ToList());
                }
            }
            return null;
        }

        /// <summary>
        ///     Возвращает список видеозаписей, на которых текущий пользователь поставил отметку "Мне нравится".
        /// </summary>
        /// <param name="count">Количество возвращаемых видеозаписей.</param>
        /// <param name="offset">Смещение относительно первой найденной видеозаписи для выборки определенного подмножества.</param>
        public static async Task<ListCount<Video>> GetVideos(int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("fave.getVideos");
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                var countNode = result.SelectSingleNode("count");
                if (nodes != null && countNode != null)
                {
                    return new ListCount<Video>(countNode.IntVal().Value,
                                                     nodes.Cast<XmlNode>().Select(x => new Video(x)).ToList());
                }
            }
            return null;
        }

        /// <summary>
        ///     Возвращает записи, на которых текущий пользователь поставил отметку "Мне нравится".
        /// </summary>
        /// <param name="count">Количество сообщений, которое необходимо получить (но не более 100).</param>
        /// <param name="offset">Смещение, необходимое для выборки определенного подмножества сообщений.</param>
        public static async Task<ListCount<WallEntity>> GetPosts(int? count = null, int? offset = null)
        {
            VkAPI.Manager.Method("fave.getPosts");
            if (count != null)
            {
                VkAPI.Manager.Params("count", count);
            }
            if (offset != null)
            {
                VkAPI.Manager.Params("offset", offset);
            }

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                var countNode = result.SelectSingleNode("count");
                if (nodes != null && countNode != null)
                {
                    return new ListCount<WallEntity>(countNode.IntVal().Value,
                                                    nodes.Cast<XmlNode>().Select(x => new WallEntity(x)).ToList());
                }
            }
            return null;
        }

        /// <summary>
        ///     Возвращает ссылки, добавленные в закладки текущим пользователем.
        /// </summary>
        public static async Task<ListCount<LinkInfo>> GetLinks()
        {
            VkAPI.Manager.Method("fave.getLinks");

            var apiManager = await VkAPI.Manager.Execute();
            var result = apiManager.GetResponseXml();
            if (VkAPI.Manager.MethodSuccessed)
            {
                var nodes = result.SelectNodes("items/*");
                var countNode = result.SelectSingleNode("count");
                if (nodes != null && countNode != null)
                {
                    return new ListCount<LinkInfo>(countNode.IntVal().Value,
                                                   nodes.Cast<XmlNode>().Select(x => new LinkInfo(x)).ToList());
                }
            }
            return null;
        }
    }
}