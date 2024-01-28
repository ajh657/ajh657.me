using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ajh657.Backend.Interop;
using ajh657.Common.Data.Records;
using ajh657.Common.Strings.Backend;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;

namespace ajh657.Backend.Cache
{
    public class StoryCache : IStoryCache
    {
        private readonly BlobContainerClient _storyContainerClient;
        private readonly ICosmosInterop _cosmosInterop;
        public StoryCache(ICosmosInterop cosmosInterop, IAzureClientFactory<BlobServiceClient> blobClientFactory)
        {
            _cosmosInterop = cosmosInterop;
            _storyContainerClient = blobClientFactory.CreateClient("StorageClient").GetBlobContainerClient("stories");
        }

        public async Task<IEnumerable<Story>> GetStoriesAsync()
        {
            var cacheItemResponce = await _cosmosInterop.GetAsync<CacheItem<Story[]>>(Strings.CosmosDataBase,
                                                                                      Strings.CosmosContainerCache,
                                                                                      Strings.CosmosStoryCacheItemId,
                                                                                      CacheType.Story.ToString());
            var cacheItem = cacheItemResponce?.Resource;

            cacheItem ??= await RebuildStoryCache();

            if (cacheItem.data == null)
            {
                cacheItem = await RebuildStoryCache();
            }

            return cacheItem.data;

        }

        public async Task<CacheItem<Story[]>> RebuildStoryCache()
        {
            var storiesSegemnts = _storyContainerClient.GetBlobsAsync().AsPages(default, 5);

            var stories = new List<Story>();

            await foreach (var blobPage in storiesSegemnts)
            {
                foreach (var blobRef in blobPage.Values)
                {

                    var blobClient = _storyContainerClient.GetBlobClient(blobRef.Name);

                    var tagsResponce = await blobClient.GetTagsAsync();

                    string title;

                    try
                    {
                        title = tagsResponce.Value.Tags["Title"];
                    }
                    catch (KeyNotFoundException)
                    {
                        title = blobRef.Name;
                    }

                    stories.Add(new Story
                    {
                        Title = title,
                        StorageUrl = blobClient.Uri.ToString()
                    });
                }
            }

            var oldCacheResponce = await _cosmosInterop.GetAsync<CacheItem<Story[]>>(Strings.CosmosDataBase, Strings.CosmosContainerCache, Strings.CosmosStoryCacheItemId, CacheType.Story.ToString());
            var oldCache = oldCacheResponce?.Resource;

            var id = oldCache?.id ?? Guid.NewGuid().ToString();

            var cacheItem = new CacheItem<Story[]>
            {
                id = id,
                cacheType = CacheType.Story,
                data = stories.ToArray()
            };

            await _cosmosInterop.UpsertAsync(Strings.CosmosDataBase, Strings.CosmosContainerCache, Strings.CosmosStoryCacheItemId, CacheType.Story.ToString(), cacheItem);

            return cacheItem;
        }
    }
}
