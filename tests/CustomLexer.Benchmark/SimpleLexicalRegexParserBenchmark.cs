using System;
using System.IO;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using CustomLexer.ByMatchesAndIndices;

namespace CustomLexer.Benchmark
{
    [CoreJob]
    [WarmupCount(3)]
    [IterationCount(3)]
    public class SimpleLexicalRegexParserBenchmark
    {
        private ILexer _lexer;
        private string _content;

        [Params(1)]
        public int NumberOfWordsInGroup;

        [GlobalSetup]
        public void Setup()
        {
            _lexer = new LexerByRegexMatchesAndIndices(new RegexTokenizer());

            _content = ReadManifestData("CustomLexer.Benchmark/Resources/sherlock.txt");
        }

        [Benchmark]
        public void Sherlock() => _lexer.Parse(_content, NumberOfWordsInGroup);

        public static string ReadManifestData(string resourceName)
        {
            var assembly = typeof(SimpleLexicalRegexParserBenchmark).GetTypeInfo().Assembly;
                resourceName = resourceName.Replace("/", ".");
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}