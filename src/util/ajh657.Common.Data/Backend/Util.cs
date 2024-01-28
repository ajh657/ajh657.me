using Microsoft.Azure.Functions.Worker.Http;

namespace ajh657.Common.Data.Backend
{
    public static class Util
    {
        public static string? GetIpFromRequestHeaders(HttpHeadersCollection headers)
        {
            string? ip = null;
            if (headers.TryGetValues("X-Forwarded-For", out var values))
            {
                var header = values.FirstOrDefault();
                if (!string.IsNullOrEmpty(header))
                {
                    var addresses = header.Split([',']).FirstOrDefault();
                    if (!string.IsNullOrEmpty(addresses))
                    {
                        ip = addresses.Split([':']).FirstOrDefault();
                    }
                }
            }

            return ip;
        }

        public static string? GetUserAgentFromRequestHeaders(HttpHeadersCollection headers)
        {
            string? userAgent = null;
            if (headers.TryGetValues("User-Agent", out var values))
            {
                userAgent = values.FirstOrDefault();
            }

            return userAgent;
        }
    }
}
