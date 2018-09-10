using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace MediumServer.Extends
{
    /// <summary>
    /// Role of the user identified
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Role
    {
        /// <summary>
        /// The man who changed something
        /// </summary>
        [EnumMember(Value = "editor")]
        Editor,

        /// <summary>
        /// The man who write this article
        /// </summary>
        [EnumMember(Value = "writer")]
        Writer,
    }
}
