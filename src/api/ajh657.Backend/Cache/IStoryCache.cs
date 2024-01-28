using System.Collections.Generic;
using System.Threading.Tasks;
using ajh657.Common.Data.Records;

namespace ajh657.Backend.Cache
{
    public interface IStoryCache
    {
        Task<IEnumerable<Story>> GetStoriesAsync();
        Task<CacheItem<Story[]>> RebuildStoryCache();
    }
}