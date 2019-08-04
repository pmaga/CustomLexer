namespace CustomLexer.Lexers
{
    public static class LexerRegexPattern
    {
        private const string SentenceCharacters = "A-Za-z0-9";
        private const string EndOfSentenceCharacters = ".?!";
        // uncomment and add to first group to use Special characters that are a part of the word, e.g. semi-structured
        // private const string SpecialCharacters = "-()";
        public static string Pattern = $"(?<String>[{SentenceCharacters}]+)|(?<EndOfLineMark>[{EndOfSentenceCharacters}]+)";
    }
}