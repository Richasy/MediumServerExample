using MediumServer.Extends;
using Newtonsoft.Json;
using System;

namespace MediumServer.Models
{
    /// <summary>
    /// The article in the medium
    /// </summary>
    public class Post
    {
        /// <summary>
        /// A unique identifier for the post.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// ID of the publication this post was created under. This matches the publication ID requested in the URL of the request
        /// </summary>
        [JsonProperty("publicationId")]
        public string PublicationId { get; set; }

        /// <summary>
        /// The post’s title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The userId of the post’s author
        /// </summary>
        [JsonProperty("authorId")]
        public string AuthorId { get; set; }

        /// <summary>
        /// The post’s tags
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// The URL of the post on Medium
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The canonical URL of the post. 
        /// If canonicalUrl was not specified in the creation of the post
        /// this field will not be present.
        /// </summary>
        [JsonProperty("canonicalUrl	")]
        public string CanonicalUrl { get; set; }

        /// <summary>
        /// The publish status of the post.
        /// </summary>
        [JsonProperty("publishStatus")]
        public PublishStatus PublishStatus { get; set; }

        /// <summary>
        /// The post’s published date. If created as a draft, this field will not be present.
        /// </summary>
        [JsonProperty("publishedAt")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// The license of the post.The default is “all-rights-reserved”.
        /// </summary>
        [JsonProperty("license")]
        public License License { get; set; }

        /// <summary>
        /// The URL to the license of the post.
        /// </summary>
        [JsonProperty("licenseUrl")]
        public string LicenseUrl { get; set; }
    }
}
