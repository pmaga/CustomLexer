using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CustomLexer.Infrastructure
{
    public class TableStorage : ITableStorage
    {
        private const int BatchSize = 80;
        private readonly CloudTableClient _client;

        public TableStorage(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            _client = storageAccount.CreateCloudTableClient();
        }

        public async Task AddManyAsync<T>(string tableName, IEnumerable<T> entities) 
            where T : class, ITableEntity, new()
        {
            var table = _client.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();

            var tasks = new List<Task<IList<TableResult>>>();

            var entitiesOffset = 0;
            while (entitiesOffset < entities?.Count())
            {
                var entitiesToAdd = entities.Skip(entitiesOffset).Take(BatchSize).ToList();
                entitiesOffset += entitiesToAdd.Count;

                var operation = new TableBatchOperation();
                HydrateBatchOperation(operation, entitiesToAdd);

                tasks.Add(table.ExecuteBatchAsync(operation));
            }

            await Task.WhenAll(tasks);
        }

        private void HydrateBatchOperation<T>(TableBatchOperation operation, List<T> entities)
            where T : class, ITableEntity, new()
        {
            foreach (var entity in entities)
            {
                operation.Insert(entity);
            }
        }
    }
}