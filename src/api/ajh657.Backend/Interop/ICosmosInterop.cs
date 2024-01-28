using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace ajh657.Backend.Interop
{
    public interface ICosmosInterop
    {
        Task<ItemResponse<T>?> CreateAsync<T>(string databaseName, string containerName, T item);
        Task Delete(string databaseName, string containerName, string id, string partitionKey);
        Task<ItemResponse<T>?> GetAsync<T>(string databaseName, string containerName, string id, string partitionKey);
        Task<ItemResponse<T>?> UpdateAsync<T>(string databaseName, string containerName, string id, string partitionKey, T item);
        Task<ItemResponse<T>?> UpsertAsync<T>(string databaseName, string containerName, string id, string partitionKey, T item);
    }
}