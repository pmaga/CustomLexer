using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace CustomLexer.Infrastructure.Entities
{
    public class StatisticsEntity : TableEntity
    {
        public int TermCount { get; set; }
        public int BeginCount { get; set; }
        public int EndCount { get; set; }
        public int UpperCount { get; set; }

        public StatisticsEntity()
        {
        }

        public static StatisticsEntity CreateFrom(LexicalAnalysisResult result, Guid operationId)
        {
            return new StatisticsEntity
            {
                PartitionKey = operationId.ToString("N"),
                RowKey = result.Term,
                TermCount = result.TermCount,
                BeginCount = result.BeginCount,
                EndCount = result.EndCount,
                UpperCount = result.UpperCount
            };
        }
    }
}