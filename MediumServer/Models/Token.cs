using MediumServer.Extends;
using Newtonsoft.Json;
using System;

namespace MediumServer.Models
{
    public class Token
    {
        /// <summary>
        /// The literal string "Bearer"
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// A token that is valid for 60 days and may be used to perform authenticated requests on behalf of the user.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// A token that does not expire which may be used to acquire a new access_token.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// The scopes granted to your integration.
        /// </summary>
        [JsonProperty("scope")]
        public Scope[] Scope { get; set; }

        /// <summary>
        /// The timestamp in unix time when the access token will expire.
        /// </summary>
        [JsonProperty("expires_at")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime ExpiresAt { get; set; }

        public Token()
        {

        }

        /// <summary>
        /// Build Token by integration token
        /// </summary>
        /// <param name="IntegrationToken">Customer Integration Token</param>
        public Token(string IntegrationToken)
        {
            TokenType = "Bearer";
            AccessToken = IntegrationToken;
            RefreshToken = null;
            Scope = new Scope[] { Extends.Scope.BasicProfile, Extends.Scope.PublishPost };
            ExpiresAt = new DateTime(2100, 1, 1);
        }
    }
}
