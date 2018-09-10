using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumServer.Models
{
    /// <summary>
    /// The user in the Medium
    /// </summary>
    public class User
    {
        /// <summary>
        /// User Id.
        /// </summary>
        /// <remarks>
        /// User's unique identity on Medium.
        /// </remarks>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The user’s username on Medium.
        /// </summary>
        /// <remarks>
        /// Is not show name on profile
        /// </remarks>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// The user’s name on Medium.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The URL to the user’s profile on Medium.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The URL to the user’s avatar on Medium
        /// </summary>
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
