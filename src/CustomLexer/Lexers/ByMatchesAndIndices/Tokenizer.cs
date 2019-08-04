using System.Text.RegularExpressions;
using System.Linq;
using CustomLexer.Lexers;

namespace CustomLexer.ByMatchesAndIndices
{
    public class RegexTokenizer : ITokenizer
    {
        public Token[] Tokenize(string input)
        {
            return Regex.Matches(input, LexerRegexPattern.Pattern).Cast<Match>()
                .Select(match => new Token(match))
                .ToArray();
        }
    }
}