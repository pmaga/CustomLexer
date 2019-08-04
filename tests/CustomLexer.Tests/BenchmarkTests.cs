using System.Diagnostics;
using CustomLexer.ByMatchesAndIndices;
using CustomLexer.Lexers.ByRegexMatch;
using Xunit;
using Xunit.Abstractions;

namespace CustomLexer.Tests
{
    public class BenchmarkTests
    {
        private readonly ITestOutputHelper _output;

        public BenchmarkTests(ITestOutputHelper output)
        {
            _output = output;
      }

        [Theory]
        [EmbeddedResourceData("CustomLexer.Tests/Resources/sherlock.txt")]
        public void Benchmark(string data)
        {
            int n = 1;

            var lexer1 = new LexerByRegexMatchesAndIndices(new RegexTokenizer());
            var lexer2 = new LexerByRegexMatch();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            lexer1.Parse(data, n);

            stopwatch.Stop();
            _output.WriteLine(stopwatch.Elapsed.ToString());

            stopwatch.Reset();
            
            stopwatch.Start();

            lexer2.Parse(data, n);

            stopwatch.Stop();
            _output.WriteLine(stopwatch.Elapsed.ToString());
        }
    }
}