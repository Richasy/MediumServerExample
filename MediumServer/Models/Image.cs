using Newtonsoft.Json;

namespace MediumServer.Models
{
    /// <summary>
    /// The image resource which Medium return
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Image url in Medium
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Resource Md5 code
        /// </summary>
        [JsonProperty("md5")]
        public string Md5 { get; set; }
    }
}
