using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CustomLexer.Lexers.ByRegexMatch
{
    public class Buffer 
    {
        private readonly Queue<Token> _inner = new Queue<Token>(10);
        private readonly List<Token> _strings;
        private readonly int _desiredNumberOfStrings;

        private Match _lastMatch = null;

        private readonly Regex _regex;
        private readonly string _input;

        public bool CanContinue => _strings.Count() >= _desiredNumberOfStrings;

        public Buffer(Regex regex, string input, int numberOfStrings)
        {
            _desiredNumberOfStrings = numberOfStrings;
            _regex = regex;
            _input = input;
            _strings = new List<Token>(5);

            HydrateBuffer();
        }
        public void Move()
        {
            RemoveLastTokenAndPreceedingPunctuationMarks();
            HydrateBuffer();
        }

        private void RemoveLastTokenAndPreceedingPunctuationMarks()
        {
            while (_inner.Peek().Type == TokenType.EndOfLineMark)
            {
                _inner.Dequeue();
            }
            if (_inner.Count > 0)
            {
                _inner.Dequeue();
            }
            if (_strings.Count > 0)
            {
                _strings.RemoveAt(0);
            }
        }
        public Token PeekNextToken()
        {
            return _inner.Peek();
        }
        public IEnumerable<Token> PeekNextStringTokens()
        {
            for (int i = 0; i < _desiredNumberOfStrings; i++)
            {
                yield return _strings[i];
            }
        }

        private void HydrateBuffer()
        {
            while (_strings.Count < _desiredNumberOfStrings)
            {
                var match = TakeAndAddNextIfExists();
                if (match == null)
                {
                    break;
                }
            }
        }

        public Token PeekTokenFromNextGroup()
        {
            var match = TakeAndAddNextIfExists();
            return match != null ? _inner.Last() : null;
        }

        private Match TakeAndAddNextIfExists()
        {
            var match = NextMatch();
            if (!match.Success)
            {
                return null;
            }
            _lastMatch = match;
            var token = new Token(_lastMatch);

            _inner.Enqueue(token);
            if (token.Type == TokenType.String)
            {
                _strings.Add(token);
            }
            return _lastMatch;
        }

        private Match NextMatch()
        {
            return _lastMatch == null ? _regex.Match(_input) : _lastMatch.NextMatch();
        }
    }
}