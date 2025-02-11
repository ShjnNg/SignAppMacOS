using System.Text.Json.Serialization;

namespace Hoiana.Office.Api.Base
{
    /// <summary>
    /// Base Model Reponse
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponse<T>
    {
        /// <summary>
        /// Status code
        /// </summary>
        [JsonPropertyName("status")]
        public int Status { get; set; } = 200;

        /// <summary>
        /// Message
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
