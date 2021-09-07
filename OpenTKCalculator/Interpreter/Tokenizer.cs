using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTKCalculator
{
    public enum TokenType
    {
        NUMBER = 0,
        OPERATOR,
        FUNCTION, 
        VARIABLE,
        EOF
    }
    class Token
    {
        public TokenType type;
        private double num = double.NaN;
        private char op = ' ';
        private char var = ' ';
        private char end = '\n';
        private String func = "";

        public Token(double num)
        {
            type = TokenType.NUMBER;
            this.num = num;
        }

        public Token(char c)
        {
            if (Char.ToUpper(c) == 'X' || Char.ToUpper(c) == 'Y' || Char.ToUpper(c) == 'T')
            {
                type = TokenType.VARIABLE;
                this.var = Char.ToUpper(c);
            }
            else if(c == '\n')
            {
                type = TokenType.EOF;
            }
            else
            {
                type = TokenType.OPERATOR;
                this.op = c;
            }
        }

        public Token(String func)
        {
            type = TokenType.FUNCTION;
            this.func = func;
        }

        public dynamic GetData<T>()
        {
            switch (type)
            {
                case TokenType.FUNCTION:
                    return func;
                case TokenType.NUMBER:
                    return num;
                case TokenType.OPERATOR:
                    return op;
                case TokenType.VARIABLE:
                    return var;
                case TokenType.EOF:
                    return end;
            }

            return null;
        }

        public override string ToString()
        {
            switch (type)
            {
                case TokenType.FUNCTION:
                    return "Function: "+func;
                case TokenType.NUMBER:
                    return "Number: "+num.ToString();
                case TokenType.OPERATOR:
                    return "Operator: "+op.ToString();
                case TokenType.VARIABLE:
                    return "Variable: " + var.ToString();
                case TokenType.EOF:
                    return "EOF";
            }
            return "";
        }
    }
    class Tokenizer
    {
        private List<Token> tokens;
        public List<Token> Tokens { get => tokens.ToList(); }
        private String operators = "+-*/()^÷x";
        private List<String> functions =
        new List<string>(){
            "sin",
            "cos",
            "yan",
            "asin",
            "acos",
            "atan",
            "ln",
            "log",
            "log2",
            "abs",
            "min",
            "max"
        };

        public Tokenizer()
        {
            tokens = new List<Token>();
        }

        private enum ReadingType
        {
            NUMBER = 0,
            FUNCTION,
            NONE
        }

        public void TokenizeExpression(String expression)
        {
            tokens.Clear();
            String token = "";
            ReadingType reading = ReadingType.NONE;
            for (int i = 0; i < expression.Length; i++)
            {
                char c = expression[i];
                switch (reading)
                {
                    case ReadingType.NONE:
                        if (Char.IsDigit(c))
                        {
                            token += c;
                            reading = ReadingType.NUMBER;
                        }
                        else if (Char.IsLetter(c))
                        {
                            if (Char.ToUpper(c) == 'X' || Char.ToUpper(c) == 'Y' || Char.ToUpper(c) == 'T')
                                tokens.Add(new Token(c));
                            else
                            {
                                token += c;
                                reading = ReadingType.FUNCTION;
                            }
                        }
                        else if (operators.Contains(c))
                        {
                            tokens.Add(new Token(c));
                        }
                        else if (Char.IsWhiteSpace(c))
                            continue;
                        break;
                    case ReadingType.FUNCTION:
                        if (!Char.IsLetter(c))
                        {
                            if (functions.Contains(token))
                                tokens.Add(new Token(token));
                            else
                                throw new Exception("Invalid function \""+token+"\"");
                            token = "";
                            i--;
                            reading = ReadingType.NONE;
                            continue;
                        }
                        else
                        {
                            token += c;
                        }
                        break;
                    case ReadingType.NUMBER:
                        if (!Char.IsDigit(c) && c != '.')
                        {
                            try
                            {
                                double num = Double.Parse(token);
                                tokens.Add(new Token(num));
                            }
                            catch (Exception e)
                            {
                                throw new Exception("invalid number parsig");
                            }
                            token = "";
                            i--;
                            reading = ReadingType.NONE;
                            continue;
                        }
                        else
                        {
                            token += c;
                        }
                        break;
                }
            }


            switch (reading)
            {
                case ReadingType.NUMBER:
                    try
                    {
                        double num = Double.Parse(token);
                        tokens.Add(new Token(num));
                    }
                    catch (Exception e)
                    {
                        throw new Exception("invalid number parsig");
                    }
                    break;
                case ReadingType.FUNCTION:
                    if (functions.Contains(token))
                        tokens.Add(new Token(token));
                    break;
                case ReadingType.NONE:
                    if (token.Length == 1 && operators.Contains(token))
                        tokens.Add(new Token(token[0]));
                    break;
            }

            tokens.Add(new Token('\n'));

        }


        public void PrintTokens()
        {
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }

    }
}
