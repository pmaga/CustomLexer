using System.Text.RegularExpressions;
using System.Linq;

namespace CustomLexer
{
    public class RegexTokenizer : ITokenizer
    {
        private const string SentenceCharacters = "A-Za-z0-9";

        private const string EndOfSentenceCharacters = ".?!";

        // Special characters that are a part of the word, e.g. semi-structured
        private const string SpecialCharacters = "-()";
        private readonly string Pattern = $"(?<String>[{SentenceCharacters}{SpecialCharacters}]+)|(?<EndOfLineMark>[{EndOfSentenceCharacters}]+)";

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