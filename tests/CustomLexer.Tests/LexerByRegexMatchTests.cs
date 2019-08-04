using CustomLexer.Lexers.ByRegexMatch;

namespace CustomLexer.Tests
{
    public class LexerByRegexMatchTests : LexerTests
    {
        public LexerByRegexMatchTests()
            : base(() => new LexerByRegexMatch())
        {
            
        }
    }
}