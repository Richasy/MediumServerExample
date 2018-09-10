using MediumServer.Extends;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumServer.Models
{
    /// <summary>
    /// Send the request to create the image, this is request body model
    /// </summary>
    public class RequestImageModel
    {
        /// <summary>
        /// Image Type
        /// </summary>
        [JsonProperty("Content-Type")]
        public ImageContentType ContentType { get; set; }

        /// <summary>
        /// Image Byte Array
        /// </summary>
        [JsonProperty("Content-Disposition")]
        public byte[] ContentBytes { get; set; }
    }
}
