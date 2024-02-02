namespace ajh657.Common.Data.Frontend
{
    public static class Util
    {
        public static string? GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name);
    }
}
