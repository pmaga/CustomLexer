using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLexer.Api.Services
{
    public interface ILexicalAnalysisService
    {
         Task AnalyzeAndStoreResultsAsync(string input, int numberOfWordsInGroup);
         List<LexicalAnalysisResult> AnalyzeAndGetResults(string input, int numberOfWordsInGroup);
    }
}