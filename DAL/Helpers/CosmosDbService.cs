using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.Cosmos;

namespace DAL.Helpers
{
    public class CosmosDbService<T> : ICosmosDbService<T>
    {
        private readonly Container _container;

        public CosmosDbService(
            CosmosClient cosmosDbClient,
            string databaseName,
            string containerName)
        {
            _container = cosmosDbClient.GetContainer(databaseName, containerName);
        }

        public async Task<T> AddAsync(T item)
        {
            var myType = item.GetType();
            var propertyId = myType.GetProperty("id");
            return await _container.CreateItemAsync(item, new PartitionKey(propertyId.GetValue(item)?.ToString()));
        }

        public async Task<T> DeleteAsync(string id)
        {
            try
            {
                return await _container.DeleteItemAsync<T>(id, new PartitionKey(id));
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<T> GetAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<T> UpdateAsync(string id, T item)
        {
            try
            {
                return await _container.UpsertItemAsync(item, new PartitionKey(id));
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetMultipleAsync(string queryString)
        {
            try
            {
                var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));

                if (query is null) return new List<T>();

                var results = new List<T>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response.ToList());
                }

                if (!results.Any()) throw new Exception("No items found");

                return results;
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<T>> DetectDuplicate(string queryString)
        {
            try
            {
                var query = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));

                if (query is null) return new List<T>();

                var results = new List<T>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response.ToList());
                }

                // When there is a returned item a duplicate has been found.
                if (results.Any())
                    throw new Exception("This item with type: " + typeof(T) + " already exists.");

                return new List<T>();
            }
            catch (CosmosException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<PaginationItem<T>> GetMultiplePaginationAsync(string query, int maxItemCount,
            string continuationToken)
        {
            var paginationItem = new PaginationItem<T>
            {
                ContinuationToken = continuationToken,
                Items = new List<T>()
            };

            using (var resultSetIterator = _container.GetItemQueryIterator<T>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    MaxItemCount = maxItemCount
                },
                continuationToken: continuationToken))
            {
                // Execute query and get x items in the results. Then, get a continuation token to resume later
                while (resultSetIterator.HasMoreResults)
                {
                    var response = await resultSetIterator.ReadNextAsync();

                    paginationItem.Items.AddRange(response);

                    // Get continuation token once we've gotten maxItemCount results
                    if (response.Count != maxItemCount) continue;

                    paginationItem.ContinuationToken = response.ContinuationToken;
                    break;
                }
            }

            if (!paginationItem.Items.Any())
                throw new Exception("No messages found by this chatroom id");
            return paginationItem;
        }
    }
}