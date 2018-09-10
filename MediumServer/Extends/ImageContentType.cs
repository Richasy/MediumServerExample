using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace MediumServer.Extends
{
    /// <summary>
    /// Medium support image types
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ImageContentType
    {
        /// <summary>
        /// Jpeg image
        /// </summary>
        [EnumMember(Value = "image/jpeg")]
        Jpeg,

        /// <summary>
        /// Png image
        /// </summary>
        [EnumMember(Value = "image/png")]
        Png,

        /// <summary>
        /// Gif image
        /// </summary>
        [EnumMember(Value = "image/gif")]
        Gif,

        /// <summary>
        /// Tiff image
        /// </summary>
        [EnumMember(Value = "image/tiff")]
        Tiff,
    }
}
