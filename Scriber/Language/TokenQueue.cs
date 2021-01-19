using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Language
{
    public class TokenQueue
    {
        private readonly LinkedList<Token> tokens;
        private readonly Token? lastToken;

        public TokenQueue(IEnumerable<Token> tokens)
        {
            this.tokens = new LinkedList<Token>(tokens);
            lastToken = this.tokens.Last?.Value;
        }

        public int Count => tokens.Count;

        public void Add(Token token)
        {
            tokens.AddLast(token);
        }

        public Token Dequeue()
        {
            var token = tokens.First;
            if (token != null)
            {
                tokens.RemoveFirst();
            }
            return token?.Value ?? CreateEndToken();
        }

        public void Dequeue(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Dequeue();
            }
        }

        public void SkipWhile(Func<Token, bool> skipFunc)
        {
            var token = tokens.First;
            while (token != null)
            {
                if (!skipFunc(token.Value))
                {
                    return;
                }
                else
                {
                    token = token.Next;
                    tokens.RemoveFirst();
                }
            }
        }

        public bool SkipSearchFor(Func<Token, bool> skipFunc, Func<Token, bool> searchFunc)
        {
            var tokenNode = tokens.First;

            while (tokenNode != null)
            {
                if (searchFunc(tokenNode.Value))
                {
                    while (tokenNode != tokens.First)
                    {
                        tokens.RemoveFirst();
                    }
                    return true;
                }
                else if (!skipFunc(tokenNode.Value))
                {
                    return false;
                }
                else
                {
                    tokenNode = tokenNode.Next;
                }
            }

            return false;
        }

        public Token Peek()
        {
            var token = tokens.First?.Value;

            if (token == null)
            {
                return CreateEndToken();
            }

            return token;
        }

        public Token Peek(int depth)
        {
            var token = tokens.First;
            for (int i = 0; i < depth; i++)
            {
                if (token == null)
                {
                    return CreateEndToken();
                }
                token = token.Next;
            }
            return token?.Value ?? CreateEndToken();
        }

        private Token CreateEndToken()
        {
            var span = lastToken == null ? new TextSpan(0, 0, 0) : lastToken.Span;
            var token = new Token(TokenType.None, span.End, span.Line, string.Empty);
            return token;
        }
    }
}
