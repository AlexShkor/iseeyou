﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using ISeeYou.Vk.Dto;
using ISeeYou.Vk.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ISeeYou.Vk.Api
{
    public class VkApi
    {
        private const string BaseApiCallurl = "https://api.vk.com/method/";
        protected string AccessToken;

        public VkApi(string accessToken = null)
        {
            AccessToken = accessToken;
        }

        public List<VkUser> GetUsers(string[] fields, string[]uids)
        {
            var parametrs = new NameValueCollection { { "uids", string.Join(",", uids) }, { "fields", string.Join(",", fields) } };
            var json = Call("users.get", parametrs);

            return Parse<List<VkUser>>(json);  
        }

        public List<VkUser> GetUserFriends(string uid, string[] fields)
        {
            var parametrs = new NameValueCollection
            {
                {"user_id", uid},
                {"fields", string.Join(",", fields)}
            };
            var json = Call("friends.get", parametrs);

            return Parse<List<VkUser>>(json);  
        }

        public void Post(string ownerId, string message, string url,VkSaveWallPhotoResult photo)
        {
            var parametrs = new NameValueCollection
            {
                { "owner_id", ownerId }, 
                { "message", message }, 
                { "from_group", "1" }, 
                { "signed", "1" },
                { "attachments" , url + "," + photo.id}
            };
            var json = Call("wall.post", parametrs);
            var result = Parse<VkPost>(json);
        }


        public VkCity GetCity(string id)
        {
            var parametrs = new NameValueCollection { { "cids", string.Join(",", new[] { id }) } };
            var json = Call("places.getCityById", parametrs);

            return Parse<List<VkCity>>(json).First();
        }

        public VkCountry GetCountry(string id)
        {
            var parametrs = new NameValueCollection { { "cids", string.Join(",", new[] { id }) } };
            var json = Call("places.getCountryById", parametrs);

            return Parse<List<VkCountry>>(json).First();
        }

        public VkGroupInfo GetGroupInfo(string id)
        {
            var parametrs = new NameValueCollection { { "gids", id } };
            var json = Call("groups.getById", parametrs);

            return Parse<List<VkGroupInfo>>(json)[0];
        }

        private T Parse<T>(string json)
        {
            var response = JObject.Parse(json);
            var error = response.SelectToken("error");
            if (error != null)
            {
                throw new VkResponseException(error.ToString());
            }
            json = response.SelectToken("response").ToString();
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        private List<int> ParseUsers(string json)
        {
            var response = JObject.Parse(json);
            var error = response.SelectToken("error");
            if (error != null)
            {
                throw new VkResponseException(error.ToString());
            }
            json = response.SelectToken("response").SelectToken("users").ToString();
            return JsonConvert.DeserializeObject<List<int>>(json);
        }

        private IEnumerable<T> ParseListing<T>(string json)
        {
            var response = JObject.Parse(json);
            var error = response.SelectToken("error");
            if (error != null)
            {
                throw new VkResponseException(error.ToString());
            }
            var jobject = response.SelectToken("response");
            var totalScount = jobject[0];
            var index = 1;
            JToken jtoken = null;
            
            while (true)
            {
                try
                {
                    jtoken = jobject[index];
                }
                catch (Exception)
                {
                    jtoken = null;
                }
                if (jtoken != null)
                {
                    var str = jtoken.ToString();
                yield return jobject[index].ToObject<T>();
                index++;
                }
                else
                {
                    yield break;
                }
            }
        }

        private string Call(string methodName, NameValueCollection parametrs, string method = "POST")
        {
            if (!string.IsNullOrEmpty(AccessToken))
            {
                parametrs.Add("access_token", AccessToken);
            }
            var postData = new StringBuilder();

            foreach (var key in parametrs.AllKeys)
            {
                postData.Append(key + "=" + parametrs[key] + "&");
            }
            // remove last character
            if (postData.Length != 0)
            {
                postData.Remove(postData.Length - 1, 1);
            }

            var url = BaseApiCallurl + methodName;

            var request = new VkWebRequest(url, method, postData.ToString());
            return request.GetResponse();
        }

        public VkUploadServer GetUploadServer(string groupId)
        {
            var json = Call("photos.getWallUploadServer", new NameValueCollection() {{"group_id", groupId}});
            return Parse<VkUploadServer>(json);
        }

        public VkSaveWallPhotoResult SaveWallPhoto(string groupId, VkUploadFileResult vkUploadFileResult)
        {
            var json = Call("photos.saveWallPhoto", new NameValueCollection()
            {
                { "group_id", groupId },
                { "server", vkUploadFileResult.server },
                { "photo", vkUploadFileResult.photo },
                { "hash", vkUploadFileResult.hash },
            });
            return Parse<VkSaveWallPhotoResult[]>(json)[0];
   
        }

        public VkUploadFileResult UploadImage(string imgUrl, string uploadUrl)
        {
            using (var client = new WebClient())
            {
                var data = client.DownloadData(imgUrl);

            var Request = (HttpWebRequest) WebRequest.Create(uploadUrl);
           
                Stream _stream;
                string _boundary = String.Format("--{0}", MD5.Create());
                string _templateFile = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";
                string _templateEnd = "--{0}--\r\n\r\n";
                Request.Method = "POST";
                Request.ContentType = String.Format("multipart/form-data; boundary={0}", _boundary);
                _stream = Request.GetRequestStream();
                string FilePath = "test.jpg";
                string FileType = "application/octet-stream";
                string Name = "file1";
                byte[] contentFile = Encoding.UTF8.GetBytes(String.Format(_templateFile, _boundary, Name, FilePath, FileType));
                _stream.Write(contentFile, 0, contentFile.Length);
                _stream.Write(data, 0, data.Length);
                byte[] _lineFeed = Encoding.UTF8.GetBytes("\r\n");
                _stream.Write(_lineFeed, 0, _lineFeed.Length);
                byte[] contentEnd = Encoding.UTF8.GetBytes(String.Format(_templateEnd, _boundary));
                _stream.Write(contentEnd, 0, contentEnd.Length);
                HttpWebResponse webResponse = (HttpWebResponse)Request.GetResponse();
                StreamReader read = new StreamReader(webResponse.GetResponseStream());
                var json = read.ReadToEnd();
                return JsonConvert.DeserializeObject<VkUploadFileResult>(json);
            }
        }

        public List<PhotoDto> GetPhotos(int sourceId, string albumId)
        {
            var json = Call("photos.get", new NameValueCollection()
            {
                { "owner_id", sourceId.ToString(CultureInfo.InvariantCulture) },
                { "album_id", albumId},
                { "offset", "0"},
                { "count", "200"},
            });
            return Parse<List<PhotoDto>>(json);
        }

        public List<WallPost> GetWall(int userId)
        {
            var json = Call("wall.get", new NameValueCollection()
            {
                { "owner_id", userId.ToString(CultureInfo.InvariantCulture) },
                   { "offset", "0"},
                { "count", "100"},
                { "filter", "posted_photo"},
            });
            return ParseListing<WallPost>(json).ToList();
        }

        public List<int> Likes(long itemId, long ownerId)
        {
            var json = Call("likes.getList", new NameValueCollection()
            {
                {"type", "photo"},
                {"item_id", itemId.ToString(CultureInfo.InvariantCulture)},
                {"owner_id", ownerId.ToString(CultureInfo.InvariantCulture)},
                {"offset", "0"},
                {"count", "1000"},
            });
            return ParseUsers(json).ToList();
        }

        public IEnumerable<PhotoAlbum> GetAlbums(int ownerId)
        {
            var json = Call("photos.getAlbums", new NameValueCollection()
            {
                {"owner_id", ownerId.ToString(CultureInfo.InvariantCulture)},
            });
            return ParseListing<PhotoAlbum>(json).ToList();
        }
    }

    public class WallPost
    {
        public long id { get; set; }
        public long date { get; set; }
        public long owner_id { get; set; }
        public string text { get; set; }
        public List<AttachmetVk> attachments { get; set; } 

    }

    public class AttachmetVk
    {
        public string type { get; set; }

        public PhotoAttachment photo { get; set; }
    }

    public class PhotoDto
    {
        public int pid { get; set; }
        public string aid { get; set; }
        public string src { get; set; }
        public string src_big { get; set; }
        public string text { get; set; }
        public int created { get; set; }
        public int post_id { get; set; }
    }

    public class PhotoAttachment
    {
        public int id { get; set; }
        public string photo_604 { get; set; }
        public string photo_130 { get; set; }
        public long owner_id { get; set; }
    }


    public class VkUploadFileResult
    {
        public string server { get; set; }
        public string photo { get; set; }
        public string hash { get; set; }
    }

    public class VkSaveWallPhotoResult
    {
        public string id;
        public string aid;
        public string pid;
        public string owner_id;
        public string src;
    }
    public class VkUploadServer
    {
        public string upload_url;
        public string aid;
        public string mid;
    }
}
