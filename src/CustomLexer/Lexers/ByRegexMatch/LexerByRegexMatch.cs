using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CustomLexer.Extensions;

namespace CustomLexer.Lexers.ByRegexMatch
{
    public class LexerByRegexMatch : ILexer
    {
        private const string TermDelimeter = " ";

        public IList<LexicalAnalysisResult> Parse(string input, int numberOfWordsInGroup)
        {
            var results = new Dictionary<string, LexicalAnalysisResult>();
       
            var regex = new Regex(LexerRegexPattern.Pattern);

            var isFirst = true;            
            var buffer = new Buffer(regex, input, numberOfWordsInGroup);
            while (buffer.CanContinue)
            {
                var firstToken = buffer.PeekNextToken();
                var tokens = buffer.PeekNextStringTokens().ToArray();
                
                var term = string.Join(TermDelimeter, tokens.Select(t => t.Value)).ToLower();
                var isBegin = isFirst || firstToken.Type == TokenType.EndOfLineMark;
                var nextGroupToken = buffer.PeekTokenFromNextGroup();
                var isEnd = nextGroupToken == null || nextGroupToken.Type == TokenType.EndOfLineMark;
                var isUpper = char.IsUpper(tokens[0].Value[0]);

                var statistics = results.GetOrAdd(term, () => new LexicalAnalysisResult(term));
                statistics.IncrementTermCount();
                if (isBegin) statistics.IncrementBeginCount();
                if (isEnd) statistics.IncrementEndCount();
                if (isUpper) statistics.IncrementUpperCount();

                buffer.Move();
                isFirst = false;
            }

            return results.Values.ToList();
        }
    }
}