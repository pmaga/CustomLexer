using System;

namespace CustomLexer.Api.Services
{
    public class LexicalAnalysisService : ILexicalAnalysisService
    {
        private readonly ILexicalParser _parser;

        public LexicalAnalysisService(ILexicalParser parser)
        {
            _parser = parser;
        }

        public void Analyze(string input, int numberOfWordsInGroup)
        {
            ValidateInput(input);
            ValidateNumberOfWordsInGroup(numberOfWordsInGroup);
    
            var result = _parser.Parse(input, numberOfWordsInGroup);
        }

        public TsvFile AnalyzeAndGetResultAsTsv(string input, int numberOfWordsInGroup)
        {
            ValidateInput(input);
            ValidateNumberOfWordsInGroup(numberOfWordsInGroup);

            var result = _parser.Parse(input, numberOfWordsInGroup);
            throw new NotImplementedException();
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