using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace ajh657.Backend.Interop
{
    public class CosmosInterop : ICosmosInterop
    {
        private readonly CosmosClient _client;
        private readonly ILogger<CosmosInterop> _logger;

        public CosmosInterop(ILoggerFactory loggerFactory, CosmosClient cosmosClient)
        {
            _logger = loggerFactory.CreateLogger<CosmosInterop>();
            _client = cosmosClient;
        }

        public async Task<ItemResponse<T>?> GetAsync<T>(string databaseName, string containerName, string id, string partitionKey)
        {
            var container = _client.GetContainer(databaseName, containerName);
            var response = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
            return response;
        }

        public async Task<ItemResponse<T>?> CreateAsync<T>(string databaseName, string containerName, T item)
        {
            var container = _client.GetContainer(databaseName, containerName);
            var response = await container.CreateItemAsync(item);
            return response;
        }

        public async Task<ItemResponse<T>?> UpdateAsync<T>(string databaseName, string containerName, string id, string partitionKey, T item)
        {
            var container = _client.GetContainer(databaseName, containerName);
            var response = await container.ReplaceItemAsync(item, id);
            return response;
        }

        public async Task<ItemResponse<T>?> UpsertAsync<T>(string databaseName, string containerName, string id, string partitionKey, T item)
        {
            var container = _client.GetContainer(databaseName, containerName);
            var response = await container.UpsertItemAsync(item);
            return response;
        }

        public async Task Delete(string databaseName, string containerName, string id, string partitionKey)
        {
            var container = _client.GetContainer(databaseName, containerName);
            await container.DeleteItemAsync<object>(id, new PartitionKey(partitionKey));
        }
    }
}
