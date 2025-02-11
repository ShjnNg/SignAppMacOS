using System.Text.Json.Serialization;

namespace Hoiana.Office.Api.Models
{
    public class FileUploadResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; } = string.Empty;

        [JsonPropertyName("extension")]
        public string Extension { get; set; } = string.Empty;

        [JsonPropertyName("length")]
        public long Length { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("application")]
        public string Application { get; set; } = string.Empty;

        [JsonIgnore]
        public byte[] Content { get; set; } = [];
    }
}
