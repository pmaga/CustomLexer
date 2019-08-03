using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLexer.Infrastructure.Repositories
{
    public interface IStatisticsRepository
    {
        Task AddManyAsync(Guid operationId, IEnumerable<LexicalAnalysisResult> statistics);
    }
}