using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediumServer.Models
{

    public class MediumException : Exception
    {
        // <summary>
        /// default constructor
        /// </summary>
        /// <param name="request">additional request info</param>
        public MediumException(MediumErrorResponse request) : base() { RequestData = request; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">additional message</param>
        /// <param name="request">additional request info</param>
        public MediumException(string message, MediumErrorResponse request) : base(message) { RequestData = request; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">additional message</param>
        /// <param name="inner">inner exception</param>
        /// <param name="request">additional request info</param>
        public MediumException(string message, Exception inner, MediumErrorResponse request) : base(message, inner) { RequestData = request; }

        /// <summary>
        /// Bad request data info
        /// </summary>
        public MediumErrorResponse RequestData { get; set; }
    }

    /// <summary>
    /// The model when request failed
    /// </summary>
    public class MediumErrorResponse
    {
        /// <summary>
        /// Error List
        /// </summary>
        [JsonProperty("errors")]
        public MediumError[] Errors { get; set; }
    }

    /// <summary>
    /// The model with error info
    /// </summary>
    public class MediumError
    {
        /// <summary>
        /// Error Code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
