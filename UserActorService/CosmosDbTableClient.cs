using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace UserActorService
{
    public class CosmosDbTableClient<T> where T : ITableEntity,new()
    {
        private readonly CloudTableClient tableClient;

        public CosmosDbTableClient()
        {
            var developmentStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AZURE_STORAGE_ACCOUNT"]);
            tableClient = developmentStorageAccount.CreateCloudTableClient();
        }

        public async Task<bool> CreateTable(string name)
        {
            var tableReference = tableClient.GetTableReference(name);
            var wasTableCreated = await tableReference.CreateIfNotExistsAsync();
            return wasTableCreated;
        }

        public async Task<bool> InsertOrMerge(string tableName,T t)
        {
            var tableInsertOrMergeOperation = TableOperation.InsertOrMerge(t);
            var tableReference = tableClient.GetTableReference(tableName);
            var tableResult = await tableReference.ExecuteAsync(tableInsertOrMergeOperation);
           return tableResult.HttpStatusCode == 201 || tableResult.HttpStatusCode == 200;
        }

        public async Task<List<T>> Get(string tableName,string partitionKey)
        {
            var tableReference = tableClient.GetTableReference(tableName);
            var query = tableReference.CreateQuery<T>().Where(TableQuery.GenerateFilterCondition("Id",QueryComparisons.Equal,partitionKey));
            var querySegmentedAsync = await tableReference.ExecuteQuerySegmentedAsync<T>(query, null);
            return querySegmentedAsync.Results;
        }
    }
}