using System.Collections.Generic;
using System.Linq;

namespace CustomLexer
{
    public class SimpleLexicalRegexParser : ILexicalParser
    {
        private const string Pattern = "(?<String>[A-Za-z0-9]+)|(?<EndOfLineMark>[.?!]+)";
        private readonly ITokenizer _tokenizer;

        public SimpleLexicalRegexParser(ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public IList<LexicalAnalysisResult> Parse(string input, int numberOfWordsInGroup)
        {
            var results = new Dictionary<string, LexicalAnalysisResult>();

            var tokens = _tokenizer.Tokenize(input);
            var stringIndex = new List<int>();
            var endOfLineMarksIndex = new HashSet<int>();

            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (token.Type == TokenType.String) {
                    stringIndex.Add(i);
                } else {
                    endOfLineMarksIndex.Add(i);
                }
            }

            for (var i = 0; i < stringIndex.Count; i++)
            {
                var lastPotentialIndexInTermGroup = i + numberOfWordsInGroup;
                if (lastPotentialIndexInTermGroup > stringIndex.Count)
                {
                    break;
                }

                int[] termIndices = stringIndex.Skip(i).Take(numberOfWordsInGroup).ToArray();
                var term = string.Join(" ", termIndices.Select(idx => tokens[idx].Value.ToLowerInvariant()));

                LexicalAnalysisResult statistics;
                if (results.ContainsKey(term))
                {
                    statistics = results[term];
                }
                else
                {
                    statistics = new LexicalAnalysisResult(term);
                    results.Add(term, statistics);
                }

                statistics.IncrementTermCount();

                var previousTermIndex = stringIndex[i] - 1;
                if (endOfLineMarksIndex.Contains(previousTermIndex) || stringIndex[i] == 0)
                { 
                    statistics.IncrementBeginCount();
                } 

                var indexOfTheLastTokenInGroup = GetIndexOfTheLastTokenInGroup(stringIndex, i, numberOfWordsInGroup);
                var startIndexOfTokenInNextPossibleGroup = GetStartIndexOfTokenInNextPossibleGroup(stringIndex, i, numberOfWordsInGroup);
                if (endOfLineMarksIndex.Contains(startIndexOfTokenInNextPossibleGroup) || indexOfTheLastTokenInGroup == stringIndex.Last())
                {
                    statistics.IncrementEndCount();
                }

                if (char.IsUpper(tokens[i].Value[0]))
                {
                    statistics.IncrementUpperCount();
                } 
            }

            return results.Values.ToList();
        }
        private int GetStartIndexOfTokenInNextPossibleGroup(List<int> index, int current, int numberOfWordsInGroup) => 
            index[current] + numberOfWordsInGroup;
        private int GetIndexOfTheLastTokenInGroup(List<int> index, int current, int numberOfWordsInGroup) =>
            index[current + numberOfWordsInGroup - 1];
    }
}