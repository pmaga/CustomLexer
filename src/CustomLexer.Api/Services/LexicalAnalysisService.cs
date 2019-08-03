using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomLexer.Infrastructure.Repositories;

namespace CustomLexer.Api.Services
{
    public class LexicalAnalysisService : ILexicalAnalysisService
    {
        private readonly ILexicalParser _parser;
        private readonly IStatisticsRepository _repository;

        public LexicalAnalysisService(ILexicalParser parser, IStatisticsRepository repository)
        {
            _parser = parser;
            _repository = repository;
        }

        public async Task AnalyzeAndStoreResultsAsync(string input, int numberOfWordsInGroup)
        {
            ValidateInput(input);
            ValidateNumberOfWordsInGroup(numberOfWordsInGroup);
    
            var result = _parser.Parse(input, numberOfWordsInGroup);

            var operationId = Guid.NewGuid();
            await _repository.AddManyAsync(operationId, result);
        }

        public List<LexicalAnalysisResult> AnalyzeAndGetResults(string input, int numberOfWordsInGroup)
        {
            ValidateInput(input);
            ValidateNumberOfWordsInGroup(numberOfWordsInGroup);

            var result = _parser.Parse(input, numberOfWordsInGroup);

            return result.ToList();
        }

        private void ValidateInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty.", nameof(input));
            }
        }

        private void ValidateNumberOfWordsInGroup(int numberOfWordsInGroup)
        {
            if (numberOfWordsInGroup < 1 || numberOfWordsInGroup > 3)
            {
                throw new ArgumentException("Number of words in one group must be between 1 and 3.", nameof(numberOfWordsInGroup));
            }
        }
    }
}