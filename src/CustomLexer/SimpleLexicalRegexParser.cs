using System;
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

            var stringIndex = tokens.Where(token => token.Type == TokenType.String).Select(token => Array.IndexOf(tokens, token)).ToList();
            var endOfLineMarksIndex = tokens.Where(token => token.Type == TokenType.EndOfLineMark).Select(token => Array.IndexOf(tokens, token)).ToList();

            for (var i = 0; i < stringIndex.Count; i++)
            {
                if (i + numberOfWordsInGroup > stringIndex.Count)
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

                if (termIndices[0] == 0)
                {
                    statistics.IncrementBeginCount();
                } else if (endOfLineMarksIndex.Contains(termIndices[0] - 1))
                { 
                    statistics.IncrementBeginCount();
                }

                if (termIndices.Last() == stringIndex.Last())
                {
                    statistics.IncrementEndCount();
                } else if (endOfLineMarksIndex.Contains(termIndices.Last() + 1))
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
    }
}