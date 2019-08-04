using CustomLexer.Lexers;

namespace CustomLexer.ByMatchesAndIndices
{
    public interface ITokenizer
    {
        Token[] Tokenize(string input);
    }
}