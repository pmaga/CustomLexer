using System.Linq;
using Shouldly;
using Xunit;

namespace CustomLexer.Tests
{
    public class SimpleLexicalRegexParserTests
    {
        [Fact]
        public void Parse_ParseEverySingleToken_CaseInsensitiveAndNoDuplicates()
        {
            var input = "Ala ma kota. czy kota ma ala?";
            var lexer = new SimpleLexicalRegexParser(new RegexTokenizer());
            
            var results = lexer.Parse(input, 1);
            results.Count.ShouldBe(4);
            AssertTermStatistics(results[0], "ala", 2, 1, 1, 1);
            AssertTermStatistics(results[1], "ma", 2, 0, 0, 0);
            AssertTermStatistics(results[2], "kota", 2, 0, 1, 0);
            AssertTermStatistics(results[3], "czy", 1, 1, 0, 0);
        }

        [Fact]
        public void Parse_ParsePairTokens_CaseInsensitiveAndNoDuplicates()
        {
            var input = "Ala ma kota. czy kota ma Ala?";
            var lexer = new SimpleLexicalRegexParser(new RegexTokenizer());
            
            var results = lexer.Parse(input, 2);
            results.Count.ShouldBe(6);
            AssertTermStatistics(results[0], "ala ma", 1, 1, 0, 1);
            AssertTermStatistics(results[1], "ma kota", 1, 0, 1, 0);
            AssertTermStatistics(results[2], "kota czy", 1, 0, 0, 0);
            AssertTermStatistics(results[3], "czy kota", 1, 1, 0, 0);
            AssertTermStatistics(results[4], "kota ma", 1, 0, 0, 0);
            AssertTermStatistics(results[5], "ma ala", 1, 0, 1, 0);
        }

        [Fact]
        public void Parse_ParseTrioTokens_CaseInsensitiveAndNoDuplicates()
        {
            var input = "Ala ma kota. czy kota ma Ala?";
            var lexer = new SimpleLexicalRegexParser(new RegexTokenizer());
            
            var results = lexer.Parse(input, 3);
            results.Count.ShouldBe(5);
            AssertTermStatistics(results[0], "ala ma kota", 1, 1, 1, 1);
            AssertTermStatistics(results[1], "ma kota czy", 1, 0, 0, 0);
            AssertTermStatistics(results[2], "kota czy kota", 1, 0, 0, 0);
            AssertTermStatistics(results[3], "czy kota ma", 1, 1, 0, 0);
            AssertTermStatistics(results[4], "kota ma ala", 1, 0, 1, 0);
        }

        [Fact]
        public void Parse_CountsAllUpperCaseCases()
        {
            var input = "Lorem Ipsum? Is. Simply Lorem, ipsum Dummy text of The Lorem ipsum. Printing";
            var lexer = new SimpleLexicalRegexParser(new RegexTokenizer());
            
            var results = lexer.Parse(input, 2);
            var loremIpsumSegment = results.First(r => r.Term == "lorem ipsum");
            AssertTermStatistics(loremIpsumSegment, "lorem ipsum", 3, 1, 2, 3);
        }

        private void AssertTermStatistics(LexicalAnalysisResult result, string expectedTerm, int expectedTermCount, 
            int expectedBeginCount, int expectedEndCount, int expectedUpperCount)
        {
            result.Term.ShouldBe(expectedTerm);
            result.TermCount.ShouldBe(expectedTermCount);
            result.BeginCount.ShouldBe(expectedBeginCount);
            result.EndCount.ShouldBe(expectedEndCount);
            result.UpperCount.ShouldBe(expectedUpperCount);
        }
    }
}