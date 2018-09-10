using MediumServer.Extends;
using Newtonsoft.Json;

namespace MediumServer.Models
{
    /// <summary>
    /// Contributor for a given publication
    /// </summary>
    public class Contributor
    {
        /// <summary>
        /// An ID for the publication. This can be lifted from response of publications above
        /// </summary>
        [JsonProperty("publicationId")]
        public string PublicationId { get; set; }

        /// <summary>
        /// A user ID of the contributor.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Role of the user identified by userId in the publication identified by publicationId. 
        /// 'editor' or 'writer'
        /// </summary>
        [JsonProperty("role")]
        public Role Role { get; set; }
    }
}
