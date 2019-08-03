using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace CustomLexer.Infrastructure
{
    public interface ITableStorage
    {
        Task AddManyAsync<T>(string tableName, IEnumerable<T> entities) 
            where T : class, ITableEntity, new();
    }
}