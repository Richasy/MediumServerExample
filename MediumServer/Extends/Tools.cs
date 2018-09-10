using MediumServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MediumServer.Extends
{
    internal static class Tools
    {
        public static DateTime? FromUnixTimestampMillSeconds(long? timestamp)
        {
            if (!timestamp.HasValue) return null;
            return new DateTime(1970, 1, 1).AddMilliseconds(timestamp.Value);
        }

        public static HttpWebRequest GetRequestWithToken(
            string endpointUrl,
            HttpMethod method,
            Token token)
        {
            var request = WebRequest.Create(endpointUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new ArgumentException("URL is not a valid HTTP URL.", endpointUrl);
            }

            request.Method = method.Method.ToUpper();
            request.Accept = "application/json";
            request.Headers[HttpRequestHeader.AcceptCharset] = "utf-8";

            if (token != null)
                request.Headers["Authorization"] = "Bearer " + token.AccessToken;

            return request;
        }

        public static WebRequest SetRequestJson(this WebRequest request, object obj)
        {
            var requestBodyString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var requestBodyBytes = Encoding.UTF8.GetBytes(requestBodyString);

            request.ContentType = "application/json";
            request.GetRequestStream().Write(requestBodyBytes, 0, requestBodyBytes.Length);

            return request;
        }

        public static WebRequest SetRequestMultipartFormData(
            this WebRequest request,
            string contentType,
            byte[] content,
            string name,
            string boundary = null)
        {
            // if boundary is still empty, generate one
            if (string.IsNullOrWhiteSpace(boundary))
            {
                boundary = Guid.NewGuid().ToString("N");
            }

            var sb = new StringBuilder();
            sb.AppendLine($"--{boundary}");
            sb.AppendLine($"Content-Disposition: form-data; name=\"image\"; filename=\"{name}\"");
            sb.AppendLine($"Content-Type: {contentType}");
            sb.AppendLine();

            var requestBodyPrefixBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var requestBodySuffixBytes = Encoding.UTF8.GetBytes($"{Environment.NewLine}--{boundary}--");

            request.ContentType = $"multipart/form-data; boundary={boundary}";

            var requestStream = request.GetRequestStream();
            requestStream.Write(requestBodyPrefixBytes, 0, requestBodyPrefixBytes.Length);
            requestStream.Write(content, 0, content.Length);
            requestStream.Write(requestBodySuffixBytes, 0, requestBodySuffixBytes.Length);

            return request;
        }
        public static WebRequest SetRequestWwwFormUrlUrlencoded(
            this WebRequest request,
            Dictionary<string, string> postParams)
        {
            var requestBodyString = GenerateWwwFormUrlEncodedString(postParams);
            var requestBodyBytes = Encoding.UTF8.GetBytes(requestBodyString);

            request.ContentType = "application/x-www-form-urlencoded";
            request.GetRequestStream().Write(requestBodyBytes, 0, requestBodyBytes.Length);

            return request;
        }
        
        public static async Task<T> GetResponseJson<T>(this WebRequest request) where T : class
        {
            return (await request.GetResponse(Newtonsoft.Json.JsonConvert.DeserializeObject<T>));
        }
        
        public static async Task<T> GetResponse<T>(
            this WebRequest request,
            Func<string, T> responseBodyParser)
        {
            try
            {
                using (var response = await request.GetResponseAsync())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream == null)
                        return default(T);

                    return responseBodyParser(responseStream.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                var responseStream = ex.Response?.GetResponseStream();
                if (responseStream != null)
                {
                    var responseBody = responseStream.ReadToEnd();
                    throw new InvalidOperationException(responseBody, ex);
                }
                throw;
            }
        }
        
        public static string ReadToEnd(
            this Stream stream,
            bool seekToStart = true,
            Encoding encoding = null)
        {
            if (stream.CanSeek && seekToStart)
                stream.Seek(0, SeekOrigin.Begin);

            return (encoding == null ? new StreamReader(stream) : new StreamReader(stream, encoding))
                .ReadToEnd();
        }
        
        public static string ConcatenateString(this IEnumerable<string> strings, string delimiter)
        {
            var sb = new StringBuilder();
            foreach (var str in strings)
            {
                if (sb.Length > 0)
                    sb.Append(delimiter);
                sb.Append(str);
            }
            return sb.ToString();
        }
        
        public static string GenerateWwwFormUrlEncodedString(Dictionary<string, string> parameters)
        {
            return parameters.
                Select(p => $"{p.Key}={System.Net.WebUtility.UrlEncode(p.Value)}").
                ConcatenateString("&");
        }

        private static Stream GetRequestStream(this WebRequest request)
        {
            Task<Stream> task = request.GetRequestStreamAsync();
            while (!task.IsCompleted) { }
            return task.Result;
        }

        private static WebResponse GetResponse(this WebRequest request)
        {
            Task<WebResponse> task = request.GetResponseAsync();
            while (!task.IsCompleted) { }
            return task.Result;
        }

        public static byte[] RandomAccessStreamToBytes(IRandomAccessStream randomStream)
        {
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(randomStream.GetInputStreamAt(0));
            MemoryStream memoryStream = new MemoryStream();
            if (stream != null)
            {
                byte[] bytes = StreamToBytes(stream);
                return bytes;
            }
            return null;
        }

        private static byte[] StreamToBytes(Stream stream)
        {
            if (stream.CanSeek) // stream.Length is sure
            {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                return bytes;
            }
            else // stream.Length not sure
            {
                int initialLength = 32768; // 32k

                byte[] buffer = new byte[initialLength];
                int read = 0;

                int chunk;
                while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                {
                    read += chunk;

                    if (read == buffer.Length)
                    {
                        int nextByte = stream.ReadByte();

                        if (nextByte == -1)
                        {
                            return buffer;
                        }

                        byte[] newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        newBuffer[read] = (byte)nextByte;
                        buffer = newBuffer;
                        read++;
                    }
                }

                byte[] ret = new byte[read];
                Array.Copy(buffer, ret, read);
                return ret;
            }
        }
    }
}
