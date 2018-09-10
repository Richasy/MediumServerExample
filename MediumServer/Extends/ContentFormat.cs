using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace MediumServer.Extends
{
    /// <summary>
    /// The format of the "content" field. There are two valid values, "html", and "markdown"
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContentFormat
    {
        /// <summary>
        /// Use HTML to upload post
        /// </summary>
        [EnumMember(Value = "html")]
        HTML,

        /// <summary>
        /// Use Markdown to upload post
        /// </summary>
        [EnumMember(Value = "markdown")]
        Markdown
    }
}
