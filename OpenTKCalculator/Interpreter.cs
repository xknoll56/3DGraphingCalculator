using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    class Interpreter
    {
        private Tokenizer tokenizer;
        private readonly Dictionary<char, int> operatorPrecidence;
        Exception parseException = null;
        Token openBrack;
        Token closedBrack;

        public Interpreter()
        {
            tokenizer = new Tokenizer();
            operatorPrecidence = new Dictionary<char, int>();
            operatorPrecidence.Add('+', 0);
            operatorPrecidence.Add('-', 0);
            operatorPrecidence.Add('/', 1);
            operatorPrecidence.Add('*', 1);
            operatorPrecidence.Add('÷', 1);
            operatorPrecidence.Add('x', 1);
            operatorPrecidence.Add('^', 2);
            operatorPrecidence.Add('(', 3);
            operatorPrecidence.Add(')', 3);
            openBrack = new Token('(');
            closedBrack = new Token(')');
        }

        private bool FindOpenBrackets(Token tok)
        {
            return tok.GetData<char>() == '(';
        }

        private bool FindClosedBrackets(Token tok)
        {
            return tok.GetData<char>() == ')';
        }

        private void ReplaceAllVariables(List<Token> tokens, double x, double y)
        {
            for(int i = 0; i<tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.VARIABLE)
                {
                    if (Char.ToUpper(tokens[i].GetData<char>()) == 'X')
                        tokens[i] = new Token(x);
                    else if (Char.ToUpper(tokens[i].GetData<char>()) == 'Y')
                        tokens[i] = new Token(y);
                }
            }
        }

        public double EvaluateExpression(string expression)
        {
            tokenizer.TokenizeExpression(expression);
            List<Token> tokens = tokenizer.tokens;
            parseException = null;


            if(tokens.FindAll(new Predicate<Token>(FindOpenBrackets)).Count != tokens.FindAll(new Predicate<Token>(FindClosedBrackets)).Count)
            {
                parseException = new Exception("Invalid bracketing");
                return double.NaN;
            }
            return EvaluateExpressionRecursive(tokens.ToArray(), 0, 0);

        }

        public double EvaluateExpression(string expression, double x, double y)
        {
            tokenizer.TokenizeExpression(expression);
            List<Token> tokens = tokenizer.tokens;
            ReplaceAllVariables(tokens, x, y);
            parseException = null;

            if (tokens.FindAll(new Predicate<Token>(FindOpenBrackets)).Count != tokens.FindAll(new Predicate<Token>(FindClosedBrackets)).Count)
            {
                parseException = new Exception("Invalid bracketing");
                return double.NaN;
            }
            return EvaluateExpressionRecursive(tokens.ToArray(), x, y);

        }

        public double EvaluateExpressionRecursive(Token[] tokens, double x, double y)
        {
            Stack<Token> operatorStack = new Stack<Token>();
            Stack<Token> operandStack = new Stack<Token>();
            uint index = 0;
            Token tok;
            for (int i = 0; i < tokens.Length; i++)
            {
                tok = tokens[i];
                switch (tok.type)
                {
                    case TokenType.OPERATOR:
                        Token op;
                        if (operatorStack.TryPeek(out op))
                        {
                            char curOp = tok.GetData<char>();
                            if (curOp == '(')
                            {
                                tokens = EvaluateSubExpression(tokens, tok, x, y);
                                if (tokens.Length > 0)
                                {
                                    tok = tokens[i];
                                    operandStack.Push(tok);
                                }
                                break;
                            }
                            else
                            {
                                int curPrec = operatorPrecidence[curOp];
                                char prevOp = op.GetData<char>();
                                int prevPrec = operatorPrecidence[prevOp];
                                if (curPrec < prevPrec)
                                {
                                    //evaluate the operator on the top of the stack
                                    EvaluateTopOfStack(operatorStack, operandStack);
                                    i--;
                                    break;
                                }
                            }
                        }
                        operatorStack.Push(tok);
                        break;
                    case TokenType.NUMBER:
                        operandStack.Push(tok);
                        break;
                   
                }
            }

            if (parseException == null)
            {
                Token token;
                while (operatorStack.TryPeek(out token))
                {
                    EvaluateTopOfStack(operatorStack, operandStack);
                }

                return operandStack.Pop().GetData<double>();
            }
            else
                return double.NaN;
        }

        private Token[] EvaluateSubExpression(Token[] tokens, Token openingBracket, double x, double y)
        {
            List<Token> modifiedTokens = new List<Token>();
            List<Token> subExpression = new List<Token>();

            uint c = 0;
            bool s = false;
            bool evaluated = false;
            foreach(Token token in tokens)
            {
                if (!s)
                {
                    if (token.Equals(openingBracket))
                    {
                        s = true;
                        c++;
                    }
                    else
                        modifiedTokens.Add(token);
                }
                else if (!evaluated)
                {
                    if (token.type == TokenType.OPERATOR)
                    {
                        char v = token.GetData<char>();
                        if (v == '(')
                            c++;
                        else if (v == ')')
                            c--;
                    }
                    else if (token.type == TokenType.EOF)
                    {
                        parseException = new Exception("Invalid bracketing");
                        modifiedTokens = new List<Token>();
                        break;
                    }

                    //then the subexpression is found
                    if (c == 0)
                    {
                        modifiedTokens.Add(new Token(EvaluateExpressionRecursive(subExpression.ToArray(), x, y)));
                        evaluated = true;
                    }
                    else
                        subExpression.Add(token);
                }
                else
                    modifiedTokens.Add(token);
            }
            return modifiedTokens.ToArray();
        }

        private void EvaluateTopOfStack(Stack<Token> operatorStack, Stack<Token> operandStack)
        {
            char op = operatorStack.Pop().GetData<char>();
            double rhs = operandStack.Pop().GetData<double>();
            double lhs = operandStack.Pop().GetData<double>();
            double res = 0;
            switch (op)
            {
                case '+':
                    res = lhs + rhs;
                    break;
                case '-':
                    res = lhs - rhs;
                    break;
                case '/':
                    res = lhs / rhs;
                    break;
                case '÷':
                    res = lhs / rhs;
                    break;
                case '*':
                    res = lhs * rhs;
                    break;
                case 'x':
                    res = lhs * rhs;
                    break;
                case '^':
                    res = Math.Pow(lhs, rhs);
                    break;
            }
            operandStack.Push(new Token(res));

        }
    }
}
