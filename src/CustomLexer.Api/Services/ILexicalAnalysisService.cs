namespace CustomLexer.Api.Services
{
    public interface ILexicalAnalysisService
    {
         void Analyze(string input, int numberOfWordsInGroup);
         TsvFile AnalyzeAndGetResultAsTsv(string input, int numberOfWordsInGroup);
    }

    public class TsvFile
    {
        public string Content { get; }

        public TsvFile(string content)
        {
            Content = content;
        }
    }
}