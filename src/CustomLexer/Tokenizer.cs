using System.Text.RegularExpressions;
using System.Linq;

namespace CustomLexer
{
    public class RegexTokenizer : ITokenizer
    {
        private const string Pattern = "(?<String>[A-Za-z0-9]+)|(?<EndOfLineMark>[.?!]+)";

        public Token[] Tokenize(string input)
        {
            return Regex.Matches(input, Pattern).Cast<Match>()
                .Select(match => {
                    var tokenType = match.Groups["String"].Length > 0 ? TokenType.String
                        : TokenType.EndOfLineMark;
                    return new Token(tokenType, match.Value);
                }).ToArray();
        }
    }
}