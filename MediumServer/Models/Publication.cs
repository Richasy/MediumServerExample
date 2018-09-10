using Newtonsoft.Json;

namespace MediumServer.Models
{
    /// <summary>
    /// Post Collection? nope, the publication in Medium
    /// </summary>
    public class Publication
    {
        /// <summary>
        /// A unique identifier for the publication.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The publication’s name on Medium.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Short description of the publication
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The URL to the publication’s homepage
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The URL to the publication’s image/logo
        /// </summary>
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Show the publication name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
