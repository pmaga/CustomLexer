using System.Text.RegularExpressions;

namespace CustomLexer.Lexers
{
    public class Token
    {
        public readonly TokenType Type;
        public readonly string Value;

        public Token(Match match)
        {
            Type = IsString(match) ? TokenType.String : TokenType.EndOfLineMark;
            Value = match.Value;
        }

        private static bool IsString(Match match) => match.Groups["String"].Length > 0;
    }

    public enum TokenType
    {
        String,
        EndOfLineMark
    }
}