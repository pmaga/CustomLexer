using Xunit;
using Shouldly;
using System.Linq;

namespace CustomLexer.Tests
{
    public class RegexTokenizerTests
    {
        [Fact]
        public void Tokenize_ParseAllTokens()
        {
            var input = "The quick brown fox jumps. There is no dog! Why?";
            var tokenizer = new RegexTokenizer();
            
            var tokens = tokenizer.Tokenize(input).ToList();


            tokens.Count.ShouldBe(13);
        }

        [Fact]
        public void Tokenize_ParseAllStringTokens()
        {
            var input = "The quick brown fox jumps. There is no dog! Why?";
            var tokenizer = new RegexTokenizer();
            
            var tokens = tokenizer.Tokenize(input).ToList();

            tokens.Where(token => token.Type == TokenType.String).Count().ShouldBe(10);
        }

        [Fact]
        public void Tokenize_ParseAllEndOfLineTokens()
        {
            var input = "The quick brown fox jumps. There is no dog! Why?";
            var tokenizer = new RegexTokenizer();
            
            var tokens = tokenizer.Tokenize(input).ToList();

            tokens.Where(token => token.Type == TokenType.EndOfLineMark).Count().ShouldBe(3);
        }

        [Fact]
        public void Tokenize_CreateTokensForAllLiteralsAndSpecialCharacters()
        {
            var input = "The fox. No dog! Why?";
            var tokenizer = new RegexTokenizer();
            
            var tokens = tokenizer.Tokenize(input).ToList();

            AssertToken(tokens[0], "The", TokenType.String);
            AssertToken(tokens[1], "fox", TokenType.String);
            AssertToken(tokens[2], ".", TokenType.EndOfLineMark);
            AssertToken(tokens[3], "No", TokenType.String);
            AssertToken(tokens[4], "dog", TokenType.String);
            AssertToken(tokens[5], "!", TokenType.EndOfLineMark);
            AssertToken(tokens[6], "Why", TokenType.String);
            AssertToken(tokens[7], "?", TokenType.EndOfLineMark);
        }

        private void AssertToken(Token token, string expectedValue, TokenType expectedType)
        {
            token.ShouldSatisfyAllConditions(() => token.Type.ShouldBe(expectedType),
                () => token.Value.ShouldBe(expectedValue));
        }
    }
}
