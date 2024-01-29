using System.Text.Json.Serialization;

namespace ajh657.Common.Data.Records
{
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum CacheType
    {
        Unknown,
        Story
    }

    public record CacheItem<T>
    {
        public required string id { get; init; } = Guid.NewGuid().ToString();
        [JsonPropertyName("/cacheType")]
        public required CacheType cacheType { get; init; } = CacheType.Unknown;
        public required T data { get; init; }
        public DateTime ExpiryDate { get; init; } = DateTime.UtcNow.AddDays(7);
        public bool ManualExpiry { get; init; } = false;
    }
}
