namespace ajh657.Common.Data.Records
{
    public record Story
    {
        public required string Title { get; init; }
        public required string StorageUrl { get; init; }
    }
}
