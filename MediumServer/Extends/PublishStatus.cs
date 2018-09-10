using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace MediumServer.Extends
{
    /// <summary>
    /// Status of publish
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PublishStatus
    {
        /// <summary>
        /// Everyone can see it.
        /// </summary>
        [EnumMember(Value = "public")]
        Public,

        /// <summary>
        /// Not released for the time being, it may be modified.
        /// </summary>
        [EnumMember(Value = "draft")]
        Draft,

        /// <summary>
        /// Not show in the list
        /// </summary>
        [EnumMember(Value = "unlisted")]
        Unlisted
    }
}
