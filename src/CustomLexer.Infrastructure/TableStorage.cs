using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CustomLexer.Infrastructure
{
    public class TableStorage : ITableStorage
    {
        private const int BatchSize = 100;
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

            var taskCount = 0;
            var taskThreshold = 200;
            var batchTasks = new List<Task<IList<TableResult>>>();
            
            for (var i = 0; i < entities.Count(); i += BatchSize)
            {
                taskCount++;
                var batchItems = entities.Skip(i)
                                        .Take(BatchSize)
                                        .ToList();
            
                var batch = new TableBatchOperation();
                HydrateBatchOperation(batch, batchItems);
            
                var task = table.ExecuteBatchAsync(batch);
                batchTasks.Add(task);
            
                if (taskCount >= taskThreshold)
                {
                    await Task.WhenAll(batchTasks);
                    taskCount = 0;
                }
            }
            
            await Task.WhenAll(batchTasks);
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