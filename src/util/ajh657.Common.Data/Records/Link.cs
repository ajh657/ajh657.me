namespace ajh657.Common.Data.Records
{
    public record Link
    {
        public required string IconClass { get; init; }
        public required string Name { get; init; }
        public string URL { get; init; } = "";
    }
}
