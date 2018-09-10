using MediumServer;
using MediumServer.Extends;
using MediumServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace MediumServerExample.Tools
{
    public class MediumHelper
    {
        private const string clientId = "your_client_id";
        private const string clientSecret = "your_client_secret";
        private const string callbackUri = "your_callback_uri";
        private Scope[] scope = { Scope.BasicProfile,Scope.PublishPost,Scope.ListPublications };

        public Client _client = new Client(clientId, clientSecret);
        public User user = null;

        /// <summary>
        /// Get url to authorize
        /// </summary>
        /// <returns></returns>
        public string GetAuthUrl()
        {
            string url = _client.GetAuthorizeUrl("secretstate", callbackUri, scope);
            return url;
        }

        /// <summary>
        /// Client Init
        /// </summary>
        /// <param name="code">if authorize, write the code.</param>
        /// <param name="isIntegrationToken">Is authorize by integration token?</param>
        /// <returns></returns>
        public async Task FillClient(string code = "",bool isIntegrationToken=false)
        {
            var token = new Token();
            _client.OnError -= OnProgramError;
            _client.OnRequestFailed -= OnRequestFailed;
            _client.OnError += OnProgramError;
            _client.OnRequestFailed += OnRequestFailed;
            if (String.IsNullOrEmpty(code))
            {
                token = await IOTools.GetMediumToken();
                if (token != null)
                {
                    _client.SetToken(token);
                }
                else
                {
                    throw new ArgumentNullException("Authorize code can't be empty.");
                }
            }
            else
            {
                if (isIntegrationToken)
                {
                    token = new Token(code);
                    _client.SetToken(token);
                }
                else
                {
                    token = await _client.GetAccessToken(code, callbackUri);
                    if(token!=null && !String.IsNullOrEmpty(token.AccessToken))
                    {
                        _client.SetToken(token);
                    }
                    else
                    {
                        throw new Exception("Request Failed.");
                    }
                }
            }
            user = await _client.GetUser();
            if (user != null)
            {
                if (!String.IsNullOrEmpty(code))
                {
                    await IOTools.SaveMediumToken(token);
                    AppTools.WriteLocalSetting(Settings.IsAuthorized, "True");
                    AppTools.WriteLocalSetting(Settings.IsIntegrationToken, isIntegrationToken.ToString());
                }
            }
        }

        /// <summary>
        /// Get All Publications
        /// </summary>
        /// <returns></returns>
        public async Task<List<Publication>> GetAllPublictions()
        {
            var result = (await _client.GetAllPublications(user.Id)).ToList();
            return result;
        }

        /// <summary>
        /// Get All Contributors
        /// </summary>
        /// <returns></returns>
        public async Task<List<Contributor>> GetAllContributors()
        {
            var result = (await _client.GetAllContributors(user.Id)).ToList();
            return result;
        }

        /// <summary>
        /// Create a post to upload
        /// </summary>
        /// <param name="content">Article content</param>
        /// <param name="title">Article title</param>
        /// <param name="status">Publish status</param>
        /// <param name="publicationId">if under the publication, write it</param>
        /// <param name="tags">whats tags</param>
        /// <returns></returns>
        public async Task<bool> CreatePost(string content,string title, PublishStatus status, ContentFormat format, string publicationId="",string[] tags = null)
        {
            if (String.IsNullOrEmpty(publicationId))
            {
                var post = await _client.CreatePost(user.Id, new RequestPostModel
                {
                    Title = title,
                    ContentFormat = format,
                    Tags = tags,
                    Content = content,
                    PublishStatus = status,
                });
                return post == null ? false : true;
            }
            else
            {
                var post = await _client.CreatePostUnderPublication(publicationId, new RequestPostModel
                {
                    Title = title,
                    ContentFormat = ContentFormat.Markdown,
                    Tags = tags,
                    Content = content,
                    PublishStatus = status,
                });
                return post == null ? false : true;
            }
        }

        /// <summary>
        /// Upload Image to Medium
        /// </summary>
        /// <param name="image">Image File</param>
        /// <param name="name">Image name</param>
        /// <returns></returns>
        public async Task<Image> UploadImage(StorageFile image,string name)
        {
            var req = await _client.CreateImageRequest(image);
            var result = await _client.UploadImage(req, name);
            return result;
        }

        /// <summary>
        /// Client Binding Event
        /// </summary>
        /// <param name="ex"><see cref="Exception"/> Info</param>
        public void OnProgramError(Exception ex)
        {
            new Component.PopupToast(ex.Message).ShowPopup();
        }

        /// <summary>
        /// Client Binding Event
        /// </summary>
        /// <param name="args"><see cref="MediumErrorResponse"/> Info</param>
        private void OnRequestFailed(MediumErrorResponse args)
        {
            string msg = "";
            foreach (var item in args.Errors)
            {
                msg += item.Message + "\n";
            }
            new Component.PopupToast(msg).ShowPopup();
        }
    }
}
