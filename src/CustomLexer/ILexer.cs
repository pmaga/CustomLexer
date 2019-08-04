using System.Collections.Generic;

namespace CustomLexer
{
    public interface ILexer
    {
         IList<LexicalAnalysisResult> Parse(string input, int numberOfWordsInGroup);
    }
}