using MediumServer.Extends;
using MediumServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MediumServer
{
    public class Client
    {
        /// <summary>
        /// Basic Url
        /// </summary>
        private const string _baseUrl = "https://api.medium.com/v1";

        /// <summary>
        /// The Authorize Token
        /// </summary>
        private Token _clientToken = null;

        /// <summary>
        /// Your app client id
        /// </summary>
        private readonly string _clientId;

        /// <summary>
        /// Your app client secret
        /// </summary>
        private readonly string _clientSecret;

        /// <summary>
        /// The helper class about http
        /// </summary>
        private HttpHelper _httpHelper;

        /// <summary>
        /// Handle when app error
        /// </summary>
        public event MediumExceptionHandle<Exception> OnError;

        /// <summary>
        /// Handle when request failed
        /// </summary>
        public event MediumExceptionHandle<MediumErrorResponse> OnRequestFailed;

        /// <summary>
        /// Initialize <see cref="Client"/> class.
        /// </summary>
        /// <param name="clientId">Your app client id</param>
        /// <param name="clientSecret">your app client secret</param>
        public Client(string clientId,string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _httpHelper = new HttpHelper(_baseUrl);
        }

        /// <summary>
        /// Set the token in client
        /// </summary>
        /// <param name="token"></param>
        public void SetToken(Token token)
        {
            _clientToken = token;
            _httpHelper.SetToken(token.AccessToken);
        }

        /// <summary>
        /// Set the Json Serializer Settings
        /// </summary>
        /// <param name="settings"></param>
        public void SetJsonSerializerSettings(JsonSerializerSettings settings)
        {
            _httpHelper.SetJsonSettings(settings);
        }

        /// <summary>
        /// Get Authorize url to get the code in browser
        /// </summary>
        /// <param name="state">Arbitrary text of your choosing, which we will repeat back to you to help you prevent request forgery.</param>
        /// <param name="redirectUri">The URL where we will send the user after they have completed the login dialog. This must exactly match one of the callback URLs you provided when creating your app. This field should be URL encoded.</param>
        /// <param name="scope">The access that your integration is requesting, comma separated. Currently, there are three valid scope values, which are listed below. Most integrations should request basicProfile and publishPost</param>
        /// <returns></returns>
        public string GetAuthorizeUrl(string state,string redirectUri,Scope[] scope)
        {
            //var tempScope = new StringBuilder();
            //foreach (var s in scope)
            //{
            //    tempScope.AppendFormat(",{0}", s);
            //}
            string sc = JsonConvert.SerializeObject(scope).Replace("\"","").TrimStart('[').TrimEnd(']');
            return "https://medium.com/m/oauth/authorize?" +
                $"client_id={_clientId}" +
                $"&scope={sc}" +
                $"&state={state}" +
                $"&response_type=code" +
                $"&redirect_uri={redirectUri}";
        }

        /// <summary>
        /// You got the authorize code, now get the token
        /// </summary>
        /// <param name="code">Authorize code</param>
        /// <param name="redirectUri">The URL where we will send the user after they have completed the login dialog. This must exactly match one of the callback URLs you provided when creating your app. This field should be URL encoded.</param>
        /// <returns></returns>
        public async Task<Token> GetAccessToken(string code, string redirectUri)
        {
            var postParams = new Dictionary<string, string>
            {
                {"code", code},
                {"client_id", _clientId},
                {"client_secret", _clientSecret},
                {"grant_type", "authorization_code"},
                {"redirect_uri", redirectUri}
            };

            try
            {
                return await GetAccessToken(postParams);
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            return null;
        }

        /// <summary>
        /// Get the new token when access token was overdue
        /// </summary>
        /// <param name="refreshToken">Old token's refresh token code</param>
        /// <returns></returns>
        public async Task<Token> GetAccessToken(string refreshToken)
        {
            var postParams = new Dictionary<string, string>
            {
                {"refresh_token", refreshToken},
                {"client_id", _clientId},
                {"client_secret", _clientSecret},
                {"grant_type", "refresh_token"},
            };

            try
            {
                return await GetAccessToken(postParams);
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            return null;
        }

        private async Task<Token> GetAccessToken(Dictionary<string, string> postParams)
        {
            var request = Tools.
                GetRequestWithToken(_baseUrl + "/tokens", HttpMethod.Post, null).
                SetRequestWwwFormUrlUrlencoded(postParams);
            try
            {
                return await request.GetResponse(JsonConvert.DeserializeObject<Token>);
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            return null;
        }

        /// <summary>
        /// Get Current User
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetUser()
        {
            try
            {
                var data = await _httpHelper.GetRequest<User>("/me", true);
                return data;
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            return null;
        }

        /// <summary>
        /// Get publication list with user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<Publication>> GetAllPublications(string userId)
        {
            try
            {
                var data = await _httpHelper.GetRequest<IEnumerable<Publication>>($"/users/{userId}/publications", true);
                return data;
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            return null;
        }

        /// <summary>
        /// Get contributors with the publication
        /// </summary>
        /// <param name="publicationId">Publication Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<Contributor>> GetAllContributors(string publicationId)
        {
            try
            {
                var data = await _httpHelper.GetRequest<IEnumerable<Contributor>>($"/publications/{publicationId}/contributors", true);
                return data;
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            
            return null;
        }

        /// <summary>
        /// Crete a post
        /// </summary>
        /// <param name="userId">Current user id</param>
        /// <param name="requestPost">The post standard entity</param>
        /// <returns></returns>
        public async Task<Post> CreatePost(string userId, RequestPostModel requestPost)
        {
            try
            {
                var postBody = GetContentFromEntity(requestPost);
                return await _httpHelper.PostRequest<Post>($"/users/{userId}/posts", postBody);
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            return null;
        }

        /// <summary>
        /// Quick create a post request
        /// </summary>
        /// <param name="userId">Current user id</param>
        /// <param name="title">Article title</param>
        /// <param name="content">Article content</param>
        /// <param name="tags">Article tags, could be <c>null</c></param>
        /// <param name="format">Article type, markdown or html</param>
        /// <returns></returns>
        public RequestPostModel CreatePostRequest(string userId, string title,string content, string[] tags = null, ContentFormat format=ContentFormat.Markdown)
        {
            var req = new RequestPostModel()
            {
                Title = title,
                Content = content,
                ContentFormat = format,
                Tags = tags,
                PublishStatus = PublishStatus.Public,
                License = License.AllRightsReserved,
                NotifyFollowers = true,
                CanonicalUrl = ""
            };
            return req;
        }

        /// <summary>
        /// Build a post under the publication
        /// </summary>
        /// <param name="publicationId">Publication Id</param>
        /// <param name="requestPost">The post standard entity</param>
        /// <returns></returns>
        public async Task<Post> CreatePostUnderPublication(string publicationId, RequestPostModel requestPost)
        {
            try
            {
                var postBody = GetContentFromEntity(requestPost);
                return await _httpHelper.PostRequest<Post>($"/publications/{publicationId}/posts", postBody);
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            return null;
        }

        /// <summary>
        /// Create the image request
        /// </summary>
        /// <param name="image">Image File</param>
        /// <returns></returns>
        public async Task<RequestImageModel> CreateImageRequest(StorageFile image)
        {
            var extend = System.IO.Path.GetExtension(image.Path);
            ImageContentType type;
            switch (extend.ToLower().TrimStart('.'))
            {
                case "png":
                    type = ImageContentType.Png;
                    break;
                case "jpg":
                case "jpeg":
                    type = ImageContentType.Jpeg;
                    break;
                case "gif":
                    type = ImageContentType.Gif;
                    break;
                case "tiff":
                    type = ImageContentType.Tiff;
                    break;
                default:
                    throw (new InvalidCastException("Not support image type."));
            }
            byte[] ImageBytes;
            using (var stream=await image.OpenAsync(FileAccessMode.Read))
            {
                ImageBytes = Tools.RandomAccessStreamToBytes(stream);
            }
            var req = new RequestImageModel()
            {
                ContentType = type,
                ContentBytes = ImageBytes
            };
            return req;
        }

        /// <summary>
        /// Upload the Image with request
        /// </summary>
        /// <param name="requestImage">The image standard entity</param>
        /// <param name="name">Image name</param>
        /// <returns></returns>
        public async Task<Image> UploadImage(RequestImageModel requestImage,string name="image")
        {
            var request = Tools.
                GetRequestWithToken(
                    _baseUrl + "/images",
                    HttpMethod.Post,
                    _clientToken).
                SetRequestMultipartFormData(
                    requestImage.ContentType.ToString(),
                    requestImage.ContentBytes,
                    name);

            try
            {
                return await request.GetResponseJson<Image>();
            }
            catch (MediumException e)
            {
                OnRequestFailed(e.RequestData);
            }
            catch (Exception e)
            {
                OnError(e);
            }
            return null;
        }

        /// <summary>
        /// Represents a way to handle regular events
        /// </summary>
        /// <typeparam name="TClass">Event Type</typeparam>
        /// <param name="args">Event Data</param>
        public delegate void MediumExceptionHandle<TClass>(TClass args);

        /// <summary>
        /// Translate the entity to <see cref="StringContent"/>
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        private StringContent GetContentFromEntity<T>(T entity)
        {
            var Entity = _httpHelper.JsonSerializerSettings == null ? JsonConvert.SerializeObject(entity) : JsonConvert.SerializeObject(entity, _httpHelper.JsonSerializerSettings);
            var postBody = new StringContent(Entity, Encoding.UTF8, "application/json");
            return postBody;
        }
    }
}
