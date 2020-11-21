using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber.Language
{
    public class TokenQueue
    {
        private readonly LinkedList<Token> tokens;

        public TokenQueue()
        {
            tokens = new LinkedList<Token>();
        }

        public TokenQueue(IEnumerable<Token> tokens)
        {
            this.tokens = new LinkedList<Token>(tokens);
        }

        public int Count => tokens.Count;

        public void Add(Token token)
        {
            tokens.AddLast(token);
        }

        public Token? Dequeue()
        {
            var token = tokens.First;
            tokens.RemoveFirst();
            return token?.Value;
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

        public Token? Peek()
        {
            return tokens?.First?.Value;
        }

        public Token? Peek(int depth)
        {
            var token = tokens.First;
            for (int i = 0; i < depth; i++)
            {
                if (token == null)
                {
                    return null;
                }
                token = token.Next;
            }
            return token?.Value;
        }
    }
}
