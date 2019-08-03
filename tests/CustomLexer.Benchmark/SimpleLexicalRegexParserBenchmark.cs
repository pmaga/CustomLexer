using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace CustomLexer.Benchmark
{
    [CoreJob]
    [WarmupCount(3)]
    [IterationCount(3)]
    public class SimpleLexicalRegexParserBenchmark
    {
        private SimpleLexicalRegexParser _parser;
        private string _lowVariety10MBFileContent;


        [Params(1)]
        public int NumberOfWordsInGroup;

        [GlobalSetup]
        public void Setup()
        {
            _parser = new SimpleLexicalRegexParser(new RegexTokenizer());

            _lowVariety10MBFileContent = GetFileContentAsync("input_10MB_lowvariety.txt)").GetAwaiter().GetResult();
        }

        [Benchmark]
        public void LowVariety10MBFile() => _parser.Parse(_lowVariety10MBFileContent, NumberOfWordsInGroup);

        private async Task<string> GetFileContentAsync(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"CustomLexer.Benchmark.Resources.{fileName}");
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}