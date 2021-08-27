using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    public enum TokenType
    {
        NUMBER = 0,
        OPERATOR,
        FUNCTION, 
        VARIABLE
    }
    class Token
    {
        public TokenType type;
        private double num = double.NaN;
        private char op = ' ';
        private char var = ' ';
        private String func = "";

        public Token(double num)
        {
            type = TokenType.NUMBER;
            this.num = num;
        }

        public Token(char c)
        {
            if (Char.ToUpper(c) == 'X' || Char.ToUpper(c) == 'Y')
            {
                type = TokenType.VARIABLE;
                this.var = c;
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
            }
            return "";
        }
    }
    class Tokenizer
    {
        public List<Token> tokens { get; }
        private String operators = "+-*/()^";
        private List<String> functions =
        new List<string>(){
            "Sin",
            "Cos",
            "Tan",
            "Asin",
            "Acos",
            "Atan",
            "Ln",
            "Log",
            "Log2",
            "Abs",
            "Min",
            "Max"
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
                            token += c;
                            reading = ReadingType.FUNCTION;
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
                            if(functions.Contains(token))
                                tokens.Add(new Token(token));
                            token = "";
                            if (Char.IsDigit(c))
                            {
                                token += c;
                                reading = ReadingType.NUMBER;
                            }
                            else if (operators.Contains(c))
                            {
                                tokens.Add(new Token(c));
                                reading = ReadingType.NONE;
                            }
                            else if (c == ' ')
                                reading = ReadingType.NONE;

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
                                Console.WriteLine("Failed");
                            }
                            token = "";
                            if (Char.IsLetter(c))
                            {
                                token += c;
                                reading = ReadingType.FUNCTION;
                            }
                            else if (operators.Contains(c))
                            {
                                tokens.Add(new Token(c));
                                reading = ReadingType.NONE;
                            }
                            else if (c == ' ')
                                reading = ReadingType.NONE;
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
                        Console.WriteLine("Failed to read number");
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
