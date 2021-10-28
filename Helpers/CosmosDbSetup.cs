using System;
using System.Threading.Tasks;
using DAL.Helpers;
using Microsoft.Azure.Cosmos;

namespace Helpers
{
    public class CosmosDbSetup<T>
    {
        public static async Task<CosmosDbService<T>> InitializeCosmosClientInstanceAsync(string containerName,
            string partitionKey)
        {
            var client = new CosmosClient(Environment.GetEnvironmentVariable("Account"),
                Environment.GetEnvironmentVariable("CosmosDbConnectionString"));
            var database =
                await client.CreateDatabaseIfNotExistsAsync(Environment.GetEnvironmentVariable("CosmosDbName"));
            await database.Database.CreateContainerIfNotExistsAsync(containerName, partitionKey);

            return new CosmosDbService<T>(client, Environment.GetEnvironmentVariable("CosmosDbName"),
                containerName);
        }
    }
}