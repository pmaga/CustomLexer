using CustomLexer.ByMatchesAndIndices;

namespace CustomLexer.Tests
{
    public class LexerByRegexMatchesAndIndicesTests : LexerTests
    {
        public LexerByRegexMatchesAndIndicesTests()
            : base(() => new LexerByRegexMatchesAndIndices(new RegexTokenizer()))
        {
            
        }
    }
}