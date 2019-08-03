using BenchmarkDotNet.Running;

namespace CustomLexer.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SimpleLexicalRegexParserBenchmark>();
        }
    }
}
