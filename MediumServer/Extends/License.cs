using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace MediumServer.Extends
{
    /// <summary>
    /// The license of the post.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum License
    {
        [EnumMember(Value = "all-rights-reserved")]
        AllRightsReserved = 0,

        [EnumMember(Value = "cc-40-by")]
        Cc40By,

        [EnumMember(Value = "cc-40-by-sa")]
        Cc40BySa,

        [EnumMember(Value = "cc-40-by-nd")]
        Cc40ByNd,

        [EnumMember(Value = "cc-40-by-nc")]
        Cc40ByNc,

        [EnumMember(Value = "cc-40-by-nc-nd")]
        Cc40ByNcNd,

        [EnumMember(Value = "cc-40-by-nc-sa")]
        Cc40ByNcSa,

        [EnumMember(Value = "cc-40-zero")]
        Cc40Zero,

        [EnumMember(Value = "public-domain")]
        PublicDomain
    }
}
