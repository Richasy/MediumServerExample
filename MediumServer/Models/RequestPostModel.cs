using MediumServer.Extends;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumServer.Models
{
    /// <summary>
    /// Send the request to create the post, this is request body model
    /// </summary>
    public class RequestPostModel
    {
        /// <summary>
        /// The title of the post.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The format of the "content" field. 
        /// </summary>
        [JsonProperty("contentFormat")]
        public ContentFormat ContentFormat { get; set; }

        /// <summary>
        /// The body of the post, in a valid, semantic, HTML fragment, or Markdown. 
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Tags to classify the post. Only the first three will be used. Tags longer than 25 characters will be ignored.
        /// </summary>
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// The original home of this content, if it was originally published elsewhere.
        /// </summary>
        [JsonProperty("canonicalUrl")]
        public string CanonicalUrl { get; set; }

        /// <summary>
        /// The status of the post.The default is “public”.
        /// </summary>
        [JsonProperty("publishStatus")]
        public PublishStatus PublishStatus { get; set; }

        /// <summary>
        /// The license of the post.The default is “all-rights-reserved”.
        /// </summary>
        [JsonProperty("license")]
        public License License { get; set; }

        /// <summary>
        /// Whether to notifyFollowers that the user has published.
        /// </summary>
        [JsonProperty("notifyFollowers")]
        public bool? NotifyFollowers { get; set; }
    }
}
