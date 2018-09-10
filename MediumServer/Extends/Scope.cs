using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MediumServer.Extends
{
    /// <summary>
    /// The access that your integration is requesting
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Scope
    {
        /// <summary>
        /// Grants basic access to a user’s profile (not including their email).
        /// </summary>
        [EnumMember(Value = "basicProfile")]
        BasicProfile,

        /// <summary>
        /// Grants the ability to list publications related to the user.
        /// </summary>
        [EnumMember(Value = "listPublications")]
        ListPublications,

        /// <summary>
        /// Grants the ability to publish a post to the user’s profile.
        /// </summary>
        [EnumMember(Value = "publishPost")]
        PublishPost,

        /// <summary>
        /// [Dont' select this item! Otherwise, you will not be able to properly authorize.]
        /// ---
        /// Grants the ability to upload an image for use within a Medium post.
        /// </summary>
        [EnumMember(Value = "uploadImage")]
        UploadImage,
    }
}
