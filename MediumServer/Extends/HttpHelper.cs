using MediumServer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MediumServer.Extends
{
    internal class HttpHelper
    {
        private static HttpClient _httpClient = new HttpClient();

        private string _baseUrl;

        /// <summary>
        /// AccessToken which could use
        /// </summary>
        private string _accessToken;

        /// <summary>
        /// Serialization/Deserialization settings for Json.NET library
        /// https://www.newtonsoft.com/json/help/html/SerializationSettings.htm
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        /// <summary>
        /// Initialize <see cref="HttpHelper"/> class.
        /// </summary>
        /// <param name="baseUrl">Basic Url</param>
        public HttpHelper(string baseUrl)
        {
            _baseUrl = baseUrl;
            JsonSerializerSettings = new JsonSerializerSettings();
            JsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            JsonSerializerSettings.ContractResolver=new CamelCasePropertyNamesContractResolver();
        }

        /// <summary>
        /// Get model from Medium
        /// </summary>
        /// <typeparam name="T">Which model you need</typeparam>
        /// <param name="route">Secondary route</param>
        /// <param name="isAuthRequired">Need Access Token? Default value is <c>false</c></param>
        /// <returns></returns>
        internal async Task<T> GetRequest<T>(string route,bool isAuthRequired = false)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);

            if (isAuthRequired)
            {
                if (_httpClient.DefaultRequestHeaders.Authorization == null || _httpClient.DefaultRequestHeaders.Authorization.Parameter != _accessToken)
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }

            try
            {
                response = await _httpClient.GetAsync($"{_baseUrl}{route}").ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    if (JsonSerializerSettings != null)
                        return JsonConvert.DeserializeObject<Response<T>>(responseString, JsonSerializerSettings).Data;
                    return JsonConvert.DeserializeObject<Response<T>>(responseString).Data;
                }
                else
                {
                    Debug.WriteLine(responseString);
                    var badrequest = JsonConvert.DeserializeObject<MediumErrorResponse>(responseString);
                    throw new MediumException(badrequest);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Post the data to Medium, and get model from response
        /// </summary>
        /// <typeparam name="T">Which model you need</typeparam>
        /// <param name="route">Secondary route</param>
        /// <param name="postBody">The <see cref="HttpContent"/> data</param>
        /// <param name="isAuthRequired">Need Access Token? Default value is <c>true</c></param>
        /// <returns></returns>
        internal async Task<T> PostRequest<T>(string route,HttpContent postBody,bool isAuthRequired = true)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);

            if (isAuthRequired)
            {
                if (_httpClient.DefaultRequestHeaders.Authorization == null || _httpClient.DefaultRequestHeaders.Authorization.Parameter != _accessToken)
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }

            try
            {
                response = await _httpClient.PostAsync($"{_baseUrl}{route}",postBody).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    if (JsonSerializerSettings != null)
                        return JsonConvert.DeserializeObject<Response<T>>(responseString, JsonSerializerSettings).Data;
                    return JsonConvert.DeserializeObject<Response<T>>(responseString).Data;
                }
                else
                {
                    Debug.WriteLine(responseString);
                    var badrequest = JsonConvert.DeserializeObject<MediumErrorResponse>(responseString);
                    throw new MediumException(badrequest);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Set client token
        /// </summary>
        /// <param name="accessToken"></param>
        public void SetToken(string accessToken)
        {
            this._accessToken = accessToken;
        }

        /// <summary>
        /// Set client json serializer settings
        /// </summary>
        /// <param name="settings"></param>
        public void SetJsonSettings(JsonSerializerSettings settings)
        {
            this.JsonSerializerSettings = settings;
        }

        
    }
}
