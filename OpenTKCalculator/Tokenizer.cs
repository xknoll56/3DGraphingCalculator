using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    public enum TokenType
    {
        NUMBER = 0,
        OPERATOR,
        FUNCTION
    }
    class Token
    {
        public TokenType type;
        private double num = double.NaN;
        private char op = ' ';
        private String func = "";

        public Token(double num)
        {
            type = TokenType.NUMBER;
            this.num = num;
        }

        public Token(char op)
        {
            type = TokenType.OPERATOR;
            this.op = op;
        }

        public Token(String func)
        {
            type = TokenType.FUNCTION;
            this.func = func;
        }

        public dynamic GetData<T>()
        {
            switch(type)
            {
                case TokenType.FUNCTION:
                    return func;
                case TokenType.NUMBER:
                    return num;
                case TokenType.OPERATOR:
                    return op;
            }

            return null;
        }
    }
    class Tokenizer
    {
        public List<Token> tokens { get; }

        public Tokenizer()
        {

        }

        public void TokenizeExpression(String expression)
        {
            for(int i = 0; i<expression.Length; i++)
            {
                Console.WriteLine(expression[i]);
            }
        }


    }
}
