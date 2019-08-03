namespace CustomLexer
{
    public class LexicalAnalysisResult
    {
        public string Term { get; }
        public int TermCount { get; private set; }
        public int BeginCount { get; private set; }
        public int EndCount { get; private set; }
        public int UpperCount { get; private set; }

        public LexicalAnalysisResult(string term)
        {
            Term = term;
        }

        public void IncrementTermCount() => TermCount += 1;
        public void IncrementBeginCount() => BeginCount += 1;
        public void IncrementEndCount() => EndCount += 1;
        public void IncrementUpperCount() => UpperCount += 1;
    }
}