using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomLexer.Infrastructure.Entities;

namespace CustomLexer.Infrastructure.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ITableStorage _tableStorage;

        public StatisticsRepository(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task AddManyAsync(Guid operationId, IEnumerable<LexicalAnalysisResult> statistics)
        {
            var entities = statistics
                .Select(r => StatisticsEntity.CreateFrom(r, operationId));
            await _tableStorage.AddManyAsync("statistics", entities);
        }
    }
}