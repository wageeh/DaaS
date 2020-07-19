using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Core.Repository
{
    public class CosmosDBRepository<T> : ICosmosDBRepository<T> where T : class
    {

        private readonly string Endpoint;
        private readonly string Key;
        private readonly string DatabaseId;
        private readonly string ContainerId;
        private readonly string PartitionKey;

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        public CosmosDBRepository(string _endpoint, string _key, string _databaseId, string _collectionId,string _partitionKey)
        {
            Endpoint = _endpoint;
            Key = _key;
            DatabaseId = _databaseId;
            ContainerId = _collectionId;
            PartitionKey = _partitionKey;
            this.cosmosClient = new CosmosClient(Endpoint, Key);
            this.CreateDatabaseAsync().Wait();
            this.CreateContainerAsync().Wait();
        }

        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(DatabaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }
        
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = await this.database.CreateContainerIfNotExistsAsync(ContainerId, "/"+ PartitionKey, 400);
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                var partitionKey = new PartitionKey(PartitionKey);
                ItemResponse<T> itemResponse = await this.container.ReadItemAsync<T>(id, partitionKey);
                return (T)itemResponse.Resource;
            }
            catch (Exception e)
            {
                    throw e;
            }
        }

        // for more agility & more gerneric -but not good costwise- is to use azure search 
        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var query = this.container.GetItemLinqQueryable<T>()
                .Where(predicate);
            var iterator = query.ToFeedIterator();
            List<T> results = new List<T>();
            while (iterator.HasMoreResults)
            {
                FeedResponse<T> currentResultSet = await iterator.ReadNextAsync();
                foreach (T item in currentResultSet)
                {
                    results.Add(item);
                }
            }

            return results;
        }

        public async Task<T> CreateItemAsync(T item, string partitionKeyValue)
        {            
           var partitionKey = new PartitionKey(partitionKeyValue);
            
            return (await this.container.CreateItemAsync(item, partitionKey)).Resource;
        }

        public async Task<T> UpdateItemAsync(string id, T item)
        {
            return (await this.container.ReplaceItemAsync(item,id)).Resource;
        }

        public async Task<T> DeleteItemAsync(string id, string partitionKeyValue)
        {
            PartitionKey partitionKey = new PartitionKey(partitionKeyValue);
            ItemResponse<T> item = await this.container.DeleteItemAsync<T>(id, partitionKey);
            return item.Resource;
        }

        public async Task<IEnumerable<T>> GetAllItemsAsync()
        {
            var query = this.container.GetItemLinqQueryable<T>();

            var iterator = query.ToFeedIterator();
            List<T> results = new List<T>();
            while (iterator.HasMoreResults)
            {
                while (iterator.HasMoreResults)
                {
                    FeedResponse<T> currentResultSet = await iterator.ReadNextAsync();
                    foreach (T item in currentResultSet)
                    {
                        results.Add(item);
                    }
                }
            }

            return results;
        }
    }
}
