using System.Collections.Generic;

namespace CustomLexer
{
    public interface ILexicalParser
    {
         IList<LexicalAnalysisResult> Parse(string input, int numberOfWordsInGroup);
    }
}